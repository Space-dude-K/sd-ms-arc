using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Api_pdc_Entities.PrintDevice
{
    public class Printer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Ip { get; set; }
        public string? DeviceType { get; set; }
        public int? NumberOfPages { get; set; }
        public int? DrumLevel { get; set; }
        public int? TonerLevel { get; set; }
    }
}