using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? Bio { get; set; }

    // Startup-specific fields
    public string? CompanyName { get; set; }
    public string? BusinessDescription { get; set; }

    // Investor-specific fields
    public decimal? InvestmentCapacity { get; set; }
    public string? InvestmentPreferences { get; set; }

    public List<Project>? Projects { get; set; }
    public List<Contribution>? Contributions { get; set; }
    public List<UserReward>? UserRewards { get; set; }
}