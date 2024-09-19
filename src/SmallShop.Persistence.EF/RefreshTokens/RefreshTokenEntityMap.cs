using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmallShop.Entities;

namespace SmallShop.Persistence.EF.RefreshTokens
{
    public class RefreshTokenEntityMap : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.Id);

            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(256); 

            builder.Property(rt => rt.PRefreshToken)
                .IsRequired()
                .HasMaxLength(256); 

            builder.Property(rt => rt.CreatedDate)
                .IsRequired();

            builder.Property(rt => rt.ExpiryDate)
                .IsRequired();

            builder.Property(rt => rt.IpAddress)
                .HasMaxLength(45); 

            builder.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)  
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
