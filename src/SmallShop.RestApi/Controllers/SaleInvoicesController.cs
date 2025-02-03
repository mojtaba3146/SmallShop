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
        public async Task Add(AddSaleInvoiceDto dto)
        {
            await _service.Add(dto);
        }

        [HttpGet]
        public async Task<List<GetAllSaleInvoicesDto>> GetAll()
        {
            return await _service.GetAll();
        }

        [HttpPut]
        public async Task Update(int invoiceNum,UpdateSaleInvoiceDto dto)
        {
            await _service.Update(invoiceNum, dto);
        }
    }
}
