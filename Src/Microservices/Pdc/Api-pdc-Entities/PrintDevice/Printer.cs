using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Mongo.Migration.Documents.Attributes;
using Mongo.Migration.Documents;

namespace Api_pdc_Entities.PrintDevice
{
    [RuntimeVersion("0.1.1")]
    [StartUpVersion("0.0.1")]
    [CollectionLocation("Printer", "TestPrinters")]
    public class Printer : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Ip { get; set; }
        public string? DeviceType { get; set; }
        public DocumentVersion Version { get; set; }
    }
}