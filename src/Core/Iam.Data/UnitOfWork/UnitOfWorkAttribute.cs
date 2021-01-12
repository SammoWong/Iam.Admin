using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.UnitOfWork
{
    public class UnitOfWorkAttribute : Attribute
    {
        public UnitOfWorkAttribute(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted)
        {
            IsolationLevel = isolationLevel;
        }

        public IsolationLevel IsolationLevel { get; set; }
    }
}
