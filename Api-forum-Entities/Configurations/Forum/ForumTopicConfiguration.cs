using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models.Forum;

namespace Entities.Configuration.Forum
{
    public class ForumTopicConfiguration : IEntityTypeConfiguration<ForumTopic>
    {
        public void Configure(EntityTypeBuilder<ForumTopic> builder)
        {
            #region DbStructure
            builder
                .ToTable("ForumTopic");

            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.ForumUserId)
                .HasColumnType("INTEGER");
            builder
                .Property(p => p.Name)
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
                .Property(p => p.TotalViews)
                .HasColumnType("INTEGER")
                .IsRequired(true);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumTopic");
            builder
                .HasMany(p => p.ForumPosts)
                .WithOne(p => p.ForumTopic)
                .HasForeignKey(p => p.ForumTopicId)
                .HasConstraintName("FK_ForumTopic_ForumPost_ForumTopicId")
                .OnDelete(DeleteBehavior.SetNull);
            builder
                .HasOne(p => p.ForumUser)
                .WithMany()
                .HasConstraintName("FK_ForumTopic_ForumUser_Id")
                .OnDelete(DeleteBehavior.SetNull);
            builder
                .HasOne(p => p.ForumTopicCounter)
                .WithOne(p => p.ForumTopic)
                .HasForeignKey<ForumTopicCounter>(k => k.ForumTopicId)
                .HasConstraintName("FK_ForumTopic_ForumTopicCounter_Id")
                .OnDelete(DeleteBehavior.SetNull); ;
            #endregion
            #region DbDataSeed
            builder.HasData(
                new ForumTopic()
                {
                    Id = 1,
                    Name = "Test forum topic 1",
                    CreatedAt = DateTime.Now,
                    ForumBaseId = 1,
                    ForumUserId = 1
                },
                new ForumTopic()
                {
                    Id = 2,
                    Name = "Test forum topic 2",
                    CreatedAt = DateTime.Now,
                    ForumBaseId = 2,
                    ForumUserId = 1
                },
                new ForumTopic()
                {
                    Id = 3,
                    Name = "Test forum topic 3",
                    CreatedAt = DateTime.Now,
                    ForumBaseId = 2,
                    ForumUserId = 1
                },
                new ForumTopic()
                {
                    Id = 4,
                    Name = "Test forum topic 4",
                    CreatedAt = DateTime.Now,
                    ForumBaseId = 2,
                    ForumUserId = 1
                },
                new ForumTopic()
                {
                    Id = 5,
                    Name = "Test forum topic 5",
                    CreatedAt = DateTime.Now,
                    ForumBaseId = 2,
                    ForumUserId = 1
                },
                new ForumTopic()
                {
                    Id = 6,
                    Name = "Test forum topic 1a",
                    CreatedAt = DateTime.Now,
                    ForumBaseId = 1,
                    ForumUserId = 1
                },
                new ForumTopic()
                {
                    Id = 7,
                    Name = "Test forum topic 1b",
                    CreatedAt = DateTime.Now,
                    ForumBaseId = 1,
                    ForumUserId = 1
                },
                new ForumTopic()
                {
                    Id = 8,
                    Name = "Test forum topic 1c",
                    CreatedAt = DateTime.Now,
                    ForumBaseId = 1,
                    ForumUserId = 1
                }
            );
            #endregion
        }
    }
}