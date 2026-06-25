using AutoMapper;
using Admin = EcommerceAPI.Dto.Admin.Products;
using Customer = EcommerceAPI.Dto.Customer.Products;
using EcommerceAPI.Dto.Common;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Interfaces.Services.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Services.Products
{

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IWebHostEnvironment environment, IMapper mapper)
        {
            _productRepository = productRepository;
            _environment = environment;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Admin.ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<Admin.ProductDto>>(products);
        }

        public async Task<Admin.    ProductDto?> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetWithDetailsAsync(id);
            if (product == null) return null;

            return _mapper.Map<Admin.ProductDto>(product);
        }

        public async Task<Admin.ProductDto> AddAsync(Admin.ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            product.Id = Guid.NewGuid();
            product.CreatedDate = DateTime.UtcNow;

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return _mapper.Map<Admin.ProductDto>(product);
        }

        public async Task UpdateAsync(Guid id, Admin.ProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) throw new Exception("Product missing");

            _mapper.Map(dto, product);
            product.ModifiedDate = DateTime.UtcNow;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _productRepository.GetProductDetailsByIdAsync(id);
            if (product == null) return false;

            if (product.Variants != null)
            {
                foreach (var variant in product.Variants)
                {
                    DeletePhysicalImages(variant.Images);
                }
            }

            _productRepository.Delete(product);
            await _productRepository.SaveChangesAsync();
            return true;

        }
        private void DeletePhysicalImages(IEnumerable<ProductImage> images)
        {
            if (images == null) return;

            foreach (var img in images)
            {
                if (!string.IsNullOrEmpty(img.ImageUrl))
                {
                    string fullPath = Path.Combine(_environment.WebRootPath, img.ImageUrl.TrimStart('/'));
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }
            }
        }
        public async Task<PagedResult<Customer.ProductResponseDto>> GetProductsAsync(int skip, int take, Guid? categoryId, Guid? subCategoryId)
        {
            // 1. Retrieve the paginated db entity tracking package
            PagedResult<Product> dbPagedResult = await _productRepository.GetPagedProductWithDetailsAsync(skip, take, categoryId, subCategoryId);

            // 2. Map the inner entity list to storefront layout view models
            var mappedStorefrontDtos = _mapper.Map<List<Customer.ProductResponseDto>>(dbPagedResult.Items);

            // 3. Re-wrap into the generic pagination DTO structure for the API layer
            return new PagedResult<Customer.ProductResponseDto>
            {
                TotalItems = dbPagedResult.TotalItems,
                CurrentCount = dbPagedResult.CurrentCount,
                Items = mappedStorefrontDtos
            };
        }
    }
}
