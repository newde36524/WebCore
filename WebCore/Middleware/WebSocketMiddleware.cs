using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebCore.Middleware
{
    public class WebSocketMiddleware : BaseMiddleware
    {
        public WebSocketMiddleware(RequestDelegate next) : base(next)
        {
        }

        public override async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await base.Invoke(context);
            }
            else
            {
                var cancellationToken = context.RequestAborted;
                var clientSocket = await context.WebSockets.AcceptWebSocketAsync();
                await SocketHandle(clientSocket, cancellationToken);
            }
        }
        /// <summary>
        /// websocket连接处理程序
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task SocketHandle(WebSocket webSocket, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var recMsg = await ReceiveStringAsync(webSocket, cancellationToken);//获取接收文本
                await SendStringAsync(webSocket, recMsg, cancellationToken);//回复接收文本
            }
        }
        /// <summary>
        /// 信息发送方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="msg"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task SendStringAsync(WebSocket socket, string msg, CancellationToken cancellationToken)
        {
            var buffer = Encoding.UTF8.GetBytes(msg);
            var segment = new ArraySegment<byte>(buffer);
            await socket.SendAsync(segment, WebSocketMessageType.Text, true, cancellationToken);
        }
        /// <summary>
        /// 信息接收方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken cancellationToken)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var buffer = new ArraySegment<byte>(new byte[8192]);
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
