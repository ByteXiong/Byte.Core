using System.Collections.Concurrent;
using System.Net.WebSockets;


namespace Byte.Core.Api.Common
{
    public class WebSocketManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public void AddSocket(string id, WebSocket socket)
        {
            _sockets.TryAdd(id, socket);
        }

        public async Task RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out WebSocket socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by WebSocketManager", CancellationToken.None);
        }

        public WebSocket GetSocketById(string id)
        {
            _sockets.TryGetValue(id, out WebSocket socket);
            return socket;
        }

        public IEnumerable<WebSocket> GetAllSockets()
        {
            return _sockets.Values;
        }
    }
}