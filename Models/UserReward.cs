using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdFundingApp.Models
{
    public class UserReward
    {
        [Key]
        public int UserRewardId { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Reward")]
        public int RewardId { get; set; }
        public Reward? Reward { get; set; }
        public DateTime DateAwarded { get; set; }
    }
}
