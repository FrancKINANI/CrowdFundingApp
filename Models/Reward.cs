using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace CrowdFundingApp.Models
{
    public class Reward
    {
        [Key]
        public int RewardId { get; set; }
        [Required]
        [StringLength(100)]
        public string? Name {  get; set; }
        [Required]
        public string? Description { get; set; }

        [Required]
        public double MinimumContribution { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public List<UserReward>? UserRewards { get; set; }
    }
}
