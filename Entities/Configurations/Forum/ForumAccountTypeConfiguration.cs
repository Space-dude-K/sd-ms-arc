using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models.Forum;

namespace Entities.Configuration.Forum
{
    public class ForumAccountTypeConfiguration : IEntityTypeConfiguration<ForumAccountType>
    {
        public void Configure(EntityTypeBuilder<ForumAccountType> builder)
        {
            builder
                .ToTable("ForumAccountType");

            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.TypeName)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(false)
                .IsUnicode(true);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumAccountType");
        }
    }
}