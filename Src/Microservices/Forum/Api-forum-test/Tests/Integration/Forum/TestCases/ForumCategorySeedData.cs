using Entities;
using Entities.Models.Forum;
using ForumTest.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections;

namespace ForumTest.Tests.Integration.Forum.TestCases
{
    public class ForumCategorySeedData : TestWithEfInMemoryDb<ForumContext>, IEnumerable<object[]>
    {
        private IEnumerable<object[]> data;
        public ForumCategorySeedData()
        {
            data = Context.GetService<IDesignTimeModel>().Model.GetPopulatedModelWithSeedDataFromConfigForTestCase<ForumCategory>();
        }
        public IEnumerator<object[]> GetEnumerator()
        { return data.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }
}