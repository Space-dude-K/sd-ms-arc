using Api_fsc_Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api_fsc_Entities.Configurations
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder
                .ToTable("Device");
            builder
                .Property(p => p.Id)
               .HasColumnType("INTEGER")
               .IsRequired(true);
            builder
                .Property(p => p.Ip)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(true)
                .IsUnicode(true);
            builder
                .Property(p => p.Disk)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(true);
            builder
                .Property(p => p.TotalSpace)
                .HasColumnType("FLOAT")
                .HasMaxLength(256)
                .IsRequired(false);
            builder
                .Property(p => p.FreeSpace)
                .HasColumnType("FLOAT")
                .HasMaxLength(256)
                .IsRequired(false);
            builder
                .Property(p => p.DateTime)
                .HasColumnType("DATETIME")
                .HasMaxLength(256)
                .IsRequired(true);
            builder
                .Property(p => p.UserId)
               .HasColumnType("INTEGER")
               .IsRequired(false);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_Device");
        }
    }
}