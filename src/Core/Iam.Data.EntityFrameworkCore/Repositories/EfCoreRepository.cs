using Iam.Data.Entities;
using Iam.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iam.Data.EntityFrameworkCore.Repositories
{
    public class EfCoreRepository<TDbContext, TEntity> : Repository<TEntity> where TEntity : class, IEntity where TDbContext : DbContext
    {
        public EfCoreRepository(TDbContext context) :
            base(new EfCoreCommandRepository<TDbContext, TEntity>(context), new EfCoreQueryRepository<TDbContext, TEntity>(context))
        {
        }
    }
}
