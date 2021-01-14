1.在项目中添加此包
2.在业务服务项目的startup.cs中添加服务：
  services.AddHealthCheck()/*.AddMySql("SqlConnection")*/;
3.业务服务中添加中间件：
  endpoints.MapHealthCheck();
  健康服务查看地址路由默认为：/health


1.在健康服务检查项目中添加此包
2.在startup.cs中添加服务：
  services.AddHealthCheck();
  services.AddHealthChecksUI(setup =>
  {
      //也可以在配置文件中配置endpoint
      setup.MaximumHistoryEntriesPerEndpoint(100);
      setup.AddHealthCheckEndpoint("HealthCheck Endpoint", "http://localhost:10002/health");
  })
  .AddInMemoryStorage();//内存存储
3.添加中间件：
  app.UseEndpoints(endpoints =>
  {
      endpoints.MapControllers();
      endpoints.MapHealthCheck();//健康检查
      endpoints.MapHealthChecksUI();//健康检查UI
  });
4.可以在ConfigureServices配置endpoint，也可以在配置文件appsettings.json中配置
  "HealthChecksUI": {
    "HealthChecks": [
      {
        "Name": "Identity Endpoint",
        "Uri": "http://localhost:10001/health"
      }
    ],
    "EvaluationTimeinSeconds": 10,
    "MinimumSecondsBetweenFailureNotifications": 60
  }