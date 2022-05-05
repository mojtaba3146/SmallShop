using SmallShop.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.PurchaseInvoices.Contracts
{
    public interface PurchaseInvoiceService : Service
    {
        void Add(AddPurchaseInvoiceDto dto);
        List<GetAllPurchaseInvoicesDto> GetAll();
    }
}
