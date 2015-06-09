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
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using Sulakore.Habbo.Protocol.Encoders;
using Sulakore.Habbo.Protocol.Encryption;

namespace Sulakore.Communication
{
    /// <summary>
    /// Represents a wrapper for the <see cref="Socket"/> that allows asynchronous operations using <see cref="Task"/>.
    /// </summary>
    public class HNode : IDisposable
    {
        private readonly Socket _node;
        
        /// <summary>
        /// Gets or sets the <see cref="Rc4"/> for encrypting the data being sent.
        /// </summary>
        public Rc4 Encrypter { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="Rc4"/> for decrypting the data being received.
        /// </summary>
        public Rc4 Decrypter { get; set; }
        /// <summary>
        /// Gets or sets the value that determines whether this <see cref="HNode"/> has already been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HNode"/> class.
        /// </summary>
        /// <param name="node"></param>
        public HNode(Socket node)
        {
            _node = node;
        }

        /// <summary>
        /// Sends data to a connected <see cref="HNode"/> in an asynchronous operation.
        /// </summary>
        /// <param name="buffer">An array of type <see cref="byte"/> that contains the data to be sent.</param>
        /// <returns></returns>
        public Task<int> SendAsync(byte[] buffer)
        {
            buffer = Encrypter?.SafeParse(buffer) ?? buffer;
            return SendAsync(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// Sends data to a connected <see cref="HNode"/> in an asynchronous operation.
        /// </summary>
        /// <param name="buffer">An array of type <see cref="byte"/> that contains the data to send.</param>
        /// <param name="offset">The zero-based position in the buffer parameter at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <returns></returns>
        public Task<int> SendAsync(byte[] buffer, int offset, int size)
        {
            IAsyncResult result = _node.BeginSend(buffer, offset,
                size, SocketFlags.None, null, null);

            return Task.Factory.FromAsync(result, _node.EndSend);
        }

        /// <summary>
        /// Receives an array of type <see cref="byte"/> that contains data convertible to an <see cref="HMessage"/> in an asynchronous operation.
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> ReceiveAsync()
        {
            byte[] lengthBlock = new byte[4];
            await ReceiveAsync(lengthBlock, 0, 4).ConfigureAwait(false);

            Decrypter?.Parse(lengthBlock);
            int bodyLength = BigEndian.DecypherInt(lengthBlock);

            int bytesRead = 0;
            int totalBytesRead = 0;
            byte[] body = new byte[bodyLength];
            while (totalBytesRead != body.Length)
            {
                byte[] block = new byte[bodyLength - totalBytesRead];

                bytesRead = await ReceiveAsync(block, 0, block.Length)
                    .ConfigureAwait(false);

                Buffer.BlockCopy(block, 0, body, totalBytesRead, bytesRead);
                totalBytesRead += bytesRead;
            }
            Decrypter?.Parse(body);

            byte[] packet = new byte[4 + body.Length];
            Buffer.BlockCopy(lengthBlock, 0, packet, 0, 4);
            Buffer.BlockCopy(body, 0, packet, 4, body.Length);

            return packet;
        }
        /// <summary>
        /// Receives the specified number of bytes from a bound <see cref="HNode"/> into the specified offset position of the receive buffer in an asynchronous operation.
        /// </summary>
        /// <param name="buffer">An array of type <see cref="byte"/> that is the storage location for received data.</param>
        /// <param name="offset">The location in buffer to store the received data.</param>
        /// <param name="size">The number of bytes to receive.</param>
        /// <returns></returns>
        public Task<int> ReceiveAsync(byte[] buffer, int offset, int size)
        {
            IAsyncResult result = _node.BeginReceive(buffer, offset,
                size, SocketFlags.None, null, null);

            return Task.Factory.FromAsync(result, _node.EndReceive);
        }

        /// <summary>
        /// Returns a <see cref="HNode"/> that was intercepted on the specified port in an asynchronous operation.
        /// </summary>
        /// <param name="port">The port to listen for local connection attempts.</param>
        /// <returns></returns>
        public static async Task<HNode> InterceptAsync(int port)
        {
            var listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();

            var node = new HNode(await listener.AcceptSocketAsync());
            listener.Stop();

            return node;
        }
        /// <summary>
        /// Returns a <see cref="HNode"/> connected with the specified host/port in an asynchronous operation.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static async Task<HNode> ConnectAsync(string host, int port)
        {
            var socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            IAsyncResult result = socket.BeginConnect(host, port, null, null);
            await Task.Factory.FromAsync(result, socket.EndConnect);

            return new HNode(socket);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="HNode"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        /// <summary>
        /// Releases all resources used by the <see cref="HNode"/>.
        /// </summary>
        /// <param name="disposing">The value that determines whether managed resources should be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_node != null)
                    {
                        _node.Shutdown(SocketShutdown.Both);
                        _node.Close();
                    }

                    Encrypter = null;
                    Decrypter = null;
                }
                IsDisposed = true;
            }
        }
    }
}