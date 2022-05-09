using SmallShop.Entities;
using SmallShop.Services.PurchaseInvoices.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace SmallShop.Persistence.EF.PurchaseInvoices
{
    public class EFPurchaseInvoiceRepository : PurchaseInvoiceRepository
    {
        private readonly EFDataContext _dbContext;

        public EFPurchaseInvoiceRepository(EFDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(PurchaseInvoice purchaseInvoice)
        {
            _dbContext.PurchaseInvoices.Add(purchaseInvoice);
        }

        public List<GetAllPurchaseInvoicesDto> GetAll()
        {
            return _dbContext.PurchaseInvoices.Select(x =>
            new GetAllPurchaseInvoicesDto
            {
                InvoiceNum = x.InvoiceNum,
                Date = x.Date,
                SellerName = x.SellerName,
                GoodsId = x.GoodsId,
                Count = x.Count,
                Price = x.Price,
            }).ToList();
        }

        public PurchaseInvoice GetById(int InvoiceNum)
        {
            return _dbContext.PurchaseInvoices
                .FirstOrDefault(x => x.InvoiceNum == InvoiceNum);
        }

        public bool IsExistInvoiceNum(int invoiceNum)
        {
            return _dbContext.PurchaseInvoices.
                Any(x => x.InvoiceNum == invoiceNum);
        }
    }
}
