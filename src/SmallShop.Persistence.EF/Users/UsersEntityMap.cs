using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmallShop.Entities;

namespace SmallShop.Persistence.EF.Users
{
    internal class UsersEntityMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.FirstName)
                  .IsRequired()
                  .HasMaxLength(50);

            builder.Property(u => u.LastName)
                  .IsRequired()
                  .HasMaxLength(50);

            builder.Property(u => u.UserName)
                  .IsRequired()
                  .HasMaxLength(50);

            builder.Property(u => u.Password)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(u => u.Email)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.HasOne(u => u.Role)
                 .WithMany(r => r.Users)
                 .HasForeignKey(u => u.RoleId);
        }
    }
}
