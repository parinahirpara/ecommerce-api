using EcommerceAPI.Dto.Admin.Products;

namespace EcommerceAPI.Interfaces.Services.Products
{
    public interface IMaterialService
    {
        Task<IEnumerable<MaterialDto>> GetAllMaterialAsync();
        Task<MaterialDto?> GetMaterialByIdAsync(Guid id);
        Task<MaterialDto> CreateMaterialAsync(MaterialDto dto);
        Task<MaterialDto?> UpdateMaterialAsync(Guid id, MaterialDto dto);
        Task<bool> DeleteMaterialAsync(Guid id);
    }
}
