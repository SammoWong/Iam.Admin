using Iam.Data.EntityFrameworkCore.Tests.Fakes.Configurations;
using Iam.Data.EntityFrameworkCore.Tests.Fakes.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeDbContext : DbContext
    {
        public DbSet<FakeEntity> FakeEntities { get; set; }

        public DbSet<FakeChildEntity> FakeChildEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FakeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FakeChildEntityConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //配置连接
            //services.AddDbContextPool<FakeDbContext>(options => options.UseSqlServer(connectionString));
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Fakes;Integrated Security=true;Connection Timeout=5;");
        }
    }
}
