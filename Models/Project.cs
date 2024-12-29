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
        [Range(1, double.MaxValue, ErrorMessage = "Goal amount must be greater than 0")]
        public double GoalAmount { get; set; }

        public double CurrentAmount { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<Contribution>? Contributions { get; set; }
        public List<Reward>? Rewards { get; set; }

        // New fields
        public string? ImageUrl { get; set; }

        [Required]
        public ProjectStatus Status { get; set; } = ProjectStatus.Draft;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public double FundingProgress => GoalAmount > 0 ? (CurrentAmount / GoalAmount) * 100 : 0;

        [NotMapped]
        public int DaysRemaining => (EndDate - DateTime.Now).Days;
    }

    public enum ProjectStatus
    {
        Draft,
        Active,
        Funded,
        Expired,
        Cancelled
    }

}
