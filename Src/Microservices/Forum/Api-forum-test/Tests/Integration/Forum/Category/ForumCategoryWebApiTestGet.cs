using Entities;
using Entities.DTO.ForumDto;
using Entities.Models.Forum;
using ForumTest.Extensions;
using ForumTest.Tests.Integration.Forum.TestCases;
using Newtonsoft.Json;
using System.Net;
using Xunit.Abstractions;

namespace ForumTest.Tests.Integration.Forum.Category
{
    public partial class ForumCategoryWebApiTestGet
    {
        private readonly ITestOutputHelper _output;

        public ForumCategoryWebApiTestGet(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetAllCategoriesData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetAll_ForumCategories_ReturnsCorrectContentType(string uri, string contentType)
        {
            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().HttpClient;

            // Act
            var response = await client.GetAsync(uri);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(contentType, response.Content.Headers.ContentType.ToString());
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetAllCategoriesData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetAll_ForumCategories_ReturnsSeedData(string uri, string contentType)
        {
            _output.WriteLine("uri -> " + uri + " Type: " + contentType);

            // Arrange
            var fac = new TestWithEfInMemoryDb<ForumContext>();
            var client = new TestWithEfInMemoryDb<ForumContext>().HttpClient;
            var seedData = fac.Model.GetPopulatedModelWithSeedDataFromConfig<ForumCategory>();

            // Act
            var response = await client.GetAsync(uri);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var rawData = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData);

            foreach (var fcDb in content)
            {
                var res = seedData.Any(fc => fc.Id.Equals(fcDb.Id) && fc.Name.Equals(fcDb.Name));
                Assert.True(res, $"Db data with Id: {fcDb.Id} are not equal to seed data");
            }
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetSingleForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetSingle_ForumCategory_ReturnsCorrectContentType(string uri, string contentType)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().HttpClient;

            // Act
            var response = await client.GetAsync(uri);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(contentType, response.Content.Headers.ContentType.ToString());
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetSingleForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetSingle_ForumCategory_ReturnsSeedData(string uri, string contentType)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var fac = new TestWithEfInMemoryDb<ForumContext>();
            var client = new TestWithEfInMemoryDb<ForumContext>().HttpClient;
            var seedData = fac.Model.GetPopulatedModelWithSeedDataFromConfig<ForumCategory>();

            // Act
            var response = await client.GetAsync(uri);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData);

            Assert.Equal(seedData.Single(fc => fc.Id.Equals(responseContent.First().Id)).Id, responseContent.First().Id);
            Assert.Equal(seedData.Single(fc => fc.Id.Equals(responseContent.First().Id)).Name, responseContent.First().Name);
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetCollectionForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetCollection_ForumCategory_ReturnsCorrectContentType(string uri, string contentType, int collectionSize)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().HttpClient;

            // Act
            var response = await client.GetAsync(uri);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(contentType, response.Content.Headers.ContentType.ToString());
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetCollectionForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetCollection_ForumCategory_ReturnsSeedData(string uri, string contentType, int collectionSize)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var fac = new TestWithEfInMemoryDb<ForumContext>();
            var client = new TestWithEfInMemoryDb<ForumContext>().HttpClient;
            var seedData = fac.Model.GetPopulatedModelWithSeedDataFromConfig<ForumCategory>();

            // Act
            var response = await client.GetAsync(uri);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData);

            List<ForumCategory> categories = new();

            foreach (var fcDb in responseContent)
            {
                categories.Add(seedData.Single(fc => fc.Id.Equals(fcDb.Id) && fc.Name.Equals(fcDb.Name)));
            }

            Assert.Equal(collectionSize, categories.Count);
        }
    }
}