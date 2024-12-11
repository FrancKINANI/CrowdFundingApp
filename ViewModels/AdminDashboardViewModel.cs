using System.Collections.Generic;
using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Identity;


namespace CrowdFundingApp.ViewModels
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<User>? Users { get; set; }
        public IEnumerable<UserReward>? UserRewards { get; set; }
        public IEnumerable<Reward>? Rewards { get; set; }
        public IEnumerable<Project>? Projects { get; set; }
        public IEnumerable<Contribution>? Contributions { get; set; }
    }
}
