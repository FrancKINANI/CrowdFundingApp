using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CrowdFundingApp.Models
{
    public class CrowdFundingDbContext : IdentityDbContext<User>
    {
        public CrowdFundingDbContext(DbContextOptions<CrowdFundingDbContext> options) : base(options) { }
        public DbSet<User>? Users { get; set; }
        public DbSet<Project>? Projects { get; set; }
        public DbSet<Contribution>? Contributions { get; set; }
        public DbSet<Reward>? Rewards { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<UserReward>? UserRewards { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<User>("User");

            modelBuilder.Entity<User>()
                .HasMany(u => u.Projects)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Contributions)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserRewards)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<Project>()
            .HasOne(p => p.User)
            .WithMany(u => u.Projects)
            .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Contribution>()
                .HasOne(c => c.User)
                .WithMany(u => u.Contributions)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<UserReward>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRewards)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Contributions)
                .WithOne(c => c.Project)
                .HasForeignKey(c => c.ProjectId);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Rewards)
                .WithOne(r => r.Project)
                .HasForeignKey(r => r.ProjectId);

            modelBuilder.Entity<Reward>()
                .HasMany(r => r.UserRewards)
                .WithOne(ur => ur.Reward)
                .HasForeignKey(ur => ur.RewardId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Projects)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}