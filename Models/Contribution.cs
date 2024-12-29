using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CrowdFundingApp.Models
{
    public class Contribution
    {
        [Key]
        public int ContributionId { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        [Display(Name = "Investment Amount")]
        public double Amount { get; set; }

        [Required]
        [Display(Name = "Investment Date")]
        public DateTime ContributionDate { get; set; }

        [Required]
        public string? UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        [NotMapped]
        public List<Reward>? EligibleRewards { get; set; }
    }

}
