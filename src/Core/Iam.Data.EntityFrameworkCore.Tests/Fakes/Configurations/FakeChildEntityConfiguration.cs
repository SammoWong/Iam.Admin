using Iam.Data.EntityFrameworkCore.Tests.Fakes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.EntityFrameworkCore.Tests.Fakes.Configurations
{
    public class FakeChildEntityConfiguration : IEntityTypeConfiguration<FakeChildEntity>
    {
        public void Configure(EntityTypeBuilder<FakeChildEntity> builder)
        {
            //设置表名
            builder.ToTable(nameof(FakeChildEntity));

            //设置主键
            builder.HasKey(e => e.Id);

            //设置属性
            builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

            //设置表之间关系
            builder.HasOne(e => e.FakeEntity).WithMany(e => e.FakeChildEntities).HasForeignKey(e => e.FakeEntityId);
        }
    }
}
