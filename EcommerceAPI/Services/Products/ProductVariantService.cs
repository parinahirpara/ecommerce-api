using AutoMapper;
using EcommerceAPI.Dto.Admin.Products;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Interfaces.Services.Products;
using EcommerceAPI.Models.Products;
using System.Text.Json;

namespace EcommerceAPI.Services.Products
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public ProductVariantService(IProductVariantRepository productVariantRepository, IProductImageRepository productImageRepository, IWebHostEnvironment environment, IMapper mapper)
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
            if (variant == null) return null;

            return _mapper.Map<ProductVariantResponseDto>(variant);
        }

        public async Task<ProductVariantResponseDto> CreateVariantAsync(ProductVariantDto dto)
        {
            var variantId = Guid.NewGuid();

            // Instantiate domain entity mappings based on incoming parameters
            var variant = _mapper.Map<ProductVariant>(dto);
            variant.Id = variantId;
            variant.Images = new List<ProductImage>();

            // Parse UI image configurations (Descriptions & display placement indexing)
            var metadataList = ParseImageMetadata(dto.ImagesMetadata);

            // Process image file arrays
            if (dto.Files != null && dto.Files.Count > 0)
            {
                variant.Images = await ProcessUploadedFilesAsync(dto.Files, metadataList, dto.ProductId, variantId);
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

            _mapper.Map(dto, existingVariant);

            var metadataList = ParseImageMetadata(dto.ImagesMetadata);

            if (existingVariant.Images == null)
                existingVariant.Images = new List<ProductImage>();

            var existingImages = existingVariant.Images.ToList();

            // Strategy Update: Track retained image records by checking explicit unique database IDs
            // instead of trying to evaluate raw relative URL strings alone.
            var retainedImageIds = metadataList
                .Where(x => x.Id.HasValue && x.Id.Value != Guid.Empty)
                .Select(x => x.Id.Value)
                .ToHashSet();

            // Identify images that were explicitly removed by the user in the UI gallery layout
            var imagesToDelete = existingImages
                .Where(x => !retainedImageIds.Contains(x.Id))
                .ToList();

            foreach (var image in imagesToDelete)
            {
                string fullPath = Path.Combine(
                    _environment.WebRootPath,
                    image.ImageUrl.TrimStart('/'));

                if (File.Exists(fullPath))
                    File.Delete(fullPath);

                existingVariant.Images.Remove(image);
            }

            // Update fields (Alt Text and Display Order) for retained database records
            foreach (var image in existingVariant.Images)
            {
                var meta = metadataList.FirstOrDefault(x => x.Id == image.Id);

                if (meta != null)
                {
                    image.AltText = meta.Description;
                    image.DisplayOrder = meta.SortOrder;
                }
            }

            // Process and append newly uploaded image streams
            if (dto.Files != null && dto.Files.Count > 0)
            {
                var newImages = await ProcessUploadedFilesAsync(
                    dto.Files,
                    metadataList,
                    dto.ProductId,
                    variantId);

                foreach (var img in newImages)
                {
                    existingVariant.Images.Add(img);
                }
            }

            _productVariantRepository.Update(existingVariant);
            await _productVariantRepository.SaveChangesAsync();

            return _mapper.Map<ProductVariantResponseDto>(existingVariant);
        }

        // Helper: Process raw file streams sequentially following wiped reset states
        private async Task<ICollection<ProductImage>> ProcessUploadedFilesAsync(
           List<IFormFile> files,
           List<VariantImageMetadataDto> metadataList,
           Guid productId,
           Guid variantId)
        {
            var imagesList = new List<ProductImage>();

            // Filter metadata to find only entries designated for brand new images (Id is null or empty)
            var brandNewImagesMetadata = metadataList
                .Where(x => !x.Id.HasValue || x.Id.Value == Guid.Empty)
                .ToList();

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                {
                    throw new Exception($"File format rejected for '{file.FileName}'. Only JPG/JPEG/PNG file formats are acceptable.");
                }

                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var relativePath = Path.Combine("uploads", "products", uniqueFileName);

                // Use Host Environment pathing consistently over Directory.GetCurrentDirectory() 
                var absolutePath = Path.Combine(_environment.WebRootPath, relativePath);

                var directoryPath = Path.GetDirectoryName(absolutePath);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath!);

                using (var stream = new FileStream(absolutePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Reliably match the physical file array index with the target metadata index order 
                var meta = brandNewImagesMetadata.ElementAtOrDefault(i);

                imagesList.Add(new ProductImage
                {
                    Id = Guid.NewGuid(), // Initialize primary key for newly created entities
                    ImageUrl = "/" + relativePath.Replace("\\", "/"),
                    AltText = meta?.Description ?? file.FileName,
                    DisplayOrder = meta?.SortOrder ?? (i + 1),
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

            await _productVariantRepository.DeleteVariantAsync(id);
            await _productVariantRepository.SaveChangesAsync();
            return true;
        }
    }
}
