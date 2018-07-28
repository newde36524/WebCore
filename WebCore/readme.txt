


********* 过滤器只在控制器或者Action中起作用，可通过 services.AddMvc 中全局设置，也可以当成特性在具体的控制器
********* 或Action上指定，执行顺序由具体的Filter决定为执行重载顺序为 
********* OnAuthorization =>
********* ActionFilterExcuting =>
********* Action =>//具体的控制器方法
********* ActionExcuted =>  如果在Action中抛异常 则 OnException=>
********* OnResultExcuting => 
********* OnResultExcuted =>//执行到这里说明请求过程已完成，抛出异常不影响这次请求
		  
********* 所有过滤器中 FilterContext参数 的Result属性一旦设置，就会被提前返回，不进入之后的执行管道 *********
********* 异常过滤器的 Exception 属性设置为 null 或者 ExceptionHandled 属性设置为 true 可继续执行后续的管道否则直接返回500








