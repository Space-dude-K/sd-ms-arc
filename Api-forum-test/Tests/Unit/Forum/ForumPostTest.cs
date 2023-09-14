using Entities.Models.Forum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using ForumTest.Tests.Unit.Forum.TestCases;

namespace ForumTest.Tests.Unit.Forum
{
    public class ForumPostTest : TestWithSqlite
    {
        private readonly ITestOutputHelper output;

        public ForumPostTest(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void TableShouldGetCreated()
        {
            Assert.True(DbContext.ForumPosts.Any());
        }
        [Theory, ClassData(typeof(TestClassForForumPostData))]
        public void TableShouldContainSeedData<T>(ForumPost forumPost)
        {
            var dbData = DbContext.ForumPosts
                .Where(fc => fc.Id.Equals(forumPost.Id)
                && fc.ForumTopicId.Equals(forumPost.ForumTopicId)
                ).FirstOrDefaultAsync().Result;

            dbData.Should()
                .Match<ForumPost>((x) =>
                x.Id == forumPost.Id
                && x.ForumTopicId == forumPost.ForumTopicId
                && x.ForumUserId == forumPost.ForumUserId);
        }
    }
}