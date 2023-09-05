using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models;

namespace Entities.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasData(
            SeedUsers(5)
            );

            //builder.OwnsOne(o => o.ForumUser).HasData(new Models.Forum.ForumUser() { Id = 1 });

            //8e445865-a24d-4543-a6c6-9443d048cdb9
            
        }
        private List<AppUser> SeedUsers(int usersCount)
        {
            var users = new List<AppUser>();

            for(int i = 0; i < usersCount; i++)
            {
                AppUser appUser = new()
                {
                    Id = i + 2,
                    FirstName = "Ivan" + i,
                    LastName = "Petrov" + i,
                    Cabinet = Random.Shared.Next(999).ToString(),
                    InternalPhone = Random.Shared.Next().ToString(),
                    BirthDate = RandomDay().ToString(),
                    UserName = "G600-U" + i,
                    NormalizedUserName = ("G600-U" + i).ToUpper(),
                    Email = "G600-U" + i + "@mfrb.by",
                    NormalizedEmail = ("G600-U" + i + "@mfrb.by").ToUpper(),
                    PhoneNumber = Random.Shared.Next().ToString() + Random.Shared.Next().ToString() + Random.Shared.Next().ToString(),
                    ForumUser = new() { Id = i + 1 }
                };

                //users.Add(appUser);
            }

            users.Add(SeedSystemAdmin());

            return users;
        }
        // Admin - 1234567890
        private AppUser SeedSystemAdmin()
        {
            AppUser appUser = new()
            {
                //Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                Id = 1,
                FirstName = "System",
                LastName = "Admin",
                CreatedAt = DateTime.Now,
                Cabinet = 0.ToString(),
                InternalPhone = 0.ToString(),
                BirthDate = 0.ToString(),
                Position = "Administrator",
                Company = "My company",
                Division = "My division",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "Admin@admin.by",
                NormalizedEmail = "ADMIN@ADMIN.BY",
                PhoneNumber = 0.ToString(),
                PasswordHash = "AQAAAAIAAYagAAAAEJmQy9UwxkODjbb/iQlo7ezznBC5omr0sEhFEoTgafpAxZZRFsyVCFG8NXKSc2SGJA==",
                SecurityStamp = "FGXU4FIM2LMJZFDJD3YCUQEHQRZY4GSS"
            };

            return appUser;
        }

        private Random gen = new Random();
        DateTime RandomDay()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
    }
}