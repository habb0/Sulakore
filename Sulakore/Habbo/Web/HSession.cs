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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Sulakore.Communication;
using Sulakore.Habbo.Protocol;

namespace Sulakore.Habbo.Web
{
    public class HSession : IDisposable
    {
        private readonly Uri _hotelUri;
        private readonly object _disconnectLock;

        /// <summary>
        /// Occurs when the connection to the remote <see cref="HNode"/> has been established.
        /// </summary>
        public event EventHandler<EventArgs> Connected;
        /// <summary>
        /// Raises the <see cref="Connected"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnConnected(EventArgs e)
        {
            EventHandler<EventArgs> handler = Connected;
            if (handler != null) handler(this, e);
        }
        /// <summary>
        /// Occurs when either client/server have been disconnected, or when <see cref="Disconnect"/> has been called if the <see cref="HConnection"/> is currently connected.
        /// </summary>
        public event EventHandler<EventArgs> Disconnected;
        /// <summary>
        /// Raises the <see cref="Disconnected"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnDisconnected(EventArgs e)
        {
            EventHandler<EventArgs> handler = Disconnected;
            if (handler != null) handler(this, e);
        }
        /// <summary>
        /// Occurs when incoming data from the server has been intercepted.
        /// </summary>
        public event EventHandler<InterceptedEventArgs> DataIncoming;
        /// <summary>
        /// Raises the <see cref="DataIncoming"/> event.
        /// </summary>
        /// <param name="e">An <see cref="InterceptedEventArgs"/> that contains the event data.</param>
        /// <returns></returns>
        protected virtual void OnDataIncoming(InterceptedEventArgs e)
        {
            EventHandler<InterceptedEventArgs> handler = DataIncoming;
            if (handler != null) handler(this, e);
        }

        public HHotel Hotel { get; }
        public string Email { get; }
        public string Password { get; }
        public HTriggers Triggers { get; }
        public CookieContainer Cookies { get; }

        public HUser User { get; private set; }
        public HNode Remote { get; private set; }
        public string SsoTicket { get; private set; }
        public int TotalIncoming { get; private set; }
        public HGameData GameData { get; private set; }

        public bool IsDisposed { get; private set; }
        public bool IsConnected { get; private set; }
        public bool IsAuthenticated { get; private set; }

        private bool _isReading;
        public bool IsReading
        {
            get { return _isReading; }
            set
            {
                if (_isReading == value)
                    return;

                if (_isReading = value)
                    ReadIncomingAsync();
            }
        }

        static HSession()
        {
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;
        }
        public HSession(string email, string password, HHotel hotel)
        {
            Email = email;
            Hotel = hotel;
            Password = password;
            Triggers = new HTriggers(false);
            Cookies = new CookieContainer();

            _disconnectLock = new object();
            _hotelUri = new Uri(Hotel.ToUrl());
        }

        public void Disconnect()
        {
            if (IsConnected && Monitor.TryEnter(_disconnectLock))
            {
                try
                {
                    IsReading = false;
                    Remote.Dispose();

                    IsConnected = false;
                    OnDisconnected(EventArgs.Empty);

                    TotalIncoming = 0;
                }
                finally
                {
                    Monitor.Exit(_disconnectLock);
                }
            }
        }
        public async Task ConnectAsync()
        {
            Remote = await HNode.ConnectAsync(GameData.Host, GameData.Port)
                .ConfigureAwait(false);

            IsConnected = true;
            OnConnected(EventArgs.Empty);

            IsReading = true;
        }
        public async Task<bool> LoginAsync()
        {
            IsAuthenticated = false;
            Cookies.SetCookies(_hotelUri, await SKore.GetIPCookieAsync(Hotel).ConfigureAwait(false));
            byte[] postData = Encoding.UTF8.GetBytes($"{{\"email\":\"{Email}\",\"password\":\"{Password}\"}}");

            var loginRequest = (HttpWebRequest)WebRequest.Create($"{_hotelUri.OriginalString}/api/public/authentication/login");
            loginRequest.ContentType = "application/json;charset=UTF-8";
            loginRequest.CookieContainer = Cookies;
            loginRequest.AllowAutoRedirect = false;
            loginRequest.Method = "POST";
            loginRequest.Proxy = null;

            using (Stream requestStream = await loginRequest.GetRequestStreamAsync().ConfigureAwait(false))
                await requestStream.WriteAsync(postData, 0, postData.Length).ConfigureAwait(false);

            using (WebResponse loginResponse = await loginRequest.GetResponseAsync().ConfigureAwait(false))
            using (Stream loginStream = loginResponse.GetResponseStream())
            using (var loginReader = new StreamReader(loginStream))
            {
                string body = await loginReader.ReadToEndAsync().ConfigureAwait(false);

                User = HUser.Create(body);
                Cookies.SetCookies(_hotelUri, loginResponse.Headers["Set-Cookie"]);
                IsAuthenticated = ((HttpWebResponse)loginResponse).StatusCode == HttpStatusCode.OK;

                if (IsAuthenticated)
                    await ExtractGameDataAsync().ConfigureAwait(false);

                return IsAuthenticated;
            }
        }

        public Task<int> SendToServerAsync(byte[] data)
        {
            return Remote.SendAsync(data);
        }
        public Task<int> SendToServerAsync(ushort header, params object[] chunks)
        {
            return Remote.SendAsync(HMessage.Construct(header, chunks));
        }

        private async Task ReadIncomingAsync()
        {
            byte[] packet = await Remote.ReceiveAsync().ConfigureAwait(false);
            HandleIncoming(packet, ++TotalIncoming);
        }
        private async Task ExtractGameDataAsync()
        {
            var clientUrlRequest = (HttpWebRequest)WebRequest.Create($"{_hotelUri.OriginalString}/api/client/clienturl");
            clientUrlRequest.ContentType = "application/json;charset=UTF-8";
            clientUrlRequest.CookieContainer = Cookies;
            clientUrlRequest.AllowAutoRedirect = false;
            clientUrlRequest.Method = "GET";
            clientUrlRequest.Proxy = null;

            using (WebResponse clientUrlResponse = await clientUrlRequest.GetResponseAsync().ConfigureAwait(false))
            using (Stream clientUrlStream = clientUrlResponse.GetResponseStream())
            using (var clientUrlReader = new StreamReader(clientUrlStream))
            {
                string clientUrl = await clientUrlReader.ReadToEndAsync();
                clientUrl = clientUrl.GetChild("{\"clienturl\":\"", '"');

                using (var client = new WebClient())
                {
                    client.Proxy = null;
                    client.Headers["Cookie"] =
                        clientUrlRequest.Headers["Cookie"];

                    SsoTicket = clientUrl.Split('/').Last();
                    string clientBody = await client.DownloadStringTaskAsync(clientUrl);
                    GameData = new HGameData(clientBody);
                }
            }
        }
        private void HandleIncoming(byte[] data, int count)
        {
            var args = new InterceptedEventArgs(ReadIncomingAsync, count, data, HDestination.Client);
            Triggers.HandleIncoming(args);

            OnDataIncoming(args);

            if (!args.WasContinued && IsReading)
                ReadIncomingAsync();
        }

        public override string ToString()
            => $"{Email}:{Password}:{Hotel.ToDomain()}";

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
                    Triggers.Dispose();
                    SKore.Unsubscribe(ref Connected);
                    SKore.Unsubscribe(ref Disconnected);
                    SKore.Unsubscribe(ref DataIncoming);
                    Disconnect();
                }
                IsDisposed = true;
            }
        }
    }
}