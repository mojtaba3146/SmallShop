using SmallShop.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.SaleInvoices.Contracts
{
    public interface SaleInvoiceService : Service
    {
        void Add(AddSaleInvoiceDto dto);
        List<GetAllSaleInvoicesDto> GetAll();
        void Update(int invoiceNum, UpdateSaleInvoiceDto dto);
    }
}
