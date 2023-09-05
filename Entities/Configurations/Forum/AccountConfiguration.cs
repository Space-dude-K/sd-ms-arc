using Entities.Models.Forum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration.Forum
{
    public class AccountConfiguration : IEntityTypeConfiguration<ForumAccount>
    {
        public void Configure(EntityTypeBuilder<ForumAccount> builder)
        {
            builder
                .ToTable("ForumAccount");
            builder
                .Property(p => p.Id)
               .HasColumnType("INTEGER")
               .IsRequired(true);
            builder
                .Property(p => p.AccountTypeId)
                .HasColumnType("INTEGER");
            builder
                .Property(p => p.Ip)
                .HasColumnType("NVARCHAR")
                .IsRequired(false);
            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumAccount");
            builder
                .HasOne(p => p.ForumUser)
                .WithOne(p => p.ForumAccount)
                .HasForeignKey<ForumAccount>(p => p.Id)
                .HasConstraintName("FK_ForumAccount_ForumUser_Id");
            builder
                .HasOne(p => p.ForumAccountType)
                .WithOne(p => p.ForumAccount)
                .HasForeignKey<ForumAccount>(p => p.AccountTypeId)
                .HasConstraintName("FK_ForumAccount_ForumAccountType_Id")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}