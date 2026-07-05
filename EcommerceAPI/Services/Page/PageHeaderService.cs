using AutoMapper;
using EcommerceAPI.Dto.Admin.Page;
using EcommerceAPI.Interfaces.Repositories.Page;
using EcommerceAPI.Interfaces.Services.Page;
using EcommerceAPI.Models.Page;
using System.Text.Json;

namespace EcommerceAPI.Services.Page
{
    public class PageHeaderService : IPageHeaderService
    {
        private readonly IPageHeaderRepository _pageHeaderRepository;
        private readonly IPageQuickLinkRepository _pageQuickLinkRepository; // Assuming you have a sub-repository similar to ProductImage
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public PageHeaderService(
            IPageHeaderRepository pageHeaderRepository,
            IPageQuickLinkRepository pageQuickLinkRepository,
            IMapper mapper,
            IWebHostEnvironment environment)
        {
            _pageHeaderRepository = pageHeaderRepository;
            _pageQuickLinkRepository = pageQuickLinkRepository;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<IEnumerable<PageHeaderDto>> GetAllPageHeadersAsync()
        {
            var entities = await _pageHeaderRepository.GetAllPageHeaderAsync();
            return _mapper.Map<IEnumerable<PageHeaderDto>>(entities);
        }

        public async Task<PageHeaderDto?> GetPageHeaderAsync(string pageKey)
        {
            var entity = await _pageHeaderRepository.GetByPageKeyWithLinksAsync(pageKey);
            return _mapper.Map<PageHeaderDto>(entity);
        }

        // --- SEPARATE ADD API LOGIC ---
        public async Task<PageHeaderDto> CreatePageHeaderAsync(PageHeaderCreateDto dto)
        {
            var pageHeaderId = Guid.NewGuid();
            var newPage = _mapper.Map<PageHeader>(dto);

            newPage.Id = pageHeaderId;
            newPage.QuickLinks = new List<PageQuickLink>();

            // Parse the incoming JSON metadata string sent from Angular
            var metadataList = ParseLinksMetadata(dto.LinksMetadataJson);

            // Filter down to get the target structural placeholder slots for brand new uploads
            if (dto.LinkFiles?.Count > 0)
            {
                // FIX: Replaced string .HasValue and .Value properties with string checks
                var targetSlots = metadataList
                    .Where(x => string.IsNullOrEmpty(x.ImageUrl) ||
                                string.IsNullOrEmpty(x.Id) ||
                                x.Id == Guid.Empty.ToString())
                    .ToList();

                // Process file storage and populate the navigational collection
                newPage.QuickLinks = await SaveAndMapUploadedFilesAsync(dto.LinkFiles, targetSlots, pageHeaderId);
            }

            await _pageHeaderRepository.AddAsync(newPage);
            await _pageHeaderRepository.SaveChangesAsync();

            return _mapper.Map<PageHeaderDto>(newPage);
        }

        // --- SEPARATE EDIT API LOGIC ---
        public async Task<PageHeaderDto> UpdatePageHeaderAsync(Guid pageHeaderId, PageHeaderCreateDto dto)
        {
            var existingPage = await _pageHeaderRepository.GetByIdAsync(pageHeaderId); // Or fetch by key depending on repo layout
            if (existingPage == null)
                throw new Exception("Page configuration layout schema missing.");

            // Flatten structural core domain parameters
            existingPage.PageKey = dto.PageKey.Trim().ToLower();
            existingPage.PageTitle = dto.PageTitle.Trim();
            existingPage.Description = dto.Description.Trim();

            var metadataList = ParseLinksMetadata(dto.LinksMetadataJson);

            // Assuming your repository layer can fetch the links for this header
            var currentDbLinks = await _pageQuickLinkRepository.GetLinksByHeaderIdAsync(pageHeaderId);

            // Create O(1) dictionary lookups for performance identity matches
            var metadataLookup = metadataList
                .Where(x => !string.IsNullOrEmpty(x.Id) && x.Id != Guid.Empty.ToString())
                .ToDictionary(x => Guid.Parse(x.Id!), x => x);
            // 1. Process Structural Deletions
            var linksToDelete = currentDbLinks.Where(link => !metadataLookup.ContainsKey(link.Id)).ToList();
            foreach (var link in linksToDelete)
            {
                DeletePhysicalFile(link.ImageUrl);
                _pageQuickLinkRepository.Delete(link);
            }

            // 2. Process Modifications for Existing Cards
            foreach (var dbLink in currentDbLinks.Where(link => metadataLookup.ContainsKey(link.Id)))
            {
                var meta = metadataLookup[dbLink.Id];
                dbLink.Name = meta.Name;
                dbLink.Route = meta.Route;
                dbLink.DisplayOrder = meta.DisplayOrder;

                _pageQuickLinkRepository.Update(dbLink);
            }

            // 3. Process New File Injections Into Empty Collection Targets
            if (dto.LinkFiles?.Count > 0)
            {
                var targetSlots = metadataList
                    .Where(x => string.IsNullOrEmpty(x.ImageUrl) || string.IsNullOrEmpty(x.Id) || x.Id == Guid.Empty.ToString())
                    .ToList();

                var newLinks = await SaveAndMapUploadedFilesAsync(dto.LinkFiles, targetSlots, pageHeaderId);
                foreach (var link in newLinks)
                {
                    await _pageQuickLinkRepository.AddAsync(link);
                }
            }

            _pageHeaderRepository.Update(existingPage);
            await _pageHeaderRepository.SaveChangesAsync();

            var updatedPage = await _pageHeaderRepository.GetByPageKeyWithLinksAsync(existingPage.PageKey);
            return _mapper.Map<PageHeaderDto>(updatedPage);
        }

        // --- COMPONENT REUSABLE STREAM SECTIONS ---
        private async Task<ICollection<PageQuickLink>> SaveAndMapUploadedFilesAsync(
            List<IFormFile> files,
            List<PageQuickLinkMetadataDto> targetSlots,
            Guid pageHeaderId)
        {
            var linksList = new List<PageQuickLink>();

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                {
                    throw new Exception($"File format rejected for '{file.FileName}'. Only JPG/JPEG/PNG extensions are allowed.");
                }

                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var relativePath = Path.Combine("uploads", "navigation-cards", uniqueFileName);
                var absolutePath = Path.Combine(_environment.WebRootPath, relativePath);

                var directoryPath = Path.GetDirectoryName(absolutePath);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath!);

                using (var stream = new FileStream(absolutePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var currentMetaSlot = targetSlots.ElementAtOrDefault(i);

                linksList.Add(new PageQuickLink
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = "/" + relativePath.Replace("\\", "/"),
                    Name = currentMetaSlot?.Name ?? "Navigation Item",
                    Route = currentMetaSlot?.Route ?? "/",
                    DisplayOrder = currentMetaSlot?.DisplayOrder ?? (i + 1),
                    PageHeaderId = pageHeaderId
                });
            }

            return linksList;
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

        private List<PageQuickLinkMetadataDto> ParseLinksMetadata(string metadataJson)
        {
            if (string.IsNullOrEmpty(metadataJson)) return new List<PageQuickLinkMetadataDto>();

            return JsonSerializer.Deserialize<List<PageQuickLinkMetadataDto>>(metadataJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<PageQuickLinkMetadataDto>();
        }
    }
}