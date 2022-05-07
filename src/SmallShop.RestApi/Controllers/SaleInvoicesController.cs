using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmallShop.Services.SaleInvoices.Contracts;

namespace SmallShop.RestApi.Controllers
{
    [Route("api/saleinvoices")]
    [ApiController]
    public class SaleInvoicesController : ControllerBase
    {
        private readonly SaleInvoiceService _service;

        public SaleInvoicesController(SaleInvoiceService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddSaleInvoiceDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public void GetAll()
        {
            _service.GetAll();
        }

        [HttpPut]
        public void Update(int invoiceNum,UpdateSaleInvoiceDto dto)
        {
            _service.Update(invoiceNum, dto);
        }
    }
}
