using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Services.Goodss.Contracts;
using SmallShop.Services.Goodss.Exceptions;
using SmallShop.Services.SaleInvoices.Contracts;
using SmallShop.Services.SaleInvoices.Exceptions;

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

        public async Task Add(AddSaleInvoiceDto dto)
        {
            var isExistInvoiceNum = await _repository.
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

            var goods = await _goodsRepository.GetById(dto.GoodsId);

            if (goods == null)
            {
                throw new GoodsDoesNotExistException();
            }

            goods.GoodsInventory -= saleInvoice.Count;
            goods.SellCount += saleInvoice.Count;

            _repository.Add(saleInvoice);
            await _unitOfWork.Commit();
        }

        public async Task<List<GetAllSaleInvoicesDto>> GetAll()
        {
            return await _repository.GetAll();    
        }

        public async Task Update(int invoiceNum, UpdateSaleInvoiceDto dto)
        {
            var saleInvoice = await _repository.GetById(invoiceNum);

            if (saleInvoice == null)
            {
                throw new SaleInvoiceDoesNotExistException();
            } 
            
            saleInvoice.Date = dto.Date;
            saleInvoice.BuyerName = dto.BuyerName;
            saleInvoice.Price = dto.Price;
            saleInvoice.GoodsId = dto.GoodsId;
            saleInvoice.Count = dto.Count;

            await _unitOfWork.Commit();
        }
    }
}
