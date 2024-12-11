using CrowdFundingApp.Models;

namespace CrowdFundingApp.ViewModels
{
    public class UserDashboardViewModel
    {
        public IEnumerable<User>? Users { get; set; }
        public IEnumerable<UserReward>? UserRewards { get; set; }
        public IEnumerable<Project>? Projects { get; set; }
        public IEnumerable<Reward>? Rewards { get; set; }
        public IEnumerable<Contribution>? Contributions { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
