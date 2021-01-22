1.在项目中添加此包（参考Identity服务）
2.在项目文件appsettings.json中添加Jwt配置，例如：
  "Consul": {
    //Consul Client 地址
    "ConsulUrl": "http://127.0.0.1:8500",
    //Key路径
    "ConsulKeyPath": "",
    //当前服务名称，可以多个实例共享
    "ServiceName": "Iam.Identity",
    //当前服务地址
    "ServiceUrl": "http://localhost:9100",
    //服务tag
    "ServerTags": [ "urlprefix-/identity" ],
    //健康检查的地址，当前服务公布出来的一个api接口
    "HealthCheckUrl": "/health",
    //心跳间隔
    "HealthCheckIntervalInSecond": 20
  }
3.在startup.cs中添加服务AddConsul()
  var consulConfig = Configuration.GetSection("Consul").Get<ConsulConfig>();
  services.AddConsul(consulConfig.ConsulUrl, loadBalancer: TypeLoadBalancer.RoundRobin);
4.在Configure方法添加注册到Consul
  var consulConfig = Configuration.GetSection("Consul").Get<ConsulConfig>();
  app.RegisterToConsul(consulConfig);