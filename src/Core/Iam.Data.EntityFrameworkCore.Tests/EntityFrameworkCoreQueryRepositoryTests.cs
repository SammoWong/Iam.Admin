using Iam.Data.EntityFrameworkCore.Repositories;
using Iam.Data.EntityFrameworkCore.Tests.Fakes;
using Iam.Data.EntityFrameworkCore.Tests.Fakes.Entities;
using Iam.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Iam.Data.EntityFrameworkCore.Tests
{
    [TestClass]
    public class EntityFrameworkCoreQueryRepositoryTests
    {
        private readonly FakeDbContext _fakeDbContext;
        private readonly IRepository<FakeEntity> _fakeEntityRepository;

        public EntityFrameworkCoreQueryRepositoryTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<FakeDbContext>();
            _fakeDbContext = services.BuildServiceProvider().GetService<FakeDbContext>();
            _fakeDbContext.Database.EnsureDeleted();
            _fakeDbContext.Database.EnsureCreated();
            _fakeDbContext.Database.Migrate();
            _fakeEntityRepository = new EfCoreRepository<FakeDbContext, FakeEntity>(_fakeDbContext);
            SeedData();
        }

        [TestMethod]
        public void GetFakeEntities()
        {
            var result = _fakeEntityRepository.Get();
            Assert.IsNotNull(result);
        }


        private void SeedData()
        {
            if (!_fakeEntityRepository.Any())
            {
                for (int i = 0; i < 10; i++)
                {
                    _fakeEntityRepository.Insert(new FakeEntity { Name = Guid.NewGuid().ToString(), CreatedTime = DateTime.Now });
                }
            }
        }
    }
}
