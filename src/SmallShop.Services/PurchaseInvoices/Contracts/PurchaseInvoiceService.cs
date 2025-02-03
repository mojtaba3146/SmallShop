using SmallShop.Infrastructure.Application;

namespace SmallShop.Services.PurchaseInvoices.Contracts
{
    public interface PurchaseInvoiceService : Service
    {
        Task Add(AddPurchaseInvoiceDto dto);
        Task<List<GetAllPurchaseInvoicesDto>> GetAll();
        Task Update(int invoiceNum, UpdatePurchaseInvoiceDto dto);
    }
}
