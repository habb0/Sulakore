/* Copyright

    GitHub(Source): https://GitHub.com/ArachisH/Sulakore

    .NET library for creating Habbo Hotel related desktop applications.
    Copyright (C) 2015 ArachisH

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

    See License.txt in the project root for license information.
*/

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

using Sulakore.Communication;
using Sulakore.Habbo.Protocol;
using Sulakore.Habbo.Protocol.Encoders;
using Sulakore.Habbo.Protocol.Encryption;

namespace Sulakore.Habbo.Web
{
    // TODO: Finish

    public class HSession : IDisposable
    {
        private readonly Uri _hotelUri;
        private readonly object _sendToServerLock;

        public event EventHandler<EventArgs> Connected;
        protected virtual void OnConnected(EventArgs e)
        {
            EventHandler<EventArgs> handler = Connected;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<EventArgs> Disconnected;
        protected virtual void OnDisconnected(EventArgs e)
        {
            EventHandler<EventArgs> handler = Disconnected;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<InterceptedEventArgs> DataToClient;
        protected virtual void OnDataToClient(InterceptedEventArgs e)
        {
            EventHandler<InterceptedEventArgs> handler = DataToClient;
            if (handler != null) handler(this, e);
        }

        public HHotel Hotel { get; }
        public string Email { get; }
        public string Password { get; }
        public CookieContainer Cookies { get; }

        public int TotalIncoming { get; private set; }

        public int Port { get; }
        public string Host { get; }
        public string[] Addresses { get; }

        private Socket Remote { get; set; }
        public HUser User { get; private set; }

        public bool IsDisposed { get; private set; }
        public bool IsAuthenticated { get; private set; }

        static HSession()
        {
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;
        }
        public HSession(string email,
            string password, HHotel hotel)
        {
            Email = email;
            Hotel = hotel;
            Password = password;
            Cookies = new CookieContainer();

            _sendToServerLock = new object();
            _hotelUri = new Uri(Hotel.ToUrl());

            Remote = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task<bool> LoginAsync()
        {
            IsAuthenticated = false;
            try
            {
                Cookies.SetCookies(_hotelUri,
                    await SKore.GetIPCookieAsync(Hotel).ConfigureAwait(false));

                byte[] postData = Encoding.UTF8
                    .GetBytes($"{{\"email\":\"{Email}\",\"password\":\"{Password}\"}}");

                var loginRequest = (HttpWebRequest)WebRequest
                    .Create($"{_hotelUri.OriginalString}/api/public/authentication/login");

                loginRequest.ContentType = "application/json;charset=UTF-8";
                loginRequest.CookieContainer = Cookies;
                loginRequest.AllowAutoRedirect = false;
                loginRequest.Method = "POST";
                loginRequest.Proxy = null;

                using (Stream requestStream = await loginRequest
                    .GetRequestStreamAsync().ConfigureAwait(false))
                {
                    await requestStream.WriteAsync(postData, 0, postData.Length)
                        .ConfigureAwait(false);
                }

                using (WebResponse loginResponse = await loginRequest.GetResponseAsync()
                    .ConfigureAwait(false))
                {
                    using (Stream loginStream = loginResponse.GetResponseStream())
                    using (var loginReader = new StreamReader(loginStream))
                    {
                        string body = await loginReader.ReadToEndAsync()
                            .ConfigureAwait(false);

                        User = HUser.Create(body);

                        string cookies = loginResponse
                            .Headers[HttpResponseHeader.SetCookie];

                        Cookies.SetCookies(_hotelUri, cookies);

                        return (IsAuthenticated = ((HttpWebResponse)loginResponse)
                            .StatusCode == HttpStatusCode.OK);
                    }
                }
            }
            catch (WebException) { return false; }
        }

        public override string ToString()
            => $"{Email}:{Password}:{Hotel.ToDomain()}";

        public bool IsConnected { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="Rc4"/> for encrypting outgoing data.
        /// </summary>
        public Rc4 LocaEncrypter { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="Rc4"/> for decrypting incoming data.
        /// </summary>
        public Rc4 RemoteDecrypter { get; set; }

        public async Task ConnectAsync()
        {
            Remote = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            //  await Remote.ConnectTaskAsync(Host, Port).ConfigureAwait(false);

            ReadIncomingAsync();

            IsConnected = true;
            OnConnected(EventArgs.Empty);
        }

        private async Task ReadIncomingAsync()
        {
            byte[] lengthBlock = new byte[4];
            //  await Remote.ReceiveTaskAsync(lengthBlock, 0, 4)
            //      .ConfigureAwait(false);

            RemoteDecrypter?.Parse(lengthBlock);
            int bodyLength = BigEndian.DecypherInt(lengthBlock);

            int bytesRead = 0;
            int totalBytesRead = 0;
            byte[] body = new byte[bodyLength];
            while (totalBytesRead != body.Length)
            {
                byte[] block = new byte[bodyLength - totalBytesRead];

                //  bytesRead = await Remote.ReceiveTaskAsync(block, 0, block.Length)
                //      .ConfigureAwait(false);

                Buffer.BlockCopy(block, 0, body, totalBytesRead, bytesRead);
                totalBytesRead += bytesRead;
            }
            RemoteDecrypter?.Parse(body);

            byte[] packet = new byte[4 + body.Length];
            Buffer.BlockCopy(lengthBlock, 0, packet, 0, 4);
            Buffer.BlockCopy(body, 0, packet, 4, body.Length);

            HandleIncoming(packet, ++TotalIncoming);
        }

        private void HandleIncoming(byte[] data, int count)
        {
            var args = new InterceptedEventArgs(ReadIncomingAsync, count, data, HDestination.Client);
            //Triggers.HandleIncoming(args);

            //if (!args.Cancel && !BlockedIncoming.Contains(args.Packet.Header))
            //{
            //    if (IncomingIntercepted != null)
            //        IncomingIntercepted(this, args);

            //    if (!args.Cancel)
            //        SendToClient(args.Packet.ToBytes());
            //}

            if (!args.WasContinued)
                ReadIncomingAsync();
        }
        
        public void Disconnect()
        {
            if (Remote.Connected)
            {
                Remote.Shutdown(SocketShutdown.Both);
                Remote.Close();
            }

            //_inStep = 0;
            //_inCache = null;
            //_shouldReceive = false;

            //RemoteDecypter = null;
            LocaEncrypter = null;

            OnDisconnected(EventArgs.Empty);
        }

        public int SendToServer(byte[] data)
        {
            lock (_sendToServerLock)
            {
                data = LocaEncrypter?.SafeParse(data) ?? data;
                return Remote.Send(data);
            }
        }
        public int SendToServer(ushort header, params object[] chunks) =>
            SendToServer(HMessage.Construct(header, chunks));

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    User = null;
                    // _inCache = null;
                    //_inBuffer = null;

                    SKore.Unsubscribe(ref Connected);
                    SKore.Unsubscribe(ref Disconnected);
                    SKore.Unsubscribe(ref DataToClient);

                    Remote.Dispose();
                }
                IsDisposed = true;
            }
        }
    }
}