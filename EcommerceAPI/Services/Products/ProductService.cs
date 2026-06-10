using EcommerceAPI.Dto.Products;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Interfaces.Services.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Services.Products
{

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _environment;

        public ProductService(IProductRepository productRepository, IWebHostEnvironment environment)
        {
            _productRepository = productRepository;
            _environment = environment;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() => await _productRepository.GetAllWithDetailsAsync();

        public async Task<Product?> GetByIdAsync(Guid id) => await _productRepository.GetWithDetailsAsync(id);

        public async Task AddAsync(ProductDto dto)
        {
            var product = new Product
            {
                ProductId = Guid.NewGuid(),
                ProductSKU = dto.ProductSKU,
                ProductName = dto.ProductName,
                ProductTitle = dto.ProductTitle,
                ProductDescription = dto.ProductDescription,
                ProductShortDescription = dto.ProductShortDescription,
                ProductBrand = dto.ProductBrand,
                CategoryId = dto.CategoryId,
                SubCategoryId = dto.SubCategoryId,
                IsActive = dto.IsActive,
                CreatedDate = DateTime.UtcNow
            };
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateAsync(Guid id, ProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) throw new Exception("Product missing");

            product.ProductSKU = dto.ProductSKU;
            product.ProductName = dto.ProductName;
            product.ProductTitle = dto.ProductTitle;
            product.ProductDescription = dto.ProductDescription;
            product.ProductShortDescription = dto.ProductShortDescription;
            product.ProductBrand = dto.ProductBrand;
            product.CategoryId = dto.CategoryId;
            product.SubCategoryId = dto.SubCategoryId;
            product.IsActive = dto.IsActive;
            product.ModifiedDate = DateTime.UtcNow;

            await _productRepository.Update(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _productRepository.GetProductDetailsByIdAsync(id);
            if (product != null)
            {
                if (product.Variants != null)
                {
                    foreach (var variant in product.Variants)
                    {
                        DeletePhysicalImages(variant.Images);
                    }
                }
                await _productRepository.Delete(product);
            }
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
    }
}
