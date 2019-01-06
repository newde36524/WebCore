   
   
   记得在Startup中注册类型
   services.TryAddSingleton<IActionResultExecutor<MyContentResult>, MyJsonResultExecutor>();