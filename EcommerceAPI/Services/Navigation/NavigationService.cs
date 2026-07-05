using EcommerceAPI.Dto.Customer.Navigation;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Interfaces.Services.Navigation;
using EcommerceAPI.Models.Products;
using Microsoft.AspNetCore.SignalR;

namespace EcommerceAPI.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly ICategoryRepository _categoryRepository;

        public NavigationService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<MenuResponseDto>> GetMenuConfigurationAsync()
        {
            // Fetch categories and products data arrays from repository
            var (categories, products) = await _categoryRepository.GetMenuConfigurationDataAsync();

            // Group products dynamically by CategoryId in-memory
            var productsByCategory = products.ToLookup(p => p.CategoryId);

            var menuResult = new List<MenuResponseDto>();

            var allActiveMaterials = products
                .SelectMany(p => p.Variants)
                .Select(v => v.Material)
                .Where(m => m != null && m.IsActive)
                .GroupBy(m => m.Id)
                .Select(g => g.First())
                .ToList();

            // 2. Filter out any database-level placeholder records for "Jewellery"
            var databaseCategories = categories
                .Where(c => !c.CategoryName.Equals("Jewellery", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var jewelleryMegaMenu = new List<MegaMenuSectionDto>
    {
        new MegaMenuSectionDto
        {
            Title = "Category",
            Items = databaseCategories.Select(c => new MenuItemDto
            {
                Label = c.CategoryName,
                Route = $"/{GenerateUrlSegment(c.CategoryName)}", // e.g., /charms, /bracelets, /rings
                Id = c.Id
            }).ToList()
        },
        new MegaMenuSectionDto
        {
            Title = "Material",
            Items = allActiveMaterials.Select(m => new MenuItemDto
            {
                Label = m.MaterialName,
                Route = $"/jewellery/{GenerateUrlSegment(m.MaterialName)}", // e.g., /jewellery/sterling-silver
                Id = m.Id
            }).ToList()
        }
    };
            menuResult.Add(new MenuResponseDto { Label = "Jewellery", Route = "/jewellery", MegaMenu = jewelleryMegaMenu });

            // -------------------------------------------------------------------------
            // 2. DATABASE ITERATIONS: Main Sibling Category Links (Charms, Rings, etc.)
            // -------------------------------------------------------------------------
            foreach (var category in databaseCategories)
            {
                var catSegment = GenerateUrlSegment(category.CategoryName);
                var megaMenuSections = new List<MegaMenuSectionDto>();

                // Step A: Populate Subcategory Data
                if (category.SubCategories != null && category.SubCategories.Any())
                {
                    var categoryItems = new List<MenuItemDto>
            {
                new MenuItemDto { Label = $"All {category.CategoryName}", Route = $"/{catSegment}", Id = category.Id }
            };

                    foreach (var sub in category.SubCategories)
                    {
                        categoryItems.Add(new MenuItemDto
                        {
                            Label = sub.SubCategoryName,
                            Route = $"/{catSegment}/{GenerateUrlSegment(sub.SubCategoryName)}",
                            Id = sub.Id
                        });
                    }
                    megaMenuSections.Add(new MegaMenuSectionDto { Title = "Category", Items = categoryItems });
                }

                // Step B: Populate Category-Specific Material Arrays
                var associatedProducts = productsByCategory[category.Id];
                var categoryMaterials = associatedProducts
                    .SelectMany(p => p.Variants)
                    .Select(v => v.Material)
                    .Where(m => m != null && m.IsActive)
                    .GroupBy(m => m.Id)
                    .Select(g => g.First())
                    .ToList();

                if (categoryMaterials.Any())
                {
                    var materialItems = categoryMaterials.Select(m => new MenuItemDto
                    {
                        Label = m.MaterialName,
                        Route = $"/{catSegment}/{GenerateUrlSegment(m.MaterialName)}",
                        Id = m.Id
                    }).ToList();

                    megaMenuSections.Add(new MegaMenuSectionDto { Title = "Material", Items = materialItems });
                }

                menuResult.Add(new MenuResponseDto
                {
  
                    Label = category.CategoryName,
                    Route = $"/{catSegment}",
                    MegaMenu = megaMenuSections.Any() ? megaMenuSections : null
                });
            }

            return menuResult;
        }

        //private void AddSaleMenuShell(List<MenuResponseDto> list)
        //{
        //    list.Add(new MenuResponseDto
        //    {
        //        Label = "Sale",
        //        Route = "/sale",
        //        MegaMenu = new List<MegaMenuSectionDto>
        //    {
        //        new MegaMenuSectionDto
        //        {
        //            Title = "Category",
        //            Items = new List<MenuItemDto>
        //            {
        //                new MenuItemDto { Label = "Shop All Sale", Route = "/sale" },
        //                new MenuItemDto { Label = "Sale Charms", Route = "/sale" },
        //                new MenuItemDto { Label = "Sale Bracelets", Route = "/sale" },
        //                new MenuItemDto { Label = "Sale Rings", Route = "/sale" },
        //                new MenuItemDto { Label = "Sale Earrings", Route = "/sale" },
        //                new MenuItemDto { Label = "Sale Necklaces", Route = "/sale" }
        //            }
        //        },
        //        new MegaMenuSectionDto
        //        {
        //            Title = "Material",
        //            Items = new List<MenuItemDto>
        //            {
        //                new MenuItemDto { Label = "Sterling Silver", Route = "/sale" },
        //                new MenuItemDto { Label = "Rose Gold-plated", Route = "/sale" },
        //                new MenuItemDto { Label = "Gold-plated", Route = "/sale" },
        //                new MenuItemDto { Label = "Two-Tone", Route = "/sale" }
        //            }
        //        }
        //    }
        //    });
        //}

        private string GenerateUrlSegment(string phrase)
        {
            if (string.IsNullOrEmpty(phrase)) return "";
            return phrase.ToLower().Trim().Replace(" ", "-").Replace("/", "-").Replace("&", "and");
        }
    }
}
