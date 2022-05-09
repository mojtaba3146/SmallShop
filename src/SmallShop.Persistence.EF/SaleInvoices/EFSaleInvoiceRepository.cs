using SmallShop.Entities;
using SmallShop.Services.SaleInvoices.Contracts;
using System.Collections.Generic;
using System.Linq;

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

        public List<GetAllSaleInvoicesDto> GetAll()
        {
            return _dbContext.SaleInvoices
                .Select(x => new GetAllSaleInvoicesDto
            {
                InvoiceNum = x.InvoiceNum,
                Date = x.Date,
                BuyerName = x.BuyerName,
                Count = x.Count,
                GoodsId = x.GoodsId,
                Price = x.Price,
            }).ToList();
        }

        public SaleInvoice GetById(int invoiceNum)
        {
            return _dbContext.SaleInvoices.
                FirstOrDefault(x => x.InvoiceNum == invoiceNum);
        }

        public bool IsExistInvoiceNum(int invoiceNum)
        {
            return _dbContext.SaleInvoices.
                Any(x => x.InvoiceNum == invoiceNum);
        }
    }
}
