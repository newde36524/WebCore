using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Text;
using System.Threading.Tasks;

namespace WebCore.CustomerActionResult
{
    public class MyJsonResultExecutor : IActionResultExecutor<MyJsonResult>
    {
        private static readonly string DefaultContentType = new MediaTypeHeaderValue("application/json")
        {
            Encoding = Encoding.UTF8
        }.ToString();

        private readonly IHttpResponseStreamWriterFactory _writerFactory;
        private readonly MvcJsonOptions _options;
        private readonly IArrayPool<char> _charPool;

        public MyJsonResultExecutor(IHttpResponseStreamWriterFactory writerFactory, IOptions<MvcJsonOptions> options, ArrayPool<char> charPool)
        {
            if (writerFactory == null)
            {
                throw new ArgumentNullException(nameof(writerFactory));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (charPool == null)
            {
                throw new ArgumentNullException(nameof(charPool));
            }
            
            _writerFactory = writerFactory;
            _options = options.Value;
            _charPool = new JsonArrayPool<char>(charPool);
        }

        public async Task ExecuteAsync(ActionContext context, MyJsonResult result)
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

            //ResponseContentTypeHelper.ResolveContentTypeAndEncoding(
            //    null,
            //    response.ContentType,
            //    DefaultContentType,
            //    out var resolvedContentType,
            //    out var resolvedContentTypeEncoding);

            response.ContentType = response.ContentType ?? DefaultContentType;
            var defaultContentTypeEncoding = MediaType.GetEncoding(response.ContentType);

            var serializerSettings = _options.SerializerSettings;

            using (var writer = _writerFactory.CreateWriter(response.Body, defaultContentTypeEncoding))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.ArrayPool = _charPool;
                    jsonWriter.CloseOutput = false;
                    jsonWriter.AutoCompleteOnClose = false;

                    var jsonSerializer = JsonSerializer.Create(serializerSettings);
                    jsonSerializer.Serialize(jsonWriter, result.Value);
                }

                await writer.FlushAsync();
            }
        }
    }

}
