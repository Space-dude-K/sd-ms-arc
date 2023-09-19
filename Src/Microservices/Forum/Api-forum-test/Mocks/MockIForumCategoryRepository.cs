using Interfaces.Forum;
using Entities.Models.Forum;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Forum.API;

namespace ForumTest.Mocks
{
    internal class MockIForumCategoryRepository
    {
        public static Mock<IForumCategoryRepository> GetMock()
        {
            var mock = new Mock<IForumCategoryRepository>();
            var categories = new List<ForumCategory>()
            {
                new ForumCategory()
                {
                    Id = 1,
                    Name = "TestCategoryName1",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new ForumCategory()
                {
                    Id = 2,
                    Name = "TestCategoryName2",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                }
            };

            // Set up
            /*mock.Setup(m => m.GetAllCategoriesAsync(false))
                .Returns(() => categories);*/


            return mock;
        }
    }
}
