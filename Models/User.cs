using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdFundingApp.Models
{
    public class User : IdentityUser
    {
        //[Key]
        //public string UserId { get; set; }

        //[Required]
        //[StringLength(50)]
        //public string Username { get; set; }

        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }

        //[Required]
        //public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<Project>? Projects { get; set; }
        public List<Contribution>? Contributions { get; set; }
        public List<UserReward>? UserRewards { get; set; }

    }
}
