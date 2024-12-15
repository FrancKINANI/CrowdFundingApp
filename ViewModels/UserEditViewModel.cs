using System.ComponentModel.DataAnnotations;

namespace CrowdFundingApp.ViewModels
{
    public class UserEditViewModel
    {
        public string? Id { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
    }

}