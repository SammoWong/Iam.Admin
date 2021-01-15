using Iam.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.EntityFrameworkCore.Tests.Fakes.Entities
{
    public class FakeChildEntity : EntityBase<long>
    {
        public FakeEntity FakeEntity { get; set; }

        public long FakeEntityId { get; set; }
    }
}
