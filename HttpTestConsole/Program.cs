using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HttpTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                int i = 32;
                ref int f = ref i;
                f++;
                Console.WriteLine(i);
            }
            {
                string input = "sss,ddd";
                ReadOnlySpan<char> inputSpan = input.AsSpan();
                int commaPos = input.IndexOf(',');
                int first = int.Parse(inputSpan.Slice(0, commaPos));
                int second = int.Parse(inputSpan.Slice(commaPos + 1));
            }
            {
                string input = "sss,ddd";
                foreach (char c in input.AsSpan().Slice(2, 3))
                {
                    c = null;
                };
                //System.Buffers.Text.Base64
                //System.Buffers.Text.Utf8Parser
                //System.Buffers.Text.Utf8Formatter
                //System.Security.Cryptography.Rfc2898DeriveBytes
                //Span<byte> bytes = stackalloc byte[3];
            }


            #region 普通Get请求

            using (HttpClient httpClient = new HttpClient())
            {
                string url = "http://localhost:51582/TestController/GetResult";
                httpClient.GetAsync(url).ContinueWith(x =>
                {
                    x.Result.Content.ReadAsStringAsync().ContinueWith(r =>
                    {
                        Console.WriteLine(r.Result);
                    });
                }).GetAwaiter().GetResult();
            }

            #endregion

            #region Get带参数

            using (HttpClient httpClient = new HttpClient())
            {
                string url = "http://localhost:51582/TestController/GetReultWithParam?msg=测试&msg2=测试2";

                httpClient.GetAsync(url).ContinueWith(x =>
                {
                    x.Result.Content.ReadAsStringAsync().ContinueWith(r =>
                    {
                        Console.WriteLine(r.Result);
                    });
                }).GetAwaiter().GetResult();
            }

            #endregion

            #region 普通Post请求

            using (HttpClient httpClient = new HttpClient())
            {
                string url = "http://localhost:51582/TestController/PostResult";
                httpClient.PostAsync(url, null).ContinueWith(x =>
                {
                    x.Result.Content.ReadAsStringAsync().ContinueWith(r =>
                    {
                        Console.WriteLine(r.Result);
                    });
                }).GetAwaiter().GetResult();
            }

            #endregion

            #region Post带参数

            using (HttpClient httpClient = new HttpClient())
            {
                //FormUrlEncodedContent 和 MultipartFormDataContent 两种方式模拟表单提交
                string url = "http://localhost:51582/TestController/PostResultWithParam";

                {//方式一
                    var multipart = new MultipartFormDataContent();
                    var msg = new StringContent("123");
                    var msg2 = new StringContent("456");
                    multipart.Add(msg, "msg");
                    multipart.Add(msg2, "msg2");
                    httpClient.PostAsync(url, multipart).ContinueWith(x =>
                    {
                        x.Result.Content.ReadAsStringAsync().ContinueWith(r =>
                        {
                            Console.WriteLine(r.Result);
                        });
                    }).GetAwaiter().GetResult();
                }

                {//方式二
                    httpClient.PostAsync(url, new FormUrlEncodedContent(new Dictionary<string, string>() {
                        {"msg","123" },
                        {"msg2","456" }
                    })).ContinueWith(x =>
                    {
                        x.Result.Content.ReadAsStringAsync().ContinueWith(r =>
                        {
                            Console.WriteLine(r.Result);
                        });
                    }).GetAwaiter().GetResult();
                }
            }

            #endregion

            #region 带参数Post提交 body，服务端直接匹配model

            using (HttpClient httpClient = new HttpClient())
            {
                string url = "http://localhost:51582/TestController/PostResultWithModel";

                {//方式一 直接指定http头信息
                    StreamContent streamContent = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Id = 123, Name = "名字" }))));
                    streamContent.Headers.Add("Content-Type", " application/json");
                    httpClient.PostAsync(url, streamContent).ContinueWith(x =>
                    {
                        x.Result.Content.ReadAsStringAsync().ContinueWith(r =>
                        {
                            Console.WriteLine(r.Result);
                        });
                    }).GetAwaiter().GetResult();
                }

                {//方式二 需要先删除已有的默认http头信息
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(new { Id = 123, Name = "名字" }));
                    stringContent.Headers.Remove("Content-Type");
                    stringContent.Headers.Add("Content-Type", " application/json");
                    httpClient.PostAsync(url, stringContent).ContinueWith(x =>
                    {
                        x.Result.Content.ReadAsStringAsync().ContinueWith(r =>
                        {
                            Console.WriteLine(r.Result);
                        });
                    }).GetAwaiter().GetResult();
                }

            }

            #endregion

            #region Post提交模拟Http文件上传,文件和普通参数同时提交

            using (HttpClient httpClient = new HttpClient())
            {
                string url = "http://localhost:51582/TestController/UploadFile";

                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();

                var fileStream = new StreamContent(new MemoryStream(File.ReadAllBytes("a.jpg")));
                fileStream.Headers.Add("Content-Type", "image/jpeg");
                fileStream.Headers.Add("Content-Disposition", "form-data; name=\"file001\"; filename=\"a.jpg\"");
                multipartFormDataContent.Add(fileStream, "file001", "a.jpg");

                var fileStream2 = new StreamContent(new MemoryStream(File.ReadAllBytes("b.jpg")));
                fileStream2.Headers.Add("Content-Type", "image/jpeg");//header值中的双引号要转义，不要用单引号代替，否则上传失败
                fileStream2.Headers.Add("Content-Disposition", "form-data; name=\"file002\"; filename=\"b.jpg\"");
                multipartFormDataContent.Add(fileStream2);

                var stringContent = new StringContent("hello");
                multipartFormDataContent.Add(stringContent, "msg");

                httpClient.PostAsync(url, multipartFormDataContent).ContinueWith(x =>
                {
                    x.Result.Content.ReadAsStringAsync().ContinueWith(r =>
                    {
                        Console.WriteLine(r.Result);
                    });
                }).GetAwaiter().GetResult();
            }

            #endregion

            Console.ReadLine();
        }
    }
}
