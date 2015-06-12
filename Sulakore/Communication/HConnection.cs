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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Sulakore.Habbo.Headers;
using Sulakore.Habbo.Protocol;
using Sulakore.Habbo.Protocol.Encoders;

namespace Sulakore.Communication
{
    /// <summary>
    /// Represents a connection handler to intercept incoming/outgoing data from a post-shuffle hotel.
    /// </summary>
    public class HConnection : IDisposable
    {
        private readonly object _disconnectLock;
        private static readonly string _hostsFile;

        /// <summary>
        /// Occurs when the intercepted local node initiates the handshake with the server.
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
        /// <summary>
        /// Occurs when outgoing data from the client has been intercepted.
        /// </summary>
        public event EventHandler<InterceptedEventArgs> DataOutgoing;
        /// <summary>
        /// Raises the <see cref="DataOutgoing"/> event.
        /// </summary>
        /// <param name="e">An <see cref="InterceptedEventArgs"/> that contains the event data.</param>
        /// <returns></returns>
        protected virtual void OnDataOutgoing(InterceptedEventArgs e)
        {
            EventHandler<InterceptedEventArgs> handler = DataOutgoing;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Gets the <see cref="HTriggers"/> that handles the in-game callbacks/events.
        /// </summary>
        public HTriggers Triggers { get; }
        /// <summary>
        /// Gets the <see cref="HNode"/> representing the local connection.
        /// </summary>
        public HNode Local { get; private set; }
        /// <summary>
        /// Gets the <see cref="HNode"/> representing the remote connection.
        /// </summary>
        public HNode Remote { get; private set; }
        /// <summary>
        /// Gets a value that determines whether the <see cref="HConnection"/> has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// Gets a value that determines whether the <see cref="HConnection"/> has established a connection with the game.
        /// </summary>
        public bool IsConnected { get; private set; }
        /// <summary>
        /// Gets the <see cref="IList{T}"/> that contains the headers of outgoing packets to block.
        /// </summary>
        public IList<ushort> BlockedOutgoing { get; }
        /// <summary>
        /// Gets the <see cref="IList{T}"/> that contains the headers of incoming packets to block.
        /// </summary>
        public IList<ushort> BlockedIncoming { get; }
        /// <summary>
        /// Gets the <see cref="IDictionary{TKey, TValue}"/> that contains the replacement data for an outgoing packet determined by the header.
        /// </summary>
        public IDictionary<ushort, byte[]> ReplacedOutgoing { get; }
        /// <summary>
        /// Gets the <see cref="IDictionary{TKey, TValue}"/> that contains the replacement data for an incoming packet determined by the header.
        /// </summary>
        public IDictionary<ushort, byte[]> ReplacedIncoming { get; }
        /// <summary>
        /// Gets the total amount of packets that have been intercepted going to the server.
        /// </summary>
        public int TotalOutgoing { get; private set; }
        /// <summary>
        /// Gets the total amount of packets that have been intercepted going to the client.
        /// </summary>
        public int TotalIncoming { get; private set; }

        static HConnection()
        {
            _hostsFile = Environment.GetFolderPath(
                Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts";
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HConnection"/> class.
        /// </summary>
        public HConnection()
        {
            _disconnectLock = new object();

            Triggers = new HTriggers(false);
            BlockedOutgoing = new List<ushort>();
            BlockedIncoming = new List<ushort>();
            ReplacedOutgoing = new Dictionary<ushort, byte[]>();
            ReplacedIncoming = new Dictionary<ushort, byte[]>();

            if (!File.Exists(_hostsFile))
                File.Create(_hostsFile).Dispose();

            File.SetAttributes(_hostsFile, FileAttributes.Normal);
        }

        /// <summary>
        /// Disconnects all concurrent connections.
        /// </summary>
        public void Disconnect()
        {
            if (IsConnected && Monitor.TryEnter(_disconnectLock))
            {
                try
                {
                    Local.Dispose();
                    Remote.Dispose();

                    IsConnected = false;
                    OnDisconnected(EventArgs.Empty);
                    TotalIncoming = TotalOutgoing = 0;
                }
                finally
                {
                    Monitor.Exit(_disconnectLock);
                    RestoreHosts();
                }
            }
            else return;
        }
        /// <summary>
        /// Intercepts the attempted connection on the specified port, and establishes a connection with the host in an asynchronous operation.
        /// </summary>
        /// <param name="host">The host to establish a connection with.</param>
        /// <param name="port">The port to intercept the local connection attempt.</param>
        /// <returns></returns>
        public async Task ConnectAsync(string host, int port)
        {
            RestoreHosts();
            while (true)
            {
                File.AppendAllText(_hostsFile, $"127.0.0.1\t\t{host}\t\t#Sulakore");
                Local = await HNode.InterceptAsync(port).ConfigureAwait(true);

                RestoreHosts();
                Remote = await HNode.ConnectAsync(host, port).ConfigureAwait(false);

                byte[] buffer = new byte[6];
                int length = await Local.ReceiveAsync(buffer, 0, buffer.Length)
                    .ConfigureAwait(false);

                if (BigEndian.DecypherShort(buffer, 4) == Outgoing.CLIENT_CONNECT)
                {
                    IsConnected = true;
                    OnConnected(EventArgs.Empty);

                    byte[] packet = new byte[BigEndian.DecypherInt(buffer) + 4];
                    Buffer.BlockCopy(buffer, 0, packet, 0, 6);

                    await Local.ReceiveAsync(packet, 6, packet.Length - 6)
                        .ConfigureAwait(false);

                    HandleOutgoing(packet, ++TotalOutgoing);
                    ReadIncomingAsync();
                    break;
                }

                byte[] newBuffer = new byte[1000];
                Buffer.BlockCopy(buffer, 0, newBuffer, 0, 6);
                length = await Local.ReceiveAsync(newBuffer, 6, newBuffer.Length - 6)
                    .ConfigureAwait(false);

                await Remote.SendAsync(newBuffer, 0, length + 6)
                    .ConfigureAwait(false);

                int inLength = await Remote.ReceiveAsync(newBuffer, 0, newBuffer.Length)
                    .ConfigureAwait(false);

                await Local.SendAsync(newBuffer, 0, inLength)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Sends data to the remote <see cref="HNode"/> in an asynchronous operation.
        /// </summary>
        /// <param name="data">The data to send to the node.</param>
        /// <returns></returns>
        public Task<int> SendToServerAsync(byte[] data)
        {
            return Remote.SendAsync(data);
        }
        /// <summary>
        /// Sends data to the remote <see cref="HNode"/> using the specified header, and chunks in an asynchronous operation.
        /// </summary>
        /// <param name="header">The header to be used for the construction of the packet.</param>
        /// <param name="chunks">The chunks/values that the packet will carry.</param>
        /// <returns></returns>
        public Task<int> SendToServerAsync(ushort header, params object[] chunks)
        {
            return Remote.SendAsync(HMessage.Construct(header, chunks));
        }

        /// <summary>
        /// Sends data to the local <see cref="HNode"/> in an asynchronous operation.
        /// </summary>
        /// <param name="data">The data to send to the node.</param>
        /// <returns></returns>
        public Task<int> SendToClientAsync(byte[] data)
        {
            return Local.SendAsync(data);
        }
        /// <summary>
        /// Sends data to the local <see cref="HNode"/> using the specified header, and chunks in an asynchronous operation.
        /// </summary>
        /// <param name="header">The header to be used for the construction of the packet.</param>
        /// <param name="chunks">The chunks/values that the packet will carry.</param>
        /// <returns></returns>
        public Task<int> SendToClientAsync(ushort header, params object[] chunks)
        {
            return Local.SendAsync(HMessage.Construct(header, chunks));
        }

        private async Task ReadOutgoingAsync()
        {
            byte[] packet = await Local.ReceiveAsync().ConfigureAwait(false);
            HandleOutgoing(packet, ++TotalOutgoing);
        }
        private void HandleOutgoing(byte[] data, int count)
        {
            var args = new InterceptedEventArgs(ReadOutgoingAsync, count, data, HDestination.Server);
            Triggers.HandleOutgoing(args);

            if (!args.Cancel && !BlockedOutgoing.Contains(args.Packet.Header))
            {
                OnDataOutgoing(args);

                if (!args.Cancel)
                    SendToServerAsync(args.Packet.ToBytes());
            }

            if (!args.WasContinued)
                ReadOutgoingAsync();
        }

        private async Task ReadIncomingAsync()
        {
            byte[] packet = await Remote.ReceiveAsync().ConfigureAwait(false);
            HandleIncoming(packet, ++TotalIncoming);
        }
        private void HandleIncoming(byte[] data, int count)
        {
            var args = new InterceptedEventArgs(ReadIncomingAsync, count, data, HDestination.Client);
            Triggers.HandleIncoming(args);

            if (!args.Cancel && !BlockedIncoming.Contains(args.Packet.Header))
            {
                OnDataIncoming(args);

                if (!args.Cancel)
                    SendToClientAsync(args.Packet.ToBytes());
            }

            if (!args.WasContinued)
                ReadIncomingAsync();
        }

        /// <summary>
        /// Removes all lines from the hosts file containing: #Sulakore
        /// </summary>
        public static void RestoreHosts()
        {
            string[] lines = File.ReadAllLines(_hostsFile)
                .Where(line =>
                !line.EndsWith("#Sulakore") &&
                !string.IsNullOrWhiteSpace(line)).ToArray();

            File.WriteAllLines(_hostsFile, lines);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="HConnection"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        /// <summary>
        /// Releases all resources used by the <see cref="HConnection"/>.
        /// </summary>
        /// <param name="disposing">The value that determines whether managed resources should be disposed.</param>
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
                    SKore.Unsubscribe(ref DataOutgoing);
                    Disconnect();
                }
                IsDisposed = true;
            }
        }
    }
}