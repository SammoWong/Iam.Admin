using Iam.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.EntityFrameworkCore.Tests.Fakes.Entities
{
    public class FakeEntity : EntityBase<long>
    {
        public string Name { get; set; }

        public DateTime CreatedTime { get; set; }

        public ICollection<FakeChildEntity> FakeChildEntities { get; set; }
    }
}
