namespace CrowdFundingApp.Models
{
    public class ContributionStatistics
    {
        public double TotalInvested { get; set; }
        public int ProjectsInvested { get; set; }
        public int RewardsEarned { get; set; }
        public List<Contribution> ActiveInvestments { get; set; }
    }
}
