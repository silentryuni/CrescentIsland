﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace CrescentIsland.Website.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        public User() : base()
        {
            Characters = new List<Character>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }

        // Extended Properties
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime LastLogin { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        public string Country { get; set; }
        public string Biography { get; set; }
        [Required]
        public UserGender UserGender { get; set; }
        public byte[] AvatarImage { get; set; }
        public string AvatarMimeType { get; set; }

        // User Settings
        [Required]
        public bool ShowAge { get; set; }
        [Required]
        public bool ShowMoney { get; set; }
        [Required]
        public bool ShowGender { get; set; }

        [Required]
        public virtual ICollection<Character> Characters { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("LocalConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("Users", "dbo");
            modelBuilder.Entity<User>().ToTable("Users", "dbo").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UsersToRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims").Property(p => p.Id).HasColumnName("ClaimId");
            modelBuilder.Entity<IdentityRole>().ToTable("UserRoles").Property(p => p.Id).HasColumnName("RoleId");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Character> Characters { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<UserItem> UserItems { get; set; }
    }

    public enum UserGender
    {
        Male,
        Female,
        None
    }
}