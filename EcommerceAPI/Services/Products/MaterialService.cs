using AutoMapper;
using EcommerceAPI.Dto.Admin.Products;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Interfaces.Services.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Services.Products
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IMapper _mapper;

        public MaterialService(IMaterialRepository materialRepository, IMapper mapper)
        {
            _materialRepository = materialRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MaterialDto>> GetAllMaterialAsync()
        {
            var materials = await _materialRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MaterialDto>>(materials);
        }

        public async Task<MaterialDto?> GetMaterialByIdAsync(Guid id)
        {
            var material = await _materialRepository.GetByIdAsync(id);
            if (material == null) return null;

            return _mapper.Map<MaterialDto>(material);
        }

        public async Task<MaterialDto> CreateMaterialAsync(MaterialDto dto)
        {
            var material = _mapper.Map<Material>(dto);

            material.MaterialName = dto.MaterialName.Trim();
            material.CreatedDate = DateTime.UtcNow;
            material.IsActive = true;

            await _materialRepository.AddAsync(material);
            await _materialRepository.SaveChangesAsync();

            return _mapper.Map<MaterialDto>(material);
        }

        public async Task<MaterialDto?> UpdateMaterialAsync(Guid id, MaterialDto dto)
        {
            var existingMaterial = await _materialRepository.GetByIdAsync(id);
            if (existingMaterial == null) return null;

            // Map incoming DTO changes onto the tracked entity instance
            _mapper.Map(dto, existingMaterial);
            existingMaterial.MaterialName = dto.MaterialName.Trim();

            _materialRepository.Update(existingMaterial);
            await _materialRepository.SaveChangesAsync();

            return _mapper.Map<MaterialDto>(existingMaterial);
        }

        public async Task<bool> DeleteMaterialAsync(Guid id)
        {
            var material = await _materialRepository.GetByIdAsync(id);
            if (material == null) return false;

            _materialRepository.Delete(material);
            await _materialRepository.SaveChangesAsync();

            return true;
        }
    }
}