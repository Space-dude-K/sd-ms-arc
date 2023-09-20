using Api_fsc_Entities.Configurations;
using Api_fsc_Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_fsc_Entities
{
    public class FscContext : DbContext
    {
        public FscContext(DbContextOptions<FscContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new DeviceConfiguration());
        }
        public DbSet<Device> Devices { get; set; }
    }
}