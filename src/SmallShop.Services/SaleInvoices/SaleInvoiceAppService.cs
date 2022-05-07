using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Services.Goodss.Contracts;
using SmallShop.Services.Goodss.Exceptions;
using SmallShop.Services.SaleInvoices.Contracts;
using SmallShop.Services.SaleInvoices.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.SaleInvoices
{
    public class SaleInvoiceAppService : SaleInvoiceService
    {
        private readonly SaleInvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsRepository _goodsRepository;

        public SaleInvoiceAppService(SaleInvoiceRepository repository
            , UnitOfWork unitOfWork,
            GoodsRepository goodsRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _goodsRepository = goodsRepository;
        }

        public void Add(AddSaleInvoiceDto dto)
        {
            var isExistInvoiceNum = _repository.
                IsExistInvoiceNum(dto.InvoiceNum);

            if (isExistInvoiceNum)
            {
                throw new InvoiceNumAlreadyExistException();
            }

                var saleInvoice = new SaleInvoice
            {
                InvoiceNum = dto.InvoiceNum,
                Date = dto.Date,
                BuyerName = dto.BuyerName,
                Count = dto.Count,
                Price = dto.Price,
                GoodsId = dto.GoodsId,
            };

            var goods = _goodsRepository.GetById(dto.GoodsId);

            if (goods == null)
            {
                throw new GoodsDoesNotExistException();
            }

            goods.GoodsInventory -= saleInvoice.Count;
            goods.SellCount += saleInvoice.Count;

            _repository.Add(saleInvoice);
            _unitOfWork.Commit();
        }

        public List<GetAllSaleInvoicesDto> GetAll()
        {
            return _repository.GetAll();    
        }

        public void Update(int invoiceNum, UpdateSaleInvoiceDto dto)
        {
            var saleInvoice = _repository.GetById(invoiceNum);

            if (saleInvoice == null)
            {
                throw new SaleInvoiceDoesNotExistException();
            } 
            
            saleInvoice.Date = dto.Date;
            saleInvoice.BuyerName = dto.BuyerName;
            saleInvoice.Price = dto.Price;
            saleInvoice.GoodsId = dto.GoodsId;
            saleInvoice.Count = dto.Count;

            _unitOfWork.Commit();
        }
    }
}
