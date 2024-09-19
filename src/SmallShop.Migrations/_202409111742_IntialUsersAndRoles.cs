using FluentMigrator;

namespace SmallShop.Migrations
{
    [Migration(202409111742)]
    public class _202409111742_IntialUsersAndRoles : Migration
    {

        public override void Up()
        {
            Create.Table("Roles")
            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
            .WithColumn("Name").AsString(50).NotNullable();

            Create.Table("Users")
             .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
             .WithColumn("FirstName").AsString(50).NotNullable()
             .WithColumn("LastName").AsString(50).NotNullable()
             .WithColumn("UserName").AsString(50).NotNullable()
             .WithColumn("Password").AsString(100).NotNullable()
             .WithColumn("Email").AsString(100).NotNullable()
             .WithColumn("RoleId").AsInt32().NotNullable()
             .ForeignKey("FK_Users_Roles", "Roles", "Id")
             .OnDelete(System.Data.Rule.None);
        }
        public override void Down()
        {
            Delete.Table("Users");
            Delete.Table("Roles");
        }
    }
}
