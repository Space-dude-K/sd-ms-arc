using Entities.Models.Forum;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration.Forum
{
    public class ForumTopicCounterConfiguration : IEntityTypeConfiguration<ForumTopicCounter>
    {
        public void Configure(EntityTypeBuilder<ForumTopicCounter> builder)
        {
            #region DbStructure
            builder
                .ToTable("ForumTopicCounter");

            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.ForumTopicId)
                .HasColumnType("INTEGER")
                .IsRequired(false);
            builder
                .Property(p => p.PostCounter)
                .HasColumnType("INTEGER")
                .IsRequired(false);
            
            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumTopicCounter");

            #endregion
            #region DbDataSeed
            builder.HasData(
                new ForumTopicCounter()
                {
                    Id = 1,
                    ForumTopicId = 1,
                    PostCounter = 6
                },
                new ForumTopicCounter()
                {
                    Id = 2,
                    ForumTopicId = 2,
                    PostCounter = 4
                },
                new ForumTopicCounter()
                {
                    Id = 3,
                    ForumTopicId = 3
                },
                new ForumTopicCounter()
                {
                    Id = 4,
                    ForumTopicId = 4
                },
                new ForumTopicCounter()
                {
                    Id = 5,
                    ForumTopicId = 5
                },
                new ForumTopicCounter()
                {
                    Id = 6,
                    ForumTopicId = 6
                },
                new ForumTopicCounter()
                {
                    Id = 7,
                    ForumTopicId = 7
                },
                new ForumTopicCounter()
                {
                    Id = 8,
                    ForumTopicId = 8
                }
            );
            #endregion
        }
    }
}