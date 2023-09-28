using Api_pdc_Interfaces;

namespace Api_pdc.Settings
{
    public class MongoDbSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        //public string PrintDevicesCollectionName { get; set; } = null!;
    }
}