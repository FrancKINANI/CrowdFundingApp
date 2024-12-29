using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }

    [Required]
    [Display(Name = "Account Type")]
    public string? UserType { get; set; }

    [Display(Name = "Bio")]
    [StringLength(500)]
    public string? Bio { get; set; }

    // Startup-specific fields
    [Display(Name = "Company Name")]
    public string? CompanyName { get; set; }

    [Display(Name = "Business Description")]
    [StringLength(1000)]
    public string? BusinessDescription { get; set; }

    // Investor-specific fields
    [Display(Name = "Investment Capacity")]//to remove
    [DataType(DataType.Currency)]
    public decimal? InvestmentCapacity { get; set; }

    [Display(Name = "Investment Preferences")]
    [StringLength(500)]
    public string? InvestmentPreferences { get; set; }
}