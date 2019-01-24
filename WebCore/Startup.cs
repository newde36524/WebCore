using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
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
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using WebCore.CustomerActionResult;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Text;
using WebCore.Hosting;
using WebCore.Extension.Options;
using WebCore.SignalRHub;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddOptions();
            services.AddDirectoryBrowser();
            services.AddSession();//开启session
            var config = Configuration.GetSection("RootobjectSection").Get<Rootobject>();

            services.AddRouting();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc(options =>
            {
                var o = options;
                //options.Filters.Add(typeof(ActionFilterAttribute)); // 全局注册过滤器
                //options.Filters.Add(typeof(AjaxRequestFilterAttribute));
                //options.Filters.Add(typeof(MyResultFilterAttribute));
            });

            #region 开启Authorization认证

            #endregion

            #region 开启SignalR

            services.AddSignalR();

            #endregion

            #region 配置自定义RabbitMqService和注册发布客户端

            services.AddHostedService<MyRabbitMqService>();
            services.AddSingleton<IQueuePublish, MyRabbitMqDeclareClient>();
            services.AddSingleton<IModel>(service =>
            {
                var option = service.GetRequiredService<IOptions<RabbitMqOption>>();
                var factory = new ConnectionFactory()
                {
                    HostName = option.Value.RabbitHost,
                    UserName = option.Value.RabbitUserName,
                    Password = option.Value.RabbitPassword,
                    Port = option.Value.RabbitPort
                };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                return channel;
            });

            #endregion

            #region 配置自定义Option

            services.Configure<RabbitMqOption>(Configuration.GetSection("rabbitmq"));

            #endregion

            #region 设置自定义ActionResult

            services.TryAddSingleton<IActionResultExecutor<MyContentResult>, MyContentResultExecutor>();
            services.TryAddSingleton<IActionResultExecutor<MyJsonResult>, MyJsonResultExecutor>();

            #endregion

            #region 设置 Swagger API文档

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Web Api 接口文档",
                    Description = "Swagger WebApi调试应用",
                    TermsOfService = "None"
                });

                //Set the comments path for the swagger json and ui.
                //var basePath = System.AppContext.BaseDirectory;
                //var xmlPath = Path.Combine(basePath, "DapperSwaggerAutofac.xml");
                //c.IncludeXmlComments(xmlPath);

                //c.OperationFilter<HttpHeaderOperation>(); // 添加httpHeader参数
            });

            #endregion

            #region 默认容器替换Autofac

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            return new AutofacServiceProvider(containerBuilder.Build());

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
        {
            #region 开发环境错误页设置

            if (env.IsDevelopment())//判断是否是开发环境
            {
                //app.UseBrowserLink();
                app.UseStatusCodePages(async context =>
                {
                    context.HttpContext.Response.ContentType = "text/plain";
                    await context.HttpContext.Response.WriteAsync(
                        "Status code page, status code: " +
                        context.HttpContext.Response.StatusCode);
                });
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");//如果不是就跳转到错误页
                app.UseHsts();
            }
            app.UseSession();//开启Session
            #endregion

            #region MVC 路由 https重定向 状态页码配置相关

            app.UseRouter(routeBuilder =>
            {
                //routeBuilder.MapRoute(name: "default1", template: "test");
                //routeBuilder.MapGet("", context => Task.CompletedTask);//注意不要这样配置，会造成网站访问不符合预期，无法显示网页
                //MapGet MapPost MapPut MapDelete  和特性配置是一样的  可以在这里做路由定制化
            })
            .UseStatusCodePages()//配置状态码页面
            .UseHttpsRedirection()//http=>https 的重定向
            .UseCookiePolicy()
            .UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            })
            .UseMvc()
            .UseMvc(routes =>
            {//配置默认路由
                routes.MapRoute(
                   name: "default",
                   template: "{controller=home}/{action=index}/{id?}");
                routes.MapRoute(
                   name: "test",
                   template: "test");
            });

            #endregion

            #region 启用文件系统文件夹视图

            app.UseStaticFiles()//默认允许访问wwwroot文件夹
            .UseDefaultFiles(new DefaultFilesOptions()
            {//UseDefaultFiles 必须在 UseStaticFiles 之前调用。UseDefaultFiles 只是重写了 URL，而不是真的提供了这样一个文件。你必须开启静态文件中间件（UseStaticFiles）来提供这个文件。
                DefaultFileNames = new List<string>() { "test" }
            })
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
            });

            var option = new FileServerOptions()//有目录访问功能，也能访问文件
            {
                EnableDirectoryBrowsing = true,//启用静态文件、默认文件和目录浏览功能
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticSource", "Images")),
                RequestPath = new PathString("/downStaticFiles")//配置静态文件的访问路由 例：https://localhost:44320/staticfiles/a.jpg
            };

            app.UseFileServer()//启用静态文件和默认文件，但不允许直接访问目录
             .UseFileServer(option);//配置静态文件的访问方式，静态文件的访问不走中间件过滤器，并且不走路由，因为他只识别路径，并不是路由到具体的Action

            #endregion

            #region 启用Swagger中间件

            app
            .UseSwagger()//注意 不管有没有配置SwaggerOptions参数，这一步都是必须的
            //.UseSwagger(options =>
            //{
            //    options.RouteTemplate = "/swagger/v1/swagger.json";
            //    options.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Host = httpReq.Host.Value);
            //})
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger 接口 V1.0");
            });

            #endregion

            #region 设置自定义中间件

            app.UseWebSockets()
            .UseMiddleware<AddEndpointMiddleware>("")
            .UseMiddleware<WebSocketMiddleware>()//使用 WebSocket 中间件
            .UseMiddleware<SampleMiddleware>();//使用自定义中间件，框架内部提供多个默认中间件，也是通过这种方式添加的，也可以通过定义IApplicationBuilder的扩展方法美化注册

            #endregion

            #region Map Use Run

            app.MapWhen(httpcontext => httpcontext.Request.Path != "/ws", application =>
            {//只有程序启动时才会执行

            });
            app.Map("/path", _app =>
            {//通过指定路径分发管道
                _app.Run(async context =>
                {
                    await context.Response.WriteAsync("当前路由不存在");
                });
            });

            app.Use(async (context, next) =>
            {
                //通过Use  定义中间件
                //下一个管道执行前
                await next();//表示执行下一个管道
                //下一个管道执行后
            });


            app.Run(async context =>
            {//定义抛异常中间件，并不执行之后的管道
                if (context.Request.Query.ContainsKey("throw"))
                {
                    await context.Response.WriteAsync("are you ok?");
                }
            });

            #endregion

            #region 设置Application生命周期钩子

            var logger = loggerFactory.CreateLogger<Startup>();
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                Console.WriteLine("网站启动");
                logger.LogInformation("网站启动");
            });
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                Console.WriteLine("网站正在停止");
                logger.LogInformation("网站正在停止");
            });
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                Console.WriteLine("网站已停止");
                logger.LogInformation("网站已停止");
            });

            #endregion

            #region 设置Cors

            app.UseCors(options =>
            {
                options.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials()
                .WithOrigins("https://localhost:5000");
            });

            #endregion

            #region 设置SignalR

            app.UseSignalR(routes =>
            {
                routes.MapHub<MyHub>("/hubs");
            });

            #endregion
        }
    }

    public class Rootobject
    {
        public string Param1 { get; set; }
        public string Param2 { get; set; }
    }

}
