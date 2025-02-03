using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Services.Goodss.Contracts;
using SmallShop.Services.Goodss.Exceptions;
using SmallShop.Services.PurchaseInvoices.Contracts;
using SmallShop.Services.PurchaseInvoices.Exceptions;

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

        public async Task Add(AddPurchaseInvoiceDto dto)
        {
            var isExistInvoiceNum = await _repository.
                IsExistInvoiceNum(dto.InvoiceNum);

            if (isExistInvoiceNum)
            {
                throw new InvoiceNumAlreadyExistException();
            }
            var purchaseInvoice = new PurchaseInvoice
            {
                Date = dto.Date,
                GoodsId = dto.GoodsId,
                Price = dto.Price,
                SellerName = dto.SellerName,
                Count = dto.Count,
                InvoiceNum = dto.InvoiceNum,
            };

            var goods= await _goodsRepository.GetById(dto.GoodsId);

            if (goods==null)
            {
                throw new GoodsDoesNotExistException();
            }

            goods.GoodsInventory += purchaseInvoice.Count;

            _repository.Add(purchaseInvoice);
            await _unitOfWork.Commit();
        }

        public async Task<List<GetAllPurchaseInvoicesDto>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task Update(int invoiceNum, UpdatePurchaseInvoiceDto dto)
        {
            var purchaseInvoice = await _repository.GetById(invoiceNum);

            if (purchaseInvoice == null)
            {
                throw new PurchaseInvoiceDoesNotExistException();
            }

            purchaseInvoice.Price = dto.Price;
            purchaseInvoice.SellerName = dto.SellerName;
            purchaseInvoice.Count = dto.Count;
            purchaseInvoice.Date = dto.Date;
            purchaseInvoice.GoodsId = dto.GoodsId;

            await _unitOfWork.Commit();
        }
    }
}
