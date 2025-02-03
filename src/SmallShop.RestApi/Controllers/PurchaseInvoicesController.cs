using Microsoft.AspNetCore.Mvc;
using SmallShop.Services.PurchaseInvoices.Contracts;

namespace SmallShop.RestApi.Controllers
{
    [Route("api/purchaseinvoices")]
    [ApiController]
    public class PurchaseInvoicesController : ControllerBase
    {
        private readonly PurchaseInvoiceService _service;

        public PurchaseInvoicesController(
            PurchaseInvoiceService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task Add(AddPurchaseInvoiceDto dto)
        {
           await _service.Add(dto);
        }

        [HttpGet]
        public async Task<List<GetAllPurchaseInvoicesDto>> GetAll()
        {
            return await _service.GetAll();
        }

        [HttpPut]
        public async Task Update(int invoiceNum,UpdatePurchaseInvoiceDto dto)
        {
            await _service.Update(invoiceNum, dto);
        }
    }
}
