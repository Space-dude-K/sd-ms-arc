using Api_pdc_Entities;
using Api_pdc_Entities.PrintDevice;
using Mongo.Migration.Migrations.Database;
using Mongo.Migration.Migrations.Document;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Api_pdc.Migrations
{
    public class MyTestMigration : DatabaseMigration
    {

        public MyTestMigration()
        : base("0.0.1")
        {
        }

        public override void Up(IMongoDatabase db)
        {
            var collection = db.GetCollection<Printer>("Printer");
            collection.InsertOne(new Printer
            {
                Ip = "1.1.1.1",
                DeviceType = "TestType1"
            });
        }

        public override void Down(IMongoDatabase db)
        {
            var collection = db.GetCollection<Printer>("Printer");
            collection.DeleteOne(Builders<Printer>.Filter.Eq(c => c.Id, "AddedInMigration"));
        }
    }
}
