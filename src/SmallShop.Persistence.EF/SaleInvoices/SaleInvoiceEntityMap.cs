using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmallShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Persistence.EF.SaleInvoices
{
    public class SaleInvoiceEntityMap : IEntityTypeConfiguration<SaleInvoice>
    {
        public void Configure(EntityTypeBuilder<SaleInvoice> builder)
        {
            builder.ToTable("SaleInvoices");

            builder.HasKey(_ => _.InvoiceNum);

            builder.Property(_ => _.InvoiceNum)
                .IsRequired();

            builder.Property(_ => _.Date)
                .IsRequired();

            builder.Property(_ => _.Price)
                .IsRequired();

            builder.Property(_ => _.Count)
                .IsRequired();

            builder.Property(_ => _.BuyerName)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(_ => _.Goods)
                .WithMany(_ => _.SaleInvoices)
                .HasForeignKey(_ => _.GoodsId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
