�ο�Iam.Sample.EventBus��Ŀ
1.����Ŀ����Ӵ˰�
2.����Ŀ�ļ�appsettings.json��������ã����磺
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
3.��startup.cs����ӷ���AddEventBus()
  //�����Ϣ����
  services.AddEventBus(Configuration, builder => builder.UseRabbitMQ());
  //����¼��������
  services.AddTransient<MessageReceivedIntegrationEventHandler>();
4.����EventBus
  ConfigureEventBus(app);
  public void ConfigureEventBus(IApplicationBuilder app)
  {
      var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
      //event subscribe
      eventBus.Subscribe<MessageReceivedIntegrationEvent, MessageReceivedIntegrationEventHandler>();
      //start consume
      eventBus.StartSubscribe();
  }