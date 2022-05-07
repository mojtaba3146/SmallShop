using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.SaleInvoices.Contracts
{
    public interface SaleInvoiceRepository : Repository
    {
        void Add(SaleInvoice saleInvoice);
        bool IsExistInvoiceNum(int invoiceNum);
        List<GetAllSaleInvoicesDto> GetAll();
        SaleInvoice GetById(int invoiceNum);
    }
}
