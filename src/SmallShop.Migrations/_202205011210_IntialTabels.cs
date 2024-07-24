using FluentMigrator;

namespace SmallShop.Migrations
{
    [Migration(202205011210)]
    public class _202205011210_IntialTabels : Migration
    {
        public override void Up()
        {
            CreateCategory();
            CreateGoods();
            CreatePurshaseInvoice();
            CreateSaleInvoice();
        }

        public override void Down()
        {
            Delete.Table("Categories");
            Delete.Table("Goodss");
            Delete.Table("PurchaseInvoices");
            Delete.Table("SaleInvoices");
        }

        private void CreateSaleInvoice()
        {
            Create.Table("SaleInvoices")
                            .WithColumn("InvoiceNum").AsInt32().PrimaryKey().NotNullable()
                            .WithColumn("Date").AsDateTime().NotNullable()
                            .WithColumn("Count").AsInt32().NotNullable()
                            .WithColumn("Price").AsInt32().NotNullable()
                            .WithColumn("BuyerName").AsString(50).NotNullable()
                            .WithColumn("GoodsId").AsInt32().NotNullable()
                            .ForeignKey("FK_SaleInvoices_Goods", "Goodss", "GoodsCode")
                            .OnDelete(System.Data.Rule.None);
        }

        private void CreatePurshaseInvoice()
        {
            Create.Table("PurchaseInvoices")
                            .WithColumn("InvoiceNum").AsInt32().PrimaryKey().NotNullable()
                            .WithColumn("Date").AsDateTime().NotNullable()
                            .WithColumn("Count").AsInt32().NotNullable()
                            .WithColumn("Price").AsInt32().NotNullable()
                            .WithColumn("SellerName").AsString(50).NotNullable()
                            .WithColumn("GoodsId").AsInt32().NotNullable()
                            .ForeignKey("FK_PurchaseInvoices_Goods", "Goodss", "GoodsCode")
                            .OnDelete(System.Data.Rule.None);
        }

        private void CreateGoods()
        {
            Create.Table("Goodss")
                            .WithColumn("GoodsCode").AsInt32().PrimaryKey().NotNullable()
                            .WithColumn("Name").AsString(50).NotNullable()
                            .WithColumn("Price").AsInt32().NotNullable()
                            .WithColumn("GoodsInventory").AsInt32().Nullable()
                            .WithColumn("MinInventory").AsInt32().NotNullable()
                            .WithColumn("MaxInventory").AsInt32().NotNullable()
                            .WithColumn("SellCount").AsInt32().Nullable()
                            .WithColumn("CategoryId").AsInt32().NotNullable()
                            .ForeignKey("FK_Goodss_Categories", "Categories", "Id")
                            .OnDelete(System.Data.Rule.None);
        }

        private void CreateCategory()
        {
            Create.Table("Categories")
                              .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
                              .WithColumn("Title").AsString(50).NotNullable();
        }

    }
}
