﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.ServiceDiscovery.LoadBalancers
{
    public interface ILoadBalancer
    {
        ServiceInformation Load(IList<ServiceInformation> services);
    }
}
