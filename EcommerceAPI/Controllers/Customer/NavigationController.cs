using EcommerceAPI.Dto.Customer.Navigation;
using EcommerceAPI.Interfaces.Services.Navigation;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers.Customer
{
    [ApiController]
    [Route("api/[controller]")]
    public class NavigationController : ControllerBase
    {
        private readonly INavigationService _navigationService;

        public NavigationController(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [HttpGet("config")]
        public async Task<IActionResult> GetFullNavigationConfig()
        {
            try
            {
                // 1. Fetch data from service layer exactly once
                List<MenuResponseDto> sourceMenuData = await _navigationService.GetMenuConfigurationAsync();

                if (sourceMenuData == null || !sourceMenuData.Any())
                {
                    return NotFound(new { message = "No navigation data structural setup could be found." });
                }

                var dynamicRoutes = new List<RouteDto>();
                var processedPaths = new HashSet<string>();

                // 2. Traversal Loop to calculate all route paths from source data
                foreach (var categoryNode in sourceMenuData)
                {
                    bool isJewelleryRoot = categoryNode.Label.Equals("Jewellery", StringComparison.OrdinalIgnoreCase);

                    Guid? currentCategoryId = isJewelleryRoot ? null : categoryNode.Id;
                    string categorySlug = categoryNode.Route.TrimStart('/');

                    // POSSIBILITY 1: Base category routes
                    AddRouteHelper(dynamicRoutes, processedPaths, categoryNode.Route, categorySlug, currentCategoryId);

                    if (categoryNode.MegaMenu == null) continue;

                    var categoryGroup = categoryNode.MegaMenu.FirstOrDefault(m => m.Title == "Category");
                    var materialGroup = categoryNode.MegaMenu.FirstOrDefault(m => m.Title == "Material");

                    // SPECIAL ROOT HANDLER: Jewellery
                    if (isJewelleryRoot)
                    {
                        if (categoryGroup?.Items != null)
                        {
                            foreach (var catItem in categoryGroup.Items)
                            {
                                string targetCatSlug = catItem.Route.TrimStart('/');
                                AddRouteHelper(dynamicRoutes, processedPaths, catItem.Route, targetCatSlug, catItem.Id);
                            }
                        }

                        if (materialGroup?.Items != null)
                        {
                            foreach (var matItem in materialGroup.Items)
                            {
                                string[] segments = matItem.Route.TrimStart('/').Split('/');
                                string materialSlug = segments.Last();

                                AddRouteHelper(dynamicRoutes, processedPaths, matItem.Route, "jewellery", null, material: materialSlug, materialId: matItem.Id);
                            }
                        }
                        continue;
                    }

                    // POSSIBILITY 3: :Category/:subCategory
                    if (categoryGroup?.Items != null)
                    {
                        foreach (var subItem in categoryGroup.Items)
                        {
                            string[] segments = subItem.Route.TrimStart('/').Split('/');
                            if (segments.Length == 1 && segments[0] == categorySlug) continue;

                            string subCategorySlug = segments.Last();

                            AddRouteHelper(dynamicRoutes, processedPaths, subItem.Route, categorySlug, currentCategoryId, subCategoryKey: subCategorySlug, subCategoryId: subItem.Id);
                        }
                    }

                    // POSSIBILITY 2: :Category/:Material
                    if (materialGroup?.Items != null)
                    {
                        foreach (var matItem in materialGroup.Items)
                        {
                            string[] segments = matItem.Route.TrimStart('/').Split('/');
                            string materialSlug = segments.Last();

                            AddRouteHelper(dynamicRoutes, processedPaths, matItem.Route, categorySlug, currentCategoryId, material: materialSlug, materialId: matItem.Id);
                        }
                    }

                    // POSSIBILITY 4: :Category/:SubCategory/:Material
                    if (categoryGroup?.Items != null && materialGroup?.Items != null)
                    {
                        foreach (var subItem in categoryGroup.Items)
                        {
                            string[] subSegments = subItem.Route.TrimStart('/').Split('/');
                            if (subSegments.Length < 2) continue;

                            string subCategorySlug = subSegments[1];

                            foreach (var matItem in materialGroup.Items)
                            {
                                string[] matSegments = matItem.Route.TrimStart('/').Split('/');
                                if (matSegments.Length < 2) continue;

                                string materialSlug = matSegments[1];
                                string combinedDeepPath = $"{categorySlug}/{subCategorySlug}/{materialSlug}";

                                AddRouteHelper(dynamicRoutes, processedPaths, combinedDeepPath, categorySlug, currentCategoryId, subCategorySlug, subItem.Id, materialSlug, matItem.Id);
                            }
                        }
                    }
                }

                // Append product fallback catch-alls at the tail end
                dynamicRoutes.Add(new RouteDto { Path = ":category/:subCategory/:productId/:variantId", Component = "ProductDetailComponent" });
                dynamicRoutes.Add(new RouteDto { Path = ":category/:productId/:variantId", Component = "ProductDetailComponent" });

                return Ok(new NavigationConfigDto
                {
                    Menu = sourceMenuData,
                    Routes = dynamicRoutes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An internal server error occurred while building initialization models.", details = ex.Message });
            }
        }
        private void AddRouteHelper(
            List<RouteDto> routeList,
            HashSet<string> exclusionSet,
            string rawRoute,
            string categoryKey,
            Guid? categoryId,
            string? subCategoryKey = null,
            Guid? subCategoryId = null,
            string? material = null,
            Guid? materialId = null)
        {
            string cleanPath = rawRoute.TrimStart('/');
            if (string.IsNullOrEmpty(cleanPath) || exclusionSet.Contains(cleanPath)) return;

            routeList.Add(new RouteDto
            {
                Path = cleanPath,
                Component = "JewelleryComponent",
                Data = new RouteDataDto
                {
                    CategoryKey = categoryKey,
                    CategoryId = categoryId,
                    SubCategoryKey = subCategoryKey,
                    SubCategoryId = subCategoryId,
                    Material = material,
                    MaterialId = materialId
                }
            });

            exclusionSet.Add(cleanPath);
        }
    }
}