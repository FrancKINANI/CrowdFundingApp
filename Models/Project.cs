using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CrowdFundingApp.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public double GoalAmount { get; set; }

        public double CurrentAmount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        [ForeignKey("User")]
        public string? UserId { get; set; }
        public User? User { get; set; }


        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<Contribution>? Contributions { get; set; }
        public List<Reward>? Rewards { get; set; }
    }

}
