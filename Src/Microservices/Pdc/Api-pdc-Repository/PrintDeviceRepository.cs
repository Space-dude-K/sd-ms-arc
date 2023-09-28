using Api_pdc_Entities.PrintDevice;
using Api_pdc_Interfaces;
using Api_pdc_Interfaces.MongoDbRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_pdc_Repository
{
    public class PrintDeviceRepository : BaseRepository<Printer>, IPrintDeviceRepository
    {
        public PrintDeviceRepository(IPdcContext context) : base(context)
        {
        }
    }
}
