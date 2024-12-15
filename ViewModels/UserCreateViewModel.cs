using System.ComponentModel.DataAnnotations;

namespace CrowdFundingApp.ViewModels
{
    public class UserCreateViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

}