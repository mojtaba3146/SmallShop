using SmallShop.Entities;
using SmallShop.Infrastructure.Application;

namespace SmallShop.Services.SaleInvoices.Contracts
{
    public interface SaleInvoiceRepository : Repository
    {
        void Add(SaleInvoice saleInvoice);
        Task<bool> IsExistInvoiceNum(int invoiceNum);
        Task<List<GetAllSaleInvoicesDto>> GetAll();
        Task<SaleInvoice?> GetById(int invoiceNum);
    }
}
