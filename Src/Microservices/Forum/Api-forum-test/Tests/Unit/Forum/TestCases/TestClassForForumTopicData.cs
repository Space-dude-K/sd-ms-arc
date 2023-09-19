using Entities.Models.Forum;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections;
using ForumTest.Extensions;

namespace ForumTest.Tests.Unit.Forum.TestCases
{
    public class TestClassForForumTopicData : TestWithSqlite, IEnumerable<object[]>
    {
        private IEnumerable<object[]> data;

        public TestClassForForumTopicData()
        {
            data = DbContext.GetService<IDesignTimeModel>().Model.GetPopulatedModelWithSeedDataFromConfigForTestCase<ForumTopic>();
        }
        public IEnumerator<object[]> GetEnumerator()
        { return data.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }
}