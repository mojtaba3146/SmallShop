using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Services.Goodss.Contracts;
using SmallShop.Services.Goodss.Exceptions;
using SmallShop.Services.PurchaseInvoices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.PurchaseInvoices
{
    public class PurchaseInvoiceAppService : PurchaseInvoiceService
    {
        private readonly PurchaseInvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsRepository _goodsRepository;

        public PurchaseInvoiceAppService(PurchaseInvoiceRepository repository
            ,UnitOfWork unitOfWork,
            GoodsRepository goodsRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _goodsRepository = goodsRepository;
        }

        public void Add(AddPurchaseInvoiceDto dto)
        {
            var purchaseInvoice = new PurchaseInvoice
            {
                Date = dto.Date,
                GoodsId = dto.GoodsId,
                Price = dto.Price,
                SellerName = dto.SellerName,
                Count = dto.Count,
                InvoiceNum = dto.InvoiceNum,
            };

            var goods= _goodsRepository.GetById(dto.GoodsId);

            if (goods==null)
            {
                throw new GoodsDoesNotExistException();
            }

            goods.GoodsInventory += purchaseInvoice.Count;

            _repository.Add(purchaseInvoice);
            _unitOfWork.Commit();
        }

        public List<GetAllPurchaseInvoicesDto> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
