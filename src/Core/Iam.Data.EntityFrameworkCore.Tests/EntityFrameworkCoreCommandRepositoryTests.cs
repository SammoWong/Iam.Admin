using Iam.Data.EntityFrameworkCore.Repositories;
using Iam.Data.EntityFrameworkCore.Tests.Fakes;
using Iam.Data.EntityFrameworkCore.Tests.Fakes.Entities;
using Iam.Data.EntityFrameworkCore.UnitOfWork;
using Iam.Data.Repositories;
using Iam.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Iam.Data.EntityFrameworkCore.Tests
{
    [TestClass]
    public class EntityFrameworkCoreCommandRepositoryTests
    {
        private readonly FakeDbContext _fakeDbContext;
        private readonly IRepository<FakeEntity> _fakeEntityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EntityFrameworkCoreCommandRepositoryTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<FakeDbContext>();
            services.AddTransient<IUnitOfWork, EfCoreUnitOfWork<FakeDbContext>>();
            _fakeDbContext = services.BuildServiceProvider().GetService<FakeDbContext>();
            _fakeDbContext.Database.EnsureDeleted();
            _fakeDbContext.Database.EnsureCreated();
            _fakeDbContext.Database.Migrate();
            _fakeEntityRepository = new EfCoreRepository<FakeDbContext, FakeEntity>(_fakeDbContext);
            _unitOfWork = services.BuildServiceProvider().GetService<IUnitOfWork>();
        }

        [TestMethod]
        public void AddFakeEntity()
        {
            var fakeEntity = new FakeEntity
            {
                Name = "fake1",
                CreatedTime = DateTime.Now,
                FakeChildEntities = new List<FakeChildEntity> { new FakeChildEntity() }
            };
            _fakeEntityRepository.Insert(fakeEntity);
        }

        /// <summary>
        /// NOT WORK
        /// </summary>
        [TestMethod]
        public void AddFakeEntityWithUow()
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var entities = new List<FakeEntity>
                {
                    new FakeEntity{Name = Guid.NewGuid().ToString(), CreatedTime = DateTime.Now},
                    new FakeEntity{Name = Guid.NewGuid().ToString(), CreatedTime = DateTime.Now},
                };

                _fakeEntityRepository.InsertRange(entities);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
            finally
            {
                _unitOfWork.Dispose();
            }

        }

        [TestMethod]
        public void AddFakeEntityWithTransaction()
        {
            var entities = new List<FakeEntity>
            {
                new FakeEntity{Name = Guid.NewGuid().ToString(), CreatedTime = DateTime.Now},
                new FakeEntity{Name = Guid.NewGuid().ToString(), CreatedTime = DateTime.Now},
            };

            using (var transaction = _fakeDbContext.Database.BeginTransaction())
            {
                try
                {
                    _fakeDbContext.Set<FakeEntity>().AddRange(entities);
                    _fakeDbContext.SaveChanges();
                    //throw new Exception();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
