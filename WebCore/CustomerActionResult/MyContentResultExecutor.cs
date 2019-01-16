using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace WebCore.CustomerActionResult
{
    public class MyContentResultExecutor : IActionResultExecutor<MyContentResult>
    {
        private const string DefaultContentType = "text/plain; charset=utf-8";
        private readonly IHttpResponseStreamWriterFactory _httpResponseStreamWriterFactory;

        public MyContentResultExecutor(IHttpResponseStreamWriterFactory httpResponseStreamWriterFactory)
        {
            _httpResponseStreamWriterFactory = httpResponseStreamWriterFactory;
        }

        public async Task ExecuteAsync(ActionContext context, MyContentResult result)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }
            var response = context.HttpContext.Response;
            ResponseContentTypeHelper.ResolveContentTypeAndEncoding(
                null,
                response.ContentType,
                DefaultContentType,
                out var resolvedContentType,
                out var resolvedContentTypeEncoding);
            response.ContentType = resolvedContentType;
            var defaultContentTypeEncoding = MediaType.GetEncoding(response.ContentType);
            if (result.Content != null)
            {
                string content = JsonConvert.SerializeObject(result.Content);
                response.ContentLength = resolvedContentTypeEncoding.GetByteCount(content);
                using (var textWriter = _httpResponseStreamWriterFactory.CreateWriter(response.Body, resolvedContentTypeEncoding))
                {
                    await textWriter.WriteAsync(content);
                    await textWriter.FlushAsync();
                }
            }
        }
    }

}
