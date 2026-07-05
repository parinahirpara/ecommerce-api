using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models.Page
{
    public class PageQuickLink :BaseEntity
    {
        public string Name { get; set; }       
        public string ImageUrl { get; set; }   
        public string Route { get; set; }      
        public int DisplayOrder { get; set; } 
        public Guid PageHeaderId { get; set; }
        [ForeignKey("PageHeaderId")]
        public PageHeader PageHeader { get; set; }
    }
}
