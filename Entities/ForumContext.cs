using Entities.Configuration;
using Entities.Configuration.File;
using Entities.Configuration.Forum;
using Entities.Models;
using Entities.Models.Forum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ForumContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public ForumContext(DbContextOptions<ForumContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ForumUserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            modelBuilder.ApplyConfiguration(new ForumFileConfiguration());
            
            modelBuilder.ApplyConfiguration(new ForumCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ForumBaseConfiguration());
            modelBuilder.ApplyConfiguration(new ForumTopicConfiguration());
            modelBuilder.ApplyConfiguration(new ForumPostConfiguration());
            modelBuilder.ApplyConfiguration(new ForumTopicCounterConfiguration());

            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new ForumAccountTypeConfiguration());

            // Seeding the relation between our Admin and roles to AspNetUserRoles table
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>
                {
                    RoleId = 1,
                    UserId = 1
                },
                new IdentityUserRole<int>
                {
                    RoleId = 2,
                    UserId = 1
                }
            );
        }
        public DbSet<ForumUser> ForumUsers { get; set; }
        public DbSet<ForumCategory> ForumCategories { get; set; }
        public DbSet<ForumBase> ForumBases { get; set; }
        public DbSet<ForumTopic> ForumTopics { get; set; }
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<ForumAccountType> ForumAccountTypes { get; set; }
    }
}