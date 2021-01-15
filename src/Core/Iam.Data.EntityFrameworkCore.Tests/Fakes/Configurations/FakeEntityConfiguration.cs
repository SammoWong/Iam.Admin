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
    public class FakeEntityConfiguration : IEntityTypeConfiguration<FakeEntity>
    {
        public void Configure(EntityTypeBuilder<FakeEntity> builder)
        {
            //设置表名
            builder.ToTable(nameof(FakeEntity));
            
            //设置主键
            builder.HasKey(e => e.Id);

            //设置属性
            builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.CreatedTime).HasColumnName(nameof(FakeEntity.CreatedTime));

            //设置表之间关系
            builder.HasMany(e => e.FakeChildEntities).WithOne(e => e.FakeEntity).HasForeignKey(e => e.FakeEntityId);
        }
    }
}
