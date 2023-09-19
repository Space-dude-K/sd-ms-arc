using Microsoft.EntityFrameworkCore.Metadata;
using ForumTest.Helpers;

namespace ForumTest.Extensions
{
    public static class TestExtensions
    {
        public static List<object[]> GetPopulatedModelWithSeedDataFromConfigForTestCase<T>(this IModel model)
        {
            List<object[]> result = new();

            foreach (var prop in model.GetEntityTypes().Where(p => p.ClrType.Equals(typeof(T))))
            {
                var seedData = prop.GetSeedData(true);

                foreach (var dicts in seedData)
                {
                    var typeInstance = Activator.CreateInstance(prop.ClrType);

                    foreach (var kvp in dicts)
                    {
                        TestHelper.SetObjectProperty(kvp.Key, kvp.Value, typeInstance);
                    }

                    result.Add(new object[] { (T)typeInstance });
                }
            }

            return result;
        }
        public static List<T> GetPopulatedModelWithSeedDataFromConfig<T>(this IModel model)
        {
            List<T> result = new();

            foreach (var prop in model.GetEntityTypes().Where(p => p.ClrType.Equals(typeof(T))))
            {
                var seedData = prop.GetSeedData(true);

                foreach (var dicts in seedData)
                {
                    T typeInstance = (T)Activator.CreateInstance(prop.ClrType);

                    foreach (var kvp in dicts)
                    {
                        TestHelper.SetObjectProperty(kvp.Key, kvp.Value, typeInstance);
                    }

                    result.Add(typeInstance);
                }
            }

            return result;
        }
    }
}