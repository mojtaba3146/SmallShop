using FluentMigrator;

namespace SmallShop.Migrations
{
    [Migration(202409161243)]
    public class _202409161243_InitialRefreshToken : Migration
    {

        public override void Up()
        {
            Create.Table("RefreshTokens")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Token").AsString(256).NotNullable()
            .WithColumn("PRefreshToken").AsString(256).NotNullable()
            .WithColumn("CreatedDate").AsDateTime().NotNullable()
            .WithColumn("ExpiryDate").AsDateTime().NotNullable()
            .WithColumn("IpAddress").AsString(45).Nullable()
            .WithColumn("UserId").AsInt32().NotNullable()
            .ForeignKey("FK_RefreshTokens_Users", "Users", "Id")
            .OnDelete(System.Data.Rule.Cascade);
        }
        public override void Down()
        {
            Delete.Table("RefreshTokens");
        }
    }
}
