﻿// <auto-generated />
using System;
using CrowdFundingApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CrowdFundingApp.Migrations
{
    [DbContext(typeof(CrowdFundingDbContext))]
    [Migration("20241205180337_InitialModel")]
    partial class InitialModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CrowdFundingApp.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("CrowdFundingApp.Models.Contribution", b =>
                {
                    b.Property<int>("ContributionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContributionId"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("ContributionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ContributionId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("Contributions");
                });

            modelBuilder.Entity("CrowdFundingApp.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<double>("CurrentAmount")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("GoalAmount")
                        .HasColumnType("float");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ProjectId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("CrowdFundingApp.Models.Reward", b =>
                {
                    b.Property<int>("RewardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RewardId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("MinimumContribution")
                        .HasColumnType("float");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("RewardId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Rewards");
                });

            modelBuilder.Entity("CrowdFundingApp.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CrowdFundingApp.Models.Contribution", b =>
                {
                    b.HasOne("CrowdFundingApp.Models.Project", "Project")
                        .WithMany("Contributions")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CrowdFundingApp.Models.User", "User")
                        .WithMany("Contributions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CrowdFundingApp.Models.Project", b =>
                {
                    b.HasOne("CrowdFundingApp.Models.Category", "Category")
                        .WithMany("Projects")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CrowdFundingApp.Models.User", "User")
                        .WithMany("Projects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CrowdFundingApp.Models.Reward", b =>
                {
                    b.HasOne("CrowdFundingApp.Models.Project", "Project")
                        .WithMany("Rewards")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("CrowdFundingApp.Models.Category", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("CrowdFundingApp.Models.Project", b =>
                {
                    b.Navigation("Contributions");

                    b.Navigation("Rewards");
                });

            modelBuilder.Entity("CrowdFundingApp.Models.User", b =>
                {
                    b.Navigation("Contributions");

                    b.Navigation("Projects");
                });
#pragma warning restore 612, 618
        }
    }
}