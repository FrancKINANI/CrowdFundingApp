﻿using System.ComponentModel.DataAnnotations;

namespace CrowdFundingApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }

        public ICollection<Project> Projects { get; set; }
        public ICollection<Contribution> Contributions { get; set; }

    }
}
