参考Iam.Sample.EventBus项目
1.在项目中添加此包
2.在项目文件appsettings.json中添加配置，例如：
  "EventBus": {
    "RabbitMQ": {
      "HostName": "127.0.0.1",
      "Port": "5672",
      "UserName": "guest",
      "Password": "guest",
      "VirtualHost": "/",
      "ExchangeName": "default_exchange",
      "QueueName": "default_queue",
      "PrefetchCount": "1",
      "RetryCount": "3"
    }
  }
3.在startup.cs中添加服务AddEventBus()
  //添加消息总线
  services.AddEventBus(Configuration, builder => builder.UseRabbitMQ());
  //添加事件处理程序
  services.AddTransient<MessageReceivedIntegrationEventHandler>();
4.配置EventBus
  ConfigureEventBus(app);
  public void ConfigureEventBus(IApplicationBuilder app)
  {
      var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
      //event subscribe
      eventBus.Subscribe<MessageReceivedIntegrationEvent, MessageReceivedIntegrationEventHandler>();
      //start consume
      eventBus.StartSubscribe();
  }