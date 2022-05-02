using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmallShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Persistence.EF.Categories
{
    public class CategoryEntityMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.Property(_ => _.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(_ => _.Goodss)
                .WithOne(_ => _.Category)
                .HasForeignKey(_ => _.CategoryId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
