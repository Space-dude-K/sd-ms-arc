using Entities;
using ForumTest.Tests.Integration.Forum.TestCases;
using System.Net;

namespace ForumTest.Tests.Integration.Forum.Category
{
    public partial class ForumCategoryWebApiTestGet
    {
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.DeleteSingleForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task DeleteSingle_ForumCategory_ReturnsNotFound(string uri)
        {
            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().HttpClient;

            // Act
            var responseGetBeforeDel = await client.GetAsync(uri);
            var responseDelete = await client.DeleteAsync(uri);
            var responseGetAfterDel = await client.GetAsync(uri);

            // Assert
            responseGetBeforeDel.EnsureSuccessStatusCode(); // Status Code 200-299
            responseDelete.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal(HttpStatusCode.OK, responseGetBeforeDel.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, responseGetAfterDel.StatusCode);
        }
    }
}