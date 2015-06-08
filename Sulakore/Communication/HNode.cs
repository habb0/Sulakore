using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using Sulakore.Habbo.Protocol.Encoders;
using Sulakore.Habbo.Protocol.Encryption;

namespace Sulakore.Communication
{
    public class HNode : IDisposable
    {
        private readonly object _sendLock;

        /// <summary>
        /// Gets the <see cref="Socket"/> that is being sent data, and receiving data.
        /// </summary>
        public Socket Node { get; }
        /// <summary>
        /// Gets or sets the <see cref="Rc4"/> for encrypting data being sent.
        /// </summary>
        public Rc4 Encrypter { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="Rc4"/> for decrypting data being received.
        /// </summary>
        public Rc4 Decrypter { get; set; }
        /// <summary>
        /// Gets or sets the value that determines whether this <see cref="HNode"/> has already been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        public HNode(Socket node)
        {
            Node = node;

            _sendLock = new object();
        }

        public Task<int> SendAsync(byte[] data)
        {
            lock (_sendLock)
                data = Encrypter?.SafeParse(data) ?? data;

            return SendAsync(data, 0, data.Length);
        }
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

        public Task<int> SendAsync(byte[] buffer, int offset, int size)
        {
            IAsyncResult result = Node.BeginSend(buffer, 0, buffer.Length,
                SocketFlags.None, null, null);

            return Task.Factory.FromAsync(result, Node.EndSend);
        }
        public Task<int> ReceiveAsync(byte[] buffer, int offset, int size)
        {
            IAsyncResult result = Node.BeginReceive(buffer, offset,
                size, SocketFlags.None, null, null);

            return Task.Factory.FromAsync(result, Node.EndReceive);
        }

        public static async Task<HNode> InterceptAsync(int port)
        {
            var listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();

            var node = new HNode(await listener.AcceptSocketAsync());
            listener.Stop();

            return node;
        }
        public static async Task<HNode> ConnectAsync(string host, int port)
        {
            var socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            IAsyncResult result = socket.BeginConnect(host, port, null, null);
            await Task.Factory.FromAsync(result, socket.EndConnect);

            return new HNode(socket);
        }

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
                    if (Node != null)
                    {
                        Node?.Shutdown(SocketShutdown.Both);
                        Node?.Close();
                    }

                    Encrypter = null;
                    Decrypter = null;
                }
                IsDisposed = true;
            }
        }
    }
}