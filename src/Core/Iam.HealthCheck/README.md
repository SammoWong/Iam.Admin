1.����Ŀ����Ӵ˰�
2.��ҵ�������Ŀ��startup.cs����ӷ���
  services.AddHealthCheck()/*.AddMySql("SqlConnection")*/;
3.ҵ�����������м����
  endpoints.MapHealthCheck();
  ��������鿴��ַ·��Ĭ��Ϊ��/health


1.�ڽ�����������Ŀ����Ӵ˰�
2.��startup.cs����ӷ���
  services.AddHealthCheck();
  services.AddHealthChecksUI(setup =>
  {
      //Ҳ�����������ļ�������endpoint
      setup.MaximumHistoryEntriesPerEndpoint(100);
      setup.AddHealthCheckEndpoint("HealthCheck Endpoint", "http://localhost:10002/health");
  })
  .AddInMemoryStorage();//�ڴ�洢
3.����м����
  app.UseEndpoints(endpoints =>
  {
      endpoints.MapControllers();
      endpoints.MapHealthCheck();//�������
      endpoints.MapHealthChecksUI();//�������UI
  });
4.������ConfigureServices����endpoint��Ҳ�����������ļ�appsettings.json������
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