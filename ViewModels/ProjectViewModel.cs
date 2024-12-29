// ViewModels/ProjectViewModel.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using CrowdFundingApp.Models;

namespace CrowdFundingApp.ViewModels
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Goal amount must be greater than 0")]
        [Display(Name = "Goal Amount")]
        public double GoalAmount { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Project Image")]
        public IFormFile? ImageFile { get; set; }

        public string? ImageUrl { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public double CurrentAmount { get; set; }
        public double FundingProgress { get; set; }
        public int DaysRemaining { get; set; }
    }
}