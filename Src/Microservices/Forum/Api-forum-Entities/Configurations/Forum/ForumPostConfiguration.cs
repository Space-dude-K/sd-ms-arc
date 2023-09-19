using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models.Forum;

namespace Entities.Configuration.Forum
{
    public class ForumPostConfiguration : IEntityTypeConfiguration<ForumPost>
    {
        public void Configure(EntityTypeBuilder<ForumPost> builder)
        {
            #region DbStructure
            builder
                .ToTable("ForumPost");

            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.ForumUserId)
                .HasColumnType("INTEGER");
            builder
                .Property(p => p.PostText)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(false)
                .IsUnicode(true);
            builder
                .Property(p => p.Likes)
                .HasColumnType("INTEGER")
                .IsRequired(false);
            builder
                .Property(p => p.CreatedAt)
                .HasColumnType("Date")
                .IsRequired(false);
            builder
                .Property(p => p.UpdatedAt)
                .HasColumnType("Date")
                .IsRequired(false);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumPost");
            builder
                .HasOne(p => p.ForumUser)
                .WithMany()
                .HasConstraintName("FK_ForumPost_ForumUser_Id")
                .OnDelete(DeleteBehavior.SetNull);
            builder
                .HasMany(p => p.ForumFiles)
                .WithOne(p => p.ForumPost)
                .HasConstraintName("FK_ForumPost_ForumFile_Id")
                .OnDelete(DeleteBehavior.SetNull);
            #endregion
            #region DbDataSeed
            builder.HasData(
                new ForumPost()
                {
                    Id = 1,
                    PostText = "1111111111111111111111",
                    CreatedAt = DateTime.Now,
                    ForumTopicId = 1,
                    ForumUserId = 1
                },
                new ForumPost()
                {
                    Id = 2,
                    CreatedAt = DateTime.Now,
                    ForumTopicId = 2,
                    ForumUserId = 1
                },
                new ForumPost()
                {
                    Id = 3,
                    CreatedAt = DateTime.Now,
                    ForumTopicId = 2,
                    ForumUserId = 1
                },
                new ForumPost()
                {
                    Id = 4,
                    CreatedAt = DateTime.Now,
                    ForumTopicId = 2,
                    ForumUserId = 1
                },
                new ForumPost()
                {
                    Id = 5,
                    CreatedAt = DateTime.Now,
                    ForumTopicId = 2,
                    ForumUserId = 1
                },
                new ForumPost()
                {
                    Id = 6,
                    PostText = "222222222222222222",
                    CreatedAt = DateTime.Now,
                    ForumTopicId = 1,
                    ForumUserId = 1
                },
                new ForumPost()
                {
                    Id = 7,
                    PostText = "333333333333333",
                    CreatedAt = DateTime.Now,
                    ForumTopicId = 1,
                    ForumUserId = 1
                },
                new ForumPost()
                {
                    Id = 8,
                    PostText = "44444444444444",
                    CreatedAt = DateTime.Now,
                    ForumTopicId = 1,
                    ForumUserId = 1
                },
                new ForumPost()
                {
                    Id = 9,
                    PostText = "555555555555555",
                    CreatedAt = DateTime.Now,
                    ForumTopicId = 1,
                    ForumUserId = 1
                },
                new ForumPost()
                {
                    Id = 10,
                    PostText = "666666666666666",
                    CreatedAt = DateTime.Now,
                    ForumTopicId = 1,
                    ForumUserId = 1
                }
            );
            #endregion
        }
    }
}