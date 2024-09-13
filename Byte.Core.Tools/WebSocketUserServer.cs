using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Tools
{
    //
    public class WebSocketServer
    {
        private static ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> _sockets = new ConcurrentDictionary<string, System.Net.WebSockets.WebSocket>();


        public ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> UserSockets => _sockets;
        public bool UserSet( string socketId, WebSocket socket )
        {
          return  _sockets.TryAdd(socketId, socket);
        }
        public bool UserRomeve(string socketId)
        {
          return  _sockets.TryRemove(socketId,out _);
        }
        public bool UserContainsKey(string socketId)
        {
            return _sockets.ContainsKey(socketId);
        }
         

        //public void SendData(byte[] data)
        //{
        //    _socket.Send(data);
        //}

        //public int ReceiveData(byte[] buffer)
        //{
        //    return _socket.Receive(buffer);
        //}

    }


}
