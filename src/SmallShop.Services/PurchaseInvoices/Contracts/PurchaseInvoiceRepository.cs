using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.PurchaseInvoices.Contracts
{
    public interface PurchaseInvoiceRepository : Repository
    {
        void Add(PurchaseInvoice purchaseInvoice);
        List<GetAllPurchaseInvoicesDto> GetAll();
        PurchaseInvoice GetById(int InvoiceNum);
        bool IsExistInvoiceNum(int invoiceNum);
    }
}
