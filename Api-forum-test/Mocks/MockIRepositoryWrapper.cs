using Interfaces;
using Interfaces.Forum;
using Interfaces.Forum.API;
using Moq;
using Repository.API.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumTest.Mocks
{
    internal class MockRepositoryWrapper
    {
        public static Mock<IRepositoryManager> GetMock()
        {
            var mock = new Mock<IRepositoryManager>();

            mock.Setup(m => m.ForumCategory).Returns(() => new Mock<IForumCategoryRepository>().Object);
            mock.Setup(m => m.ForumBase).Returns(() => new Mock<IForumBaseRepository>().Object);
            mock.Setup(m => m.ForumTopic).Returns(() => new Mock<IForumTopicRepository>().Object);
            mock.Setup(m => m.ForumPost).Returns(() => new Mock<IForumPostRepository>().Object);

            mock.Setup(m => m.SaveAsync()).Callback(() => { return; });

            // Setup the mock
            return mock;
        }
    }
}
