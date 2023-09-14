using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto;
using Entities;
using ForumTest.Tests.Integration.Forum.TestCases;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ForumTest.Tests.Integration.Forum.Category
{
    public partial class ForumCategoryWebApiTestGet
    {
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.PostSingleForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task PostSingle_ForumCategory_ReturnsCaseData(string uri, string expectedCategoryName)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().HttpClient;
            var jsonContent = JsonConvert.SerializeObject(new { Name = expectedCategoryName });

            // Act
            var response = await client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<ForumCategoryDto>(rawData);

            Assert.Equal(expectedCategoryName, responseContent.Name);
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.PostSingleForumCategoryDataErrors), MemberType = typeof(ForumCategoryCaseData))]
        public async Task PostSingle_ForumCategory_ReturnsUprocessableEntity(string uri, string expectedError)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().HttpClient;
            var jsonContent = JsonConvert.SerializeObject(new { });

            // Act
            var response = await client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            string body = string.Empty;
            using (var reader = new StreamReader(response.Content.ReadAsStream()))
            {
                body = await reader.ReadToEndAsync();
            }

            _output.WriteLine("Body -> " + body);

            // Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal(expectedError, body);
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.PostCollectionForumCategoryDataErrors), MemberType = typeof(ForumCategoryCaseData))]
        public async Task PostCollection_ForumCategory_ReturnsUprocessableEntities(string uri,
            List<ForumCategoryForCreationDto> expectedCategoryNames, string expectedError)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().HttpClient;
            var jsonContent = JsonConvert.SerializeObject(expectedCategoryNames);
            expectedError = "{" + string.Join(",", expectedCategoryNames.Select((e, i) => expectedError
            .Insert(1, "[" + i.ToString() + "]."))) + "}";

            _output.WriteLine("Json -> " + jsonContent);

            // Act
            var response = await client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            string body = string.Empty;
            using (var reader = new StreamReader(response.Content.ReadAsStream()))
            {
                body = await reader.ReadToEndAsync();
            }

            _output.WriteLine("body -> " + body);

            // Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal(expectedError, body);
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.PostCollectionForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task PostCollection_ForumCategory_ReturnsCaseData(string uri, List<ForumCategoryForCreationDto> expectedCategoryNames)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().HttpClient;
            var jsonContent = JsonConvert.SerializeObject(expectedCategoryNames);

            _output.WriteLine("Json -> " + jsonContent);

            // Act
            var response = await client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData);

            for (int i = 0; i < responseContent.Count(); i++)
            {
                Assert.Equal(expectedCategoryNames[i].Name, responseContent.ToArray()[i].Name);
            }

            Assert.Equal(expectedCategoryNames.Count(), responseContent.Count());
        }
    }
}