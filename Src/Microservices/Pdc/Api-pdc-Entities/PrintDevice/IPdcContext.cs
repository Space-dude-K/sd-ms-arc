using Api_pdc_Entities.PrintDevice;
using MongoDB.Driver;

namespace Api_pdc_Interfaces
{
    public interface IPdcContext
    {
        IMongoCollection<Printer> GetCollection<Printer>(string name);
    }
}