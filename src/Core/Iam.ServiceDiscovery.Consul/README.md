1.����Ŀ����Ӵ˰����ο�Identity����
2.����Ŀ�ļ�appsettings.json�����Jwt���ã����磺
  "Consul": {
    //Consul Client ��ַ
    "ConsulUrl": "http://127.0.0.1:8500",
    //Key·��
    "ConsulKeyPath": "",
    //��ǰ�������ƣ����Զ��ʵ������
    "ServiceName": "Iam.Identity",
    //��ǰ�����ַ
    "ServiceUrl": "http://localhost:9100",
    //����tag
    "ServerTags": [ "urlprefix-/identity" ],
    //�������ĵ�ַ����ǰ���񹫲�������һ��api�ӿ�
    "HealthCheckUrl": "/health",
    //�������
    "HealthCheckIntervalInSecond": 20
  }
3.��startup.cs����ӷ���AddConsul()
  var consulConfig = Configuration.GetSection("Consul").Get<ConsulConfig>();
  services.AddConsul(consulConfig.ConsulUrl, loadBalancer: TypeLoadBalancer.RoundRobin);
4.��Configure�������ע�ᵽConsul
  var consulConfig = Configuration.GetSection("Consul").Get<ConsulConfig>();
  app.RegisterToConsul(consulConfig);