using System.ComponentModel.DataAnnotations;

namespace CrowdFundingApp.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }

        public List<Project>? Projects { get; set; }
    }

}
