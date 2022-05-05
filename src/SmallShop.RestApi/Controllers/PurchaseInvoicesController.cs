using Microsoft.AspNetCore.Http;
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
        public void Add(AddPurchaseInvoiceDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public void GetAll()
        {
            _service.GetAll();
        }
    }
}
