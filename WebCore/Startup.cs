using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using WebCore.Fileters;
using WebCore.Middleware;

namespace WebCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddOptions();
            services.AddDirectoryBrowser();

            var config = Configuration.GetSection("RootobjectSection").Get<Rootobject>();

            services.AddRouting();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc(options =>
            {
                var o = options;
                //options.Filters.Add(typeof(ActionFilterAttribute)); // 全局注册过滤器
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseRouter(routeBuilder =>
            {
                //routeBuilder.MapRoute(name: "default1", template: "test");
                //routeBuilder.MapGet("", context => Task.CompletedTask);//注意不要这样配置，会造成网站访问不符合预期，无法显示网页
                //MapGet MapPost MapPut MapDelete  和特性配置是一样的  可以在这里做路由定制化
            })

             .UseStatusCodePages()//配置状态码页面
             .UseHttpsRedirection()//http=>https 的重定向
             .UseDefaultFiles(new DefaultFilesOptions()
             {//UseDefaultFiles 必须在 UseStaticFiles 之前调用。UseDefaultFiles 只是重写了 URL，而不是真的提供了这样一个文件。你必须开启静态文件中间件（UseStaticFiles）来提供这个文件。
                 DefaultFileNames = new List<string>() { "test" }
             })
             .UseCookiePolicy()

             //*********************************************
             .UseStaticFiles()//默认允许访问wwwroot文件夹
             .UseStaticFiles(new StaticFileOptions()//
             {
                 ServeUnknownFileTypes = true,//有安全风险  默认关闭 false
                 FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticSource", "Images")), //指定静态文件的目录位置
                 RequestPath = new PathString("/StaticFiles"),//配置静态文件的访问路由 例：https://localhost:44320/staticfiles/a.jpg
                 ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>()
                    {
                        { ".xxx","application/xxx"}//配置扩展名映射
                    })
             })
             .UseStaticFiles(new StaticFileOptions()//
             {
                 ServeUnknownFileTypes = false,//有安全风险  默认关闭 false
                 FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticSource", "Css")), //指定静态文件的目录位置
                 RequestPath = new PathString("/Style"),//配置静态文件的访问路由 例：https://localhost:44320/Style/styleSheet.css
                 ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>()
                    {
                        { ".css","text/css"}//配置扩展名映射
                    })
             })
             .UseDirectoryBrowser(new DirectoryBrowserOptions()//只有目录访问功能，不能访问文件
             {//默认禁用，开启目录访问功能
                 FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory())),//指定访问目录
                 RequestPath = new PathString("/app")//指定访问路由 例：https://localhost:44320/app/
             })

             .UseDirectoryBrowser(new DirectoryBrowserOptions()//只有目录访问功能，不能访问文件
             {//默认禁用，开启目录访问功能
                 FileProvider = new PhysicalFileProvider(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)),//指定访问目录
                 RequestPath = new PathString("/Desktop")//指定访问路由 例：https://localhost:44320/Desktop/
             })

             .UseFileServer()//启用静态文件和默认文件，但不允许直接访问目录
             .UseFileServer(new FileServerOptions()//有目录访问功能，也能访问文件
             {
                 EnableDirectoryBrowsing = true,//启用静态文件、默认文件和目录浏览功能
                 FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticSource", "Images")),
                 RequestPath = new PathString("/downStaticFiles")//配置静态文件的访问路由 例：https://localhost:44320/staticfiles/a.jpg
             })

             //*********************************************

             .UseMiddleware<WebSocketMiddleware>()//使用 WebSocket 中间件
             .UseMiddleware<SampleMiddleware>()//使用自定义中间件，框架内部提供多个默认中间件，也是通过这种方式添加的，也可以通过定义IApplicationBuilder的扩展方法美化注册
             .UseMvc();

            app.MapWhen(httpcontext => httpcontext.Request.Path != "/ws", application =>
            {//只有程序启动时才会执行
                
            });



            app.Run(async context =>
            {
                if (context.Request.Query.ContainsKey("throw"))
                {
                    await context.Response.WriteAsync("are you ok?");
                }
            });
        }
    }

    public class Rootobject
    {
        public string Param1 { get; set; }
        public string Param2 { get; set; }
    }

}
