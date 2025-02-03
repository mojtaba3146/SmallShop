using SmallShop.Infrastructure.Application;

namespace SmallShop.Services.SaleInvoices.Contracts
{
    public interface SaleInvoiceService : Service
    {
        Task Add(AddSaleInvoiceDto dto);
        Task<List<GetAllSaleInvoicesDto>> GetAll();
        Task Update(int invoiceNum, UpdateSaleInvoiceDto dto);
    }
}
