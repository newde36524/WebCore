using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebCore.Extension
{
    public static class WebSocketExtension
    {
        /// <summary>
        /// 文本信息发送方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="msg"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task SendStringAsync(this WebSocket socket, string msg, CancellationToken cancellationToken)
        {
            var buffer = Encoding.UTF8.GetBytes(msg);
            var segment = new ArraySegment<byte>(buffer);
            await socket.SendAsync(segment, WebSocketMessageType.Text, true, cancellationToken);
        }
        /// <summary>
        /// 文本信息接收方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<string> ReceiveStringAsync(this WebSocket socket, CancellationToken cancellationToken)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ArraySegment<byte> buffer = new byte[8192];
                WebSocketReceiveResult result;
                do
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    result = await socket.ReceiveAsync(buffer, cancellationToken);
                    await ms.WriteAsync(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return string.Empty;
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

    }
}
