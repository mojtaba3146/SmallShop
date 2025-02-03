using Microsoft.EntityFrameworkCore;
using SmallShop.Entities;
using SmallShop.Services.SaleInvoices.Contracts;

namespace SmallShop.Persistence.EF.SaleInvoices
{
    public class EFSaleInvoiceRepository : SaleInvoiceRepository
    {
        private readonly EFDataContext _dbContext;

        public EFSaleInvoiceRepository(EFDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(SaleInvoice saleInvoice)
        {
            _dbContext.SaleInvoices.Add(saleInvoice);
        }

        public async Task<List<GetAllSaleInvoicesDto>> GetAll()
        {
            return await _dbContext.SaleInvoices
                .Select(x => new GetAllSaleInvoicesDto
            {
                InvoiceNum = x.InvoiceNum,
                Date = x.Date,
                BuyerName = x.BuyerName,
                Count = x.Count,
                GoodsId = x.GoodsId,
                Price = x.Price,
            }).ToListAsync();
        }

        public async Task<SaleInvoice?> GetById(int invoiceNum)
        {
            return await _dbContext.SaleInvoices.
                FirstOrDefaultAsync(x => x.InvoiceNum == invoiceNum);
        }

        public async Task<bool> IsExistInvoiceNum(int invoiceNum)
        {
            return await _dbContext.SaleInvoices.
                AnyAsync(x => x.InvoiceNum == invoiceNum);
        }
    }
}
