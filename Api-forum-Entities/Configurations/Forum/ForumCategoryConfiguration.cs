using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models.Forum;

namespace Entities.Configuration.Forum
{
    public class ForumCategoryConfiguration : IEntityTypeConfiguration<ForumCategory>
    {
        public void Configure(EntityTypeBuilder<ForumCategory> builder)
        {
            #region DbStructure
            builder
                .ToTable("ForumCategory");
            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.Name)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(false)
                .IsUnicode(true);
            builder
                .Property(p => p.TotalForums)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.TotalTopics)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.CreatedAt)
                .HasColumnType("Date")
                .IsRequired(false);
            builder
                .Property(p => p.UpdatedAt)
                .HasColumnType("Date")
                .IsRequired(false);
            builder
                .Property(p => p.ForumUserId)
                .HasColumnType("INTEGER");

            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumCategory");
            builder
                .HasMany(p => p.ForumBases)
                .WithOne(p => p.ForumCategory)
                .HasForeignKey(p => p.ForumCategoryId)
                .HasConstraintName("FK_ForumCategory_ForumBase_Id")
                .OnDelete(DeleteBehavior.SetNull);
            builder
                .HasOne(p => p.ForumUser)
                .WithMany()
                .HasConstraintName("FK_ForumCategory_ForumUser_Id")
                .OnDelete(DeleteBehavior.SetNull);
            #endregion

            #region DbDataSeed
            builder.HasData(
                new ForumCategory()
                {
                    Id = 1,
                    Name = "Test category 1",
                    CreatedAt = DateTime.Now,
                    ForumUserId = 1,
                    TotalForums = 1,
                    TotalTopics = 4
                },
                new ForumCategory()
                {
                    Id = 2,
                    Name = "Test category 2",
                    CreatedAt = DateTime.Now,
                    ForumUserId = 1,
                    TotalForums = 5,
                    TotalTopics = 4
                },
                new ForumCategory()
                {
                    Id = 3,
                    Name = "Test category 3",
                    CreatedAt = DateTime.Now,
                    ForumUserId = 1
                },
                new ForumCategory()
                {
                    Id = 4,
                    Name = "Test category 4",
                    CreatedAt = DateTime.Now,
                    ForumUserId = 1
                },
                new ForumCategory()
                {
                    Id = 5,
                    Name = "Test category 5",
                    CreatedAt = DateTime.Now,
                    ForumUserId = 1
                },
                new ForumCategory()
                {
                    Id = 6,
                    Name = "Test category 6",
                    CreatedAt = DateTime.Now,
                    ForumUserId = 1
                }
            );
            #endregion
        }
    }
}