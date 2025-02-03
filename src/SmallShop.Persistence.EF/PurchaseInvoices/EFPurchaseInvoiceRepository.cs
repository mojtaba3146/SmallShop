using Microsoft.EntityFrameworkCore;
using SmallShop.Entities;
using SmallShop.Services.PurchaseInvoices.Contracts;

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

        public async Task<List<GetAllPurchaseInvoicesDto>> GetAll()
        {
            return await _dbContext.PurchaseInvoices.Select(x =>
            new GetAllPurchaseInvoicesDto
            {
                InvoiceNum = x.InvoiceNum,
                Date = x.Date,
                SellerName = x.SellerName,
                GoodsId = x.GoodsId,
                Count = x.Count,
                Price = x.Price,
            }).ToListAsync();
        }

        public async Task<PurchaseInvoice?> GetById(int InvoiceNum)
        {
            return await _dbContext.PurchaseInvoices
                .FirstOrDefaultAsync(x => x.InvoiceNum == InvoiceNum);
        }

        public async Task<bool> IsExistInvoiceNum(int invoiceNum)
        {
            return await _dbContext.PurchaseInvoices.
                AnyAsync(x => x.InvoiceNum == invoiceNum);
        }
    }
}
