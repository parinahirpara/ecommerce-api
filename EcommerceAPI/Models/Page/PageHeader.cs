namespace EcommerceAPI.Models.Page
{
    public class PageHeader : BaseEntity
    {
        public string PageKey { get; set; } 
        public string PageTitle { get; set; }
        public string Description { get; set; }

        public ICollection<PageQuickLink> QuickLinks { get; set; } = new List<PageQuickLink>();
    }
}
