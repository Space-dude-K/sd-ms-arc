using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models.Forum;

namespace Entities.Configuration.Forum
{
    public class ForumBaseConfiguration : IEntityTypeConfiguration<ForumBase>
    {
        public void Configure(EntityTypeBuilder<ForumBase> builder)
        {
            #region DbStructure
            builder
                .ToTable("ForumBase");

            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.TotalPosts)
                .HasColumnType("INTEGER");
            builder
                .Property(p => p.TotalTopics)
                .HasColumnType("INTEGER");
            builder
                .Property(p => p.ForumTitle)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(true)
                .IsUnicode(true);
            builder
                .Property(p => p.ForumSubTitle)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(false)
                .IsUnicode(true);
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
                .Property(p => p.TotalViews)
                .HasColumnType("INTEGER")
                .IsRequired(true);

            builder
                .Ignore(c => c.TotalPosts);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumBase");
            builder
                .HasMany(p => p.ForumTopics)
                .WithOne(p => p.ForumBase)
                .HasForeignKey(p => p.ForumBaseId)
                .HasConstraintName("FK_ForumBase_ForumTopic_ForumBaseId")
                .OnDelete(DeleteBehavior.SetNull);
            builder
                .HasOne(p => p.ForumUser)
                .WithMany()
                .HasConstraintName("FK_ForumBase_ForumUser_Id")
                .OnDelete(DeleteBehavior.SetNull);
            #endregion
            #region DbDataSeed
            builder.HasData(
                new ForumBase()
                {
                    Id = 1,
                    ForumTitle = "Test forum title 1",
                    ForumSubTitle = "Test forum subtitle 1",
                    CreatedAt = DateTime.Now,
                    ForumCategoryId = 1,
                    ForumUserId = 1
                },
                new ForumBase()
                {
                    Id = 2,
                    ForumTitle = "Test forum title 2",
                    ForumSubTitle = "Test forum subtitle 2",
                    CreatedAt = DateTime.Now,
                    ForumCategoryId = 2,
                    ForumUserId = 1
                },
                new ForumBase()
                {
                    Id = 3,
                    ForumTitle = "Test forum title 3",
                    ForumSubTitle = "Test forum subtitle 3",
                    CreatedAt = DateTime.Now,
                    ForumCategoryId = 2,
                    ForumUserId = 1
                },
                new ForumBase()
                {
                    Id = 4,
                    ForumTitle = "Test forum title 4",
                    ForumSubTitle = "Test forum subtitle 4",
                    CreatedAt = DateTime.Now,
                    ForumCategoryId = 2,
                    ForumUserId = 1
                },
                new ForumBase()
                {
                    Id = 5,
                    ForumTitle = "Test forum title 5",
                    ForumSubTitle = "Test forum subtitle 5",
                    CreatedAt = DateTime.Now,
                    ForumCategoryId = 2,
                    ForumUserId = 1
                },
                new ForumBase()
                {
                    Id = 6,
                    ForumTitle = "Test forum title 6",
                    ForumSubTitle = "Test forum subtitle 6",
                    CreatedAt = DateTime.Now,
                    ForumCategoryId = 2,
                    ForumUserId = 1
                }
            );
            #endregion
        }
    }
}