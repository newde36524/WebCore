using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebCore.Extension;

namespace WebCore.Internal
{
    internal class ChatRoom : IChatRoom
    {
        private ChatRoom() { }

        private static ChatRoom _chatRoom { get; set; } = new ChatRoom();

        public static ChatRoom GetInstance() => _chatRoom;

        Dictionary<string, WebSocket> _dic = new Dictionary<string, WebSocket>();//临时客户端存储

        public void Add(string key, WebSocket webSocket)
        {
            if (!_dic.ContainsKey(key))
            {
                _dic.Add(key, webSocket);
            }
        }

        public bool Remove(string key)
        {
            return _dic.ContainsKey(key) ? _dic.Remove(key) : _dic.ContainsKey(key);
        }

        public async void RaiseMsg(string msg)
        {
            foreach (var client in _dic)
            {
                await client.Value.SendStringAsync(msg, CancellationToken.None);
            }
        }

    }

    public interface IChatRoom
    {
        void Add(string key, WebSocket webSocket);
        bool Remove(string key);
        void RaiseMsg(string msg);
    }

}
