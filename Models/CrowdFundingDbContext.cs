using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class CrowdFundingDbContext : IdentityDbContext<User>
{
    public CrowdFundingDbContext(DbContextOptions<CrowdFundingDbContext> options) : base(options) { }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Contribution> Contributions { get; set; }
    public DbSet<Reward> Rewards { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<UserReward> UserRewards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<User>("User");

        modelBuilder.Entity<User>()
            .Property(u => u.InvestmentCapacity)
            .HasColumnType("decimal(18,2)");

        // Project relationships
        modelBuilder.Entity<Project>()
            .HasOne(p => p.User)
            .WithMany(u => u.Projects)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Contribution relationships
        modelBuilder.Entity<Contribution>()
            .HasOne(c => c.User)
            .WithMany(u => u.Contributions)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Contribution>()
            .HasOne(c => c.Project)
            .WithMany(p => p.Contributions)
            .HasForeignKey(c => c.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Reward relationships
        modelBuilder.Entity<Reward>()
            .HasOne(r => r.Project)
            .WithMany(p => p.Rewards)
            .HasForeignKey(r => r.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserReward>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRewards)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserReward>()
            .HasOne(ur => ur.Reward)
            .WithMany(r => r.UserRewards)
            .HasForeignKey(ur => ur.RewardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}