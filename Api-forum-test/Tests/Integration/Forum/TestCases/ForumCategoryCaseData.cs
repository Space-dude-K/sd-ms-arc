using Entities.DTO.ForumDto.Create;

namespace ForumTest.Tests.Integration.Forum.TestCases
{
    public class ForumCategoryCaseData
    {
        public static IEnumerable<object[]> GetAllCategoriesData =>
            new List<object[]>
            {
                new object[] { "/api/categories", "application/json; charset=utf-8" }
            };
        public static IEnumerable<object[]> GetSingleForumCategoryData =>
            new List<object[]>
            {
                new object[] { "/api/categories/1", "application/json; charset=utf-8" },
                new object[] { "/api/categories/2", "application/json; charset=utf-8" }
            };
        public static IEnumerable<object[]> GetCollectionForumCategoryData =>
            new List<object[]>
            {
                new object[] { "/api/categories/collection/(1,2)", "application/json; charset=utf-8", 2 },
                new object[] { "/api/categories/collection/(2,3)", "application/json; charset=utf-8", 2 },
                new object[] { "/api/categories/collection/(3,4)", "application/json; charset=utf-8", 2 }
            };
        public static IEnumerable<object[]> UpdateSingleForumCategoryData =>
            new List<object[]>
            {
                new object[] { "/api/categories/1", "Test category 1 updated" },
                new object[] { "/api/categories/2", "Test category 2 updated" },
            };
        public static IEnumerable<object[]> PostSingleForumCategoryData =>
            new List<object[]>
            {
                new object[] { "/api/categories", "Test category name 1" },
                new object[] { "/api/categories", "Test category name 2" },
            };
        public static IEnumerable<object[]> PostSingleForumCategoryDataErrors =>
            new List<object[]>
            {
                new object[] { "/api/categories", "{\"Name\":[\"Category title is a required field.\"]}" }
            };
        public static IEnumerable<object[]> PostCollectionForumCategoryDataErrors =>
            new List<object[]>
            {
                new object[]
                {
                    "/api/categories/collection",
                    new List<ForumCategoryForCreationDto>
                    {
                        new ForumCategoryForCreationDto() { },
                        new ForumCategoryForCreationDto() { }
                    },
                    "\"Name\":[\"Category title is a required field.\"]"
                },
                new object[]
                {
                    "/api/categories/collection",
                    new List<ForumCategoryForCreationDto>
                    {
                        new ForumCategoryForCreationDto() { Name = "Test name 11111111111111111111111111111111111" },
                        new ForumCategoryForCreationDto() { Name = "Test name 22222222222222222222222222222222222"}
                    },
                    "\"Name\":[\"Maximum length for the category name is 30 characters.\"]"
                }
            };
        public static IEnumerable<object[]> PostCollectionForumCategoryData =>
            new List<object[]>
            {
                new object[]
                {
                    "/api/categories/collection",
                    new List<ForumCategoryForCreationDto>
                    {
                        new ForumCategoryForCreationDto() { Name = "Test category name 1 c" },
                        new ForumCategoryForCreationDto() { Name = "Test category name 2 c" }
                    },
                },
                new object[]
                {
                    "/api/categories/collection",
                    new List<ForumCategoryForCreationDto>
                    {
                        new ForumCategoryForCreationDto() { Name = "Test category name 3 c" },
                        new ForumCategoryForCreationDto() { Name = "Test category name 4 c" }
                    }
                }  
            };
        public static IEnumerable<object[]> DeleteSingleForumCategoryData =>
            new List<object[]>
            {
                new object[] { "/api/categories/1" },
                new object[] { "/api/categories/2" },
            };
    }
}