using Entities.Models.Forum;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ForumTest.Extensions;

namespace ForumTest.Tests.Unit.Forum.TestCases
{
    public class TestClassForForumCategoryData : TestWithSqlite, IEnumerable<object[]>
    {
        private IEnumerable<object[]> data;

        public TestClassForForumCategoryData()
        {
            data = DbContext.GetService<IDesignTimeModel>().Model.GetPopulatedModelWithSeedDataFromConfigForTestCase<ForumCategory>();
        }
        public IEnumerator<object[]> GetEnumerator()
        { return data.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }
}