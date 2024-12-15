using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdFundingApp.Models
{
    public class User : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<Project>? Projects { get; set; }
        public List<Contribution>? Contributions { get; set; }
        public List<UserReward>? UserRewards { get; set; }

    }
}
