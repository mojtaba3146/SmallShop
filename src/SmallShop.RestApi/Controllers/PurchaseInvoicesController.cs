using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmallShop.Services.PurchaseInvoices.Contracts;
using System.Collections.Generic;

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
        public List<GetAllPurchaseInvoicesDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPut]
        public void Update(int invoiceNum,UpdatePurchaseInvoiceDto dto)
        {
            _service.Update(invoiceNum, dto);
        }
    }
}
