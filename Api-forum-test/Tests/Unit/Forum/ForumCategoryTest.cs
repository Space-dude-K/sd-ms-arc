using Entities.Models.Forum;
using FluentAssertions;
using Forum;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Xunit.Abstractions;
using ForumTest.Tests.Unit.Forum.TestCases;

namespace ForumTest.Tests.Unit.Forum
{
    public class ForumCategoryTest : TestWithSqlite
    {
        private readonly ITestOutputHelper _output;

        public ForumCategoryTest(ITestOutputHelper output)
        {
            _output = output;
        }
        [Fact]
        public void TableShouldGetCreated()
        {
            Assert.True(DbContext.ForumCategories.Any());
        }
        [Theory, ClassData(typeof(TestClassForForumCategoryData))]
        public void TableShouldContainSeedData<T>(ForumCategory forumCategory)
        {
            var dbData = DbContext.ForumCategories
                .Where(fc => fc.Id.Equals(forumCategory.Id)
                && fc.Name.Equals(forumCategory.Name)
                && fc.ForumUserId.Equals(forumCategory.ForumUserId)).FirstOrDefaultAsync().Result;

            dbData.Should()
                .Match<ForumCategory>((x) =>
                x.Id == forumCategory.Id
                && x.Name == forumCategory.Name
                && x.ForumUserId == forumCategory.ForumUserId);
        }

        [Fact]
        public void InsertTest_ForumCategoriesData_ReturnsTestCaseData()
        {


            Assert.True(DbContext.ForumCategories.Any());
        }
    }
}