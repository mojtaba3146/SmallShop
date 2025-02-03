using SmallShop.Entities;
using SmallShop.Infrastructure.Application;

namespace SmallShop.Services.PurchaseInvoices.Contracts
{
    public interface PurchaseInvoiceRepository : Repository
    {
        void Add(PurchaseInvoice purchaseInvoice);
        Task<List<GetAllPurchaseInvoicesDto>> GetAll();
        Task<PurchaseInvoice?> GetById(int InvoiceNum);
        Task<bool> IsExistInvoiceNum(int invoiceNum);
    }
}
