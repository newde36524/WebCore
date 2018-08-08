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
using WebCore.Extension;
using WebCore.Internal;

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
            ChatRoom.GetInstance().Add(Guid.NewGuid().ToString(),webSocket);//加入聊天室
            while (!cancellationToken.IsCancellationRequested)
            {
                var recMsg = await webSocket.ReceiveStringAsync(cancellationToken);//获取接收文本
                //await webSocket.SendStringAsync(recMsg, cancellationToken);//回复接收文本
                ChatRoom.GetInstance().RaiseMsg(recMsg);//给每个连接发送信息
            }
        }
        
    }
}
