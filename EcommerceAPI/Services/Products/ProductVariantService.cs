using EcommerceAPI.Dto.Products;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Interfaces.Services.Products;
using EcommerceAPI.Models.Products;
using EcommerceAPI.Repositories.Products;
using System.Text.Json;

namespace EcommerceAPI.Services.Products
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IWebHostEnvironment _environment;

        public ProductVariantService(IProductVariantRepository productVariantRepository, IWebHostEnvironment environment)
        {
            _productVariantRepository = productVariantRepository;
            _environment = environment;
        }

        public async Task<ProductVariant> CreateVariantAsync(ProductVariantDto dto)
        {
            var variantId = Guid.NewGuid();

            // Instantiate domain entity mappings based on incoming parameters
            var variant = new ProductVariant
            {
                VariantId = variantId,
                ProductId = dto.ProductId,
                VariantSKU = dto.VariantSKU,
                Size = dto.Size,
                Weight = dto.Weight,
                Price = dto.Price,
                DiscountPercentage = dto.DiscountPercentage,
                StockQuantity = dto.StockQuantity,
                Images = new List<ProductImage>()
            };

            // Parse UI image configurations (Descriptions & display placement indexing)
            var metadataList = ParseImageMetadata(dto.ImagesMetadata);

            // Process image file arrays
            if (dto.Files != null && dto.Files.Count > 0)
            {
                variant.Images = await ProcessUploadedFilesAsync(dto.Files, metadataList, dto.ProductId, variantId);
            }

            await _productVariantRepository.AddAsync(variant);
            return variant;
        }

        public async Task<ProductVariant> UpdateVariantAsync(Guid variantId, ProductVariantDto dto)
        {
            var existingVariant = await _productVariantRepository.GetByIdAsync(variantId);
            if (existingVariant == null) throw new Exception("Product variant could not be found.");

            // Update simple structural parameters
            existingVariant.VariantSKU = dto.VariantSKU;
            existingVariant.Size = dto.Size;
            existingVariant.Weight = dto.Weight;
            existingVariant.Price = dto.Price;
            existingVariant.DiscountPercentage = dto.DiscountPercentage;
            existingVariant.StockQuantity = dto.StockQuantity;

            var metadataList = ParseImageMetadata(dto.ImagesMetadata);

            if (dto.Files != null && dto.Files.Count > 0)
            {
                // Note: Depending on your business flow, you might want to call 
                // a repository clean method here to delete old variant images first.
                var newImages = await ProcessUploadedFilesAsync(dto.Files, metadataList, dto.ProductId, variantId);

                foreach (var img in newImages)
                {
                    existingVariant.Images.Add(img);
                }
            }

            await _productVariantRepository.Update(existingVariant);
            return existingVariant;
        }

        // Helper: Validate and process raw file streams
        private async Task<ICollection<ProductImage>> ProcessUploadedFilesAsync(
            List<IFormFile> files,
            List<VariantImageMetadataDto> metadataList,
            Guid productId,
            Guid variantId)
        {
            var imagesList = new List<ProductImage>();

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var extension = Path.GetExtension(file.FileName).ToLower();

                // Direct file validation matching your requirement (.jpg / .jpeg)
                if (extension != ".jpg" && extension != ".jpeg")
                {
                    throw new Exception($"File format rejected for '{file.FileName}'. Only JPG/JPEG file paths are acceptable.");
                }

                // Generate random hash keys to safeguard unique assets names from storage collision overwrites
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";

                // Save paths inside web project contents root relative directories
                var relativePath = Path.Combine("uploads", "products", uniqueFileName);
                var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

                // Enforce destination directory setup states
                var directoryPath = Path.GetDirectoryName(absolutePath);
                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath!);

                using (var stream = new FileStream(absolutePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Map description metadata based on matching array indices
                var meta = i < metadataList.Count ? metadataList[i] : new VariantImageMetadataDto { Description = "", SortOrder = i + 1 };

                imagesList.Add(new ProductImage
                {
                    ImageUrl = "/" + relativePath.Replace("\\", "/"), // Normalize paths uniformly across platforms
                    AltText = meta.Description,
                    DisplayOrder = meta.SortOrder, // Retain UI sequence ranking directly in DB column mapping
                    ProductId = productId,
                    ProductVariantId = variantId
                });
            }

            return imagesList;
        }

        private List<VariantImageMetadataDto> ParseImageMetadata(string metadataJson)
        {
            if (string.IsNullOrEmpty(metadataJson)) return new List<VariantImageMetadataDto>();

            return JsonSerializer.Deserialize<List<VariantImageMetadataDto>>(metadataJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<VariantImageMetadataDto>();
        }

        public async Task<IEnumerable<ProductVariant>> GetAllVariantsAsync()
        {
            return await _productVariantRepository.GetAllVariantsAsync();
        }

        public async Task<ProductVariant?> GetVariantByIdAsync(Guid id)
        {
            return await _productVariantRepository.GetVariantByIdAsync(id);
        }

        public async Task<bool> DeleteVariantAsync(Guid id)
        {
            var variant = await _productVariantRepository.GetVariantByIdAsync(id);
            if (variant == null) return false;

            if (variant.Images != null)
            {
                foreach (var img in variant.Images)
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

            return await _productVariantRepository.DeleteVariantAsync(id);
        }
    }
}
