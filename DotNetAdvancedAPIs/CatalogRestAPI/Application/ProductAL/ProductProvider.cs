using ProductDomain.BusinessLogic;
using ProductDomain.Entities;
using ProductDomain.ExternalServiceInterfaces;
using ProductDomain.RepositoryInterfaces;
using HelperUtils;

namespace Application.ProductAL
{
    public class ProductProvider : IProductProvider
    {
        private readonly IMapper _mapper;
        private IProductRepository _productRepository;
        private readonly IProductMessageBroker _messageBroker;

        public ProductProvider(IMapper mapper, IRepositoryManager repositoryManager, IProductMessageBroker messageBroker)
        {
            _mapper = mapper;
            _productRepository = repositoryManager.ProductRepository;
            _messageBroker = messageBroker;
        }

        public async Task<DBOperationStatus> AddAsync(ProductDTO productDTO)
        {
            productDTO.ID = 0;
            var product = _mapper.Map<ProductDTO, Product>(productDTO);
            DBOperationStatus status = await _productRepository.AddAsync(product);
            productDTO.ID = product.ID;
            return status;
        }

        public Task<DBOperationStatus> DeleteAsync(int id)
        {
            return _productRepository.DeleteAsync(id);
        }

        public async Task<PagedList<ProductShortInfoDTO>> GetAllAsync(ProductsFilter filter)
        {
            var pagedList = await _productRepository.GetAllAsync(filter);
            var dtoList = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductShortInfoDTO>>(pagedList.List);
            return new PagedList<ProductShortInfoDTO>(dtoList, pagedList.TotalPages, pagedList.PageNumber, pagedList.PageSize);
        }

        public async Task<(DBOperationStatus, ProductDTO)> GetById(int id)
        {
            var product = await _productRepository.GetByID(id);
            if (product != null)
            {
                var dto = _mapper.Map<Product, ProductDTO>(product);
                return (DBOperationStatus.Success, dto);
            }
            return (DBOperationStatus.NotFound, null);
        }

        public async Task<DBOperationStatus> UpdateAsync(ProductDTO productDTO)
        {
            var product = _mapper.Map<ProductDTO, Product>(productDTO);
            DBOperationStatus status = await _productRepository.UpdateAsync(product);
            ProductBL productBL = new ProductBL(product);
            productBL.OnProductUpdated += ProductBL_OnProductUpdated;
            productBL.OnProductUpdateFailed += ProductBL_OnProductUpdateFailed;
            productBL.RaiseProductUpdatedEvent(status);            
            return status;
        }

        private void ProductBL_OnProductUpdated(ProductBL sender, Product product)
        {
            _messageBroker.SendProductChangeMessageAsync(product);
        }

        private void ProductBL_OnProductUpdateFailed(ProductBL sender, Product product)
        {
            throw new NotImplementedException();
        }
                
    }
}
