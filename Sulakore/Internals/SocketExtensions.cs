using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Sulakore
{
    internal static class SocketExtensions
    {
        public static Task DiconnectTaskAsync(this Socket socket, bool reuseSocket)
        {
            IAsyncResult result = socket.BeginDisconnect(reuseSocket, null, null);
            return Task.Factory.FromAsync(result, socket.EndDisconnect);
        }
        public static Task ConnectTaskAsync(this Socket socket, string host, int port)
        {
            IAsyncResult result = socket.BeginConnect(host, port, null, null);
            return Task.Factory.FromAsync(result, socket.EndConnect);
        }
        public static Task<int> SendTaskAsync(this Socket socket, byte[] buffer, int offset, int size)
        {
            IAsyncResult result = socket.BeginSend(buffer,
                offset, size, SocketFlags.None, null, buffer);

            return Task.Factory.FromAsync(result, socket.EndSend);
        }
        public static Task<int> ReceiveTaskAsync(this Socket socket, byte[] buffer, int offset, int size)
        {
            IAsyncResult result = socket.BeginReceive(buffer,
                offset, size, SocketFlags.None, null, buffer);

            return Task.Factory.FromAsync(result, socket.EndReceive);
        }
    }
}