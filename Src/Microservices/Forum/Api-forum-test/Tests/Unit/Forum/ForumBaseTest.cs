using Entities.Models.Forum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using ForumTest.Tests.Unit.Forum.TestCases;

namespace ForumTest.Tests.Unit.Forum
{
    public class ForumBaseTest : TestWithSqlite
    {
        private readonly ITestOutputHelper output;

        public ForumBaseTest(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void TableShouldGetCreated()
        {
            Assert.True(DbContext.ForumBases.Any());
        }
        [Theory, ClassData(typeof(TestClassForForumBaseData))]
        public void TableShouldContainSeedData<T>(ForumBase forumBase)
        {
            var dbData = DbContext.ForumBases
                .Where(fc => fc.Id.Equals(forumBase.Id)
                && fc.ForumTitle.Equals(forumBase.ForumTitle)
                && fc.ForumSubTitle.Equals(forumBase.ForumSubTitle)
                && fc.ForumUserId.Equals(forumBase.ForumUserId)
                && fc.ForumCategoryId.Equals(forumBase.ForumCategoryId)
                ).FirstOrDefaultAsync().Result;

            dbData.Should()
                .Match<ForumBase>((x) =>
                x.Id == forumBase.Id
                && x.ForumTitle == forumBase.ForumTitle
                && x.ForumSubTitle == forumBase.ForumSubTitle
                && x.ForumCategoryId == forumBase.ForumCategoryId
                && x.ForumUserId == forumBase.ForumUserId);
        }
    }
}