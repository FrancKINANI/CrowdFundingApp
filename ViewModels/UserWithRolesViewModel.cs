using System.Collections.Generic;

namespace CrowdFundingApp.Models
{
    public class UserWithRolesViewModel
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public List<string>? Roles { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
