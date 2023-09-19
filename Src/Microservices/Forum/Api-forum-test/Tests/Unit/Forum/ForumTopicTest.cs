using Entities.Models.Forum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using ForumTest.Tests.Unit.Forum.TestCases;

namespace ForumTest.Tests.Unit.Forum
{
    public class ForumTopicTest : TestWithSqlite
    {
        private readonly ITestOutputHelper output;

        public ForumTopicTest(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void TableShouldGetCreated()
        {
            Assert.True(DbContext.ForumTopics.Any());
        }
        [Theory, ClassData(typeof(TestClassForForumTopicData))]
        public void TableShouldContainSeedData<T>(ForumTopic forumTopic)
        {
            var dbData = DbContext.ForumTopics
                .Where(fc => fc.Id.Equals(forumTopic.Id)
                && fc.Name.Equals(forumTopic.Name)
                && fc.ForumUserId.Equals(forumTopic.ForumUserId)
                && fc.ForumBaseId.Equals(forumTopic.ForumBaseId)
                ).FirstOrDefaultAsync().Result;

            dbData.Should()
                .Match<ForumTopic>((x) =>
                x.Id == forumTopic.Id
                && x.Name == forumTopic.Name
                && x.ForumBaseId == forumTopic.ForumBaseId
                && x.ForumUserId == forumTopic.ForumUserId);
        }
    }
}