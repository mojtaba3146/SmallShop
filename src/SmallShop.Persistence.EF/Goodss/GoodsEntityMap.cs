using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmallShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Persistence.EF.Goodss
{
    public class GoodsEntityMap : IEntityTypeConfiguration<Goods>
    {
        public void Configure(EntityTypeBuilder<Goods> builder)
        {
            builder.ToTable("Goodss");

            builder.HasKey(_ => _.GoodsCode);

            builder.Property(_ => _.GoodsCode)
                .IsRequired();

            builder.Property(_ => _.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(_ => _.Price)
                .IsRequired();

            builder.Property(_ => _.CategoryId)
                .IsRequired();

            builder.Property(_ => _.MinInventory)
                .IsRequired();

            builder.Property(_ => _.MaxInventory)
                .IsRequired();


        }
    }
}
