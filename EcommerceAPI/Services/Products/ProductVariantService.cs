using AutoMapper;
using EcommerceAPI.Dto.Admin.Products;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Interfaces.Services.Products;
using EcommerceAPI.Models.Products;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace EcommerceAPI.Services.Products
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public ProductVariantService(
            IProductVariantRepository productVariantRepository,
            IProductImageRepository productImageRepository,
            IWebHostEnvironment environment,
            IMapper mapper)
        {
            _productVariantRepository = productVariantRepository;
            _productImageRepository = productImageRepository;
            _environment = environment;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductVariantResponseDto>> GetAllVariantsAsync()
        {
            var variants = await _productVariantRepository.GetAllVariantsAsync();
            return _mapper.Map<IEnumerable<ProductVariantResponseDto>>(variants);
        }

        public async Task<ProductVariantResponseDto?> GetVariantByIdAsync(Guid id)
        {
            var variant = await _productVariantRepository.GetVariantByIdAsync(id);
            return variant == null ? null : _mapper.Map<ProductVariantResponseDto>(variant);
        }

        public async Task<ProductVariantResponseDto> CreateVariantAsync(ProductVariantDto dto)
        {
            var variantId = Guid.NewGuid();
            var variant = _mapper.Map<ProductVariant>(dto);

            variant.Id = variantId;
            variant.Images = new List<ProductImage>();

            var metadataList = ParseImageMetadata(dto.ImagesMetadata);

            if (dto.Files?.Count > 0)
            {
                var targetSlots = metadataList
                    .Where(x => string.IsNullOrEmpty(x.ImageUrl) || !x.Id.HasValue || x.Id.Value == Guid.Empty)
                    .ToList();

                variant.Images = await SaveAndMapUploadedFilesAsync(dto.Files, targetSlots, dto.ProductId, variantId);
            }

            await _productVariantRepository.AddAsync(variant);
            await _productVariantRepository.SaveChangesAsync();

            return _mapper.Map<ProductVariantResponseDto>(variant);
        }

        public async Task<ProductVariantResponseDto> UpdateVariantAsync(Guid variantId, ProductVariantDto dto)
        {
            var existingVariant = await _productVariantRepository.GetVariantByIdAsync(variantId);
            if (existingVariant == null)
                throw new Exception("Product variant not found.");

            // Flatten primitive properties manually to protect image navigation integrity
            existingVariant.ProductId = dto.ProductId;
            existingVariant.VariantSKU = dto.VariantSKU;
            existingVariant.MaterialId = dto.MaterialId;
            existingVariant.Size = dto.Size;
            existingVariant.Weight = dto.Weight;
            existingVariant.Price = dto.Price;
            existingVariant.DiscountPercentage = dto.DiscountPercentage;
            existingVariant.StockQuantity = dto.StockQuantity;

            var metadataList = ParseImageMetadata(dto.ImagesMetadata);
            var currentDbImages = await _productImageRepository.GetImagesByVariantIdAsync(variantId);

            // Create lookup maps for fast O(1) identification
            var metadataLookup = metadataList
                .Where(x => x.Id.HasValue && x.Id.Value != Guid.Empty)
                .ToDictionary(x => x.Id!.Value, x => x);

            // 1. Process Deletions
            var imagesToDelete = currentDbImages.Where(img => !metadataLookup.ContainsKey(img.Id)).ToList();
            foreach (var image in imagesToDelete)
            {
                DeletePhysicalFile(image.ImageUrl);
                _productImageRepository.Delete(image);
            }

            // 2. Process Metadata Updates for Existing Records
            foreach (var dbImage in currentDbImages.Where(img => metadataLookup.ContainsKey(img.Id)))
            {
                var meta = metadataLookup[dbImage.Id];
                dbImage.AltText = meta.Description;
                dbImage.DisplayOrder = meta.SortOrder;

                _productImageRepository.Update(dbImage);
            }

            // 3. Process New Insertions Into Empty Layout Target Positions
            if (dto.Files?.Count > 0)
            {
                var targetSlots = metadataList
                    .Where(x => string.IsNullOrEmpty(x.ImageUrl) || !x.Id.HasValue || x.Id.Value == Guid.Empty)
                    .ToList();

                var newImages = await SaveAndMapUploadedFilesAsync(dto.Files, targetSlots, dto.ProductId, variantId);
                foreach (var img in newImages)
                {
                    await _productImageRepository.AddAsync(img);
                }
            }

            await _productVariantRepository.SaveChangesAsync();

            var updatedVariant = await _productVariantRepository.GetVariantByIdAsync(variantId);
            return _mapper.Map<ProductVariantResponseDto>(updatedVariant);
        }

        public async Task<bool> DeleteVariantAsync(Guid id)
        {
            var variant = await _productVariantRepository.GetVariantByIdAsync(id);
            if (variant == null) return false;

            if (variant.Images != null)
            {
                foreach (var img in variant.Images)
                {
                    DeletePhysicalFile(img.ImageUrl);
                }
            }

            await _productVariantRepository.DeleteVariantAsync(id);
            await _productVariantRepository.SaveChangesAsync();
            return true;
        }


        private async Task<ICollection<ProductImage>> SaveAndMapUploadedFilesAsync(
            List<IFormFile> files,
            List<VariantImageMetadataDto> targetSlots,
            Guid productId,
            Guid variantId)
        {
            var imagesList = new List<ProductImage>();

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                {
                    throw new Exception($"File format rejected for '{file.FileName}'. Only JPG/JPEG/PNG formats are acceptable.");
                }

                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var relativePath = Path.Combine("uploads", "products", uniqueFileName);
                var absolutePath = Path.Combine(_environment.WebRootPath, relativePath);

                var directoryPath = Path.GetDirectoryName(absolutePath);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath!);

                using (var stream = new FileStream(absolutePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var currentMetaSlot = targetSlots.ElementAtOrDefault(i);

                imagesList.Add(new ProductImage
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = "/" + relativePath.Replace("\\", "/"),
                    AltText = currentMetaSlot?.Description ?? file.FileName,
                    DisplayOrder = currentMetaSlot?.SortOrder ?? (i + 1),
                    ProductId = productId,
                    ProductVariantId = variantId
                });
            }

            return imagesList;
        }

        private void DeletePhysicalFile(string? relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl)) return;

            string fullPath = Path.Combine(_environment.WebRootPath, relativeUrl.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        private List<VariantImageMetadataDto> ParseImageMetadata(string metadataJson)
        {
            if (string.IsNullOrEmpty(metadataJson)) return new List<VariantImageMetadataDto>();

            return JsonSerializer.Deserialize<List<VariantImageMetadataDto>>(metadataJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<VariantImageMetadataDto>();
        }
    }
}