using Entities.DTO.ForumDto;
using Entities;
using ForumTest.Tests.Integration.Forum.TestCases;
using Newtonsoft.Json;
using System.Text;

namespace ForumTest.Tests.Integration.Forum.Category
{
    public partial class ForumCategoryWebApiTestGet
    {
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.UpdateSingleForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task PutSingle_ForumCategory_ReturnsUpdatedCaseData(string uri, string expectedCategoryName)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().HttpClient;

            // Act
            var responseGetBeforeUp = await client.GetAsync(uri);
            var rawDataBeforeUp = await responseGetBeforeUp.Content.ReadAsStringAsync();
            var responseContentBeforeUp = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawDataBeforeUp);
            responseContentBeforeUp.First().Name = responseContentBeforeUp.First().Name + " updated";
            var jsonContentBeforeUp = JsonConvert.SerializeObject(responseContentBeforeUp.First());
            var response = await client.PutAsync(uri, new StringContent(jsonContentBeforeUp, Encoding.UTF8, "application/json"));
            var responseGetAfterUp = await client.GetAsync(uri);
            var rawDataAfterUp = await responseGetAfterUp.Content.ReadAsStringAsync();
            var responseContentAfterUp = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawDataAfterUp);

            // Assert
            responseGetBeforeUp.EnsureSuccessStatusCode(); // Status Code 200-299
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            responseGetAfterUp.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal(expectedCategoryName, responseContentAfterUp.First().Name);
        }
    }
}