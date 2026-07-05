
using AutoMapper;
using EcommerceAPI.Dto.Admin.Page;
using EcommerceAPI.Dto.Admin.Products;
using EcommerceAPI.Models.Page;
using EcommerceAPI.Models.Products;
using Admin = EcommerceAPI.Dto.Admin.Products;
using Customer = EcommerceAPI.Dto.Customer.Products;

namespace EcommerceAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Category, Admin.CategoryDto>();
            CreateMap<Admin.CategoryDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.SubCategories, opt => opt.Ignore());

            CreateMap<SubCategory, SubCategoryDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src =>
                src.Category != null ? src.Category.CategoryName : string.Empty));

            // 2. Mapping DTO -> Entity (For post/put mutations)
            CreateMap<SubCategoryDto, SubCategory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore()); // Prevent EF tracking noise

            CreateMap<Material, Admin.MaterialDto>();
            CreateMap<Admin.MaterialDto, Material>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

            CreateMap<PageHeader, PageHeaderDto>();
            CreateMap<PageQuickLink, PageQuickLinkDto>();

            CreateMap<PageHeaderCreateDto, PageHeader>()
            .ForMember(dest => dest.QuickLinks, opt => opt.Ignore());

            CreateMap<Product, Admin.ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : string.Empty))
            .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory != null ? src.SubCategory.SubCategoryName : string.Empty));
            CreateMap<Admin.ProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Variants, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<Admin.ProductVariantDto, ProductVariant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());
            CreateMap<ProductVariant, Admin.ProductVariantResponseDto>()
              .ForMember(
                  dest => dest.ProductSKU,
                  opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductSKU : string.Empty)
              )
              .ForMember(
                  dest => dest.MaterialName,
                  opt => opt.MapFrom(src => src.Material != null ? src.Material.MaterialName : string.Empty)
              );

            CreateMap<ProductImage, Admin.ProductImageDto>();

            CreateMap<Product, Customer.ProductResponseDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : string.Empty))
            .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory != null ? src.SubCategory.SubCategoryName : string.Empty));

            CreateMap<ProductVariant, Customer.ProductVariantDto>()
                .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material != null ? src.Material.MaterialName : "Unknown"));

            CreateMap<ProductImage, Customer.ProductImageDto>();
        }
    }
}
