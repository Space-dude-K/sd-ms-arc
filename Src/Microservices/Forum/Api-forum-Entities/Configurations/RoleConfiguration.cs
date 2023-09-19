using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models;

namespace Entities.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasData(
            new AppRole
            {
                //Id = "1c5e174e-3b0e-446f-86af-483d56fd7210",
                Id = 1,
                Name = "USER",
                NormalizedName = "USER"
            },
            new AppRole
            {
                //Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                Id = 2,
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            }
            );
        }
    }
}