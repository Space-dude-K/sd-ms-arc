using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models.File;

namespace Entities.Configuration.File
{
    public class ForumFileConfiguration : IEntityTypeConfiguration<ForumFile>
    {
        public void Configure(EntityTypeBuilder<ForumFile> builder)
        {
            builder
                .ToTable("ForumFile");
            builder
                .Property(p => p.Id)
               .HasColumnType("INTEGER")
               .IsRequired(true);
            builder
                .Property(p => p.Name)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(true)
                .IsUnicode(true);
            builder
                .Property(p => p.Path)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(true)
                .IsUnicode(true);
            builder
                .Property(p => p.ForumUserId)
               .HasColumnType("INTEGER");

            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumFile");
        }
    }
}