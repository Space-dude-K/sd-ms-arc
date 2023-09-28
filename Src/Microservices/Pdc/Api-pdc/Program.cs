using Api_pdc.Settings;
using Api_pdc_Interfaces;
using Api_pdc_Interfaces.MongoDbRepository;
using Api_pdc_Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Api_pdc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // MongoDb
            builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
            builder.Services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

            builder.Services.AddControllers();

            var app = builder.Build();

            /*app.MapGet("/", async (MongoClient client) =>     // получаем MongoClient через DI
            {
                var db = client.GetDatabase("test");    // обращаемся к базе данных
                var collection = db.GetCollection<BsonDocument>("users"); // получаем коллекцию users
                                                                          // для теста добавляем начальные данные, если коллекция пуста
                if (await collection.CountDocumentsAsync("{}") == 0)
                {
                    await collection.InsertManyAsync(new List<BsonDocument>
                    {
                        new BsonDocument{ { "Name", "Tom" },{"Age", 22}},
                        new BsonDocument{ { "Name", "Bob" },{"Age", 42}}
                    });
                }
                var users = await collection.Find("{}").ToListAsync();
                return users.ToJson();  // отправляем клиенту все документы из коллекции
            });*/



            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}