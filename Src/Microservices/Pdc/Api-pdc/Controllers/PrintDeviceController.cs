using Api_pdc_Entities.PrintDevice;
using Api_pdc_Interfaces.MongoDbRepository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Api_pdc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrintDeviceController : Controller
    {
        private readonly ILogger<PrintDeviceController> _logger;
        private readonly IPrintDeviceRepository _printDeviceRepository;

        public PrintDeviceController(ILogger<PrintDeviceController> logger, IPrintDeviceRepository printDeviceRepository)
        {
            _logger = logger;
            _printDeviceRepository = printDeviceRepository;
        }
        [HttpGet]
        [Route("pd")]
        public async Task<ActionResult<IEnumerable<Printer>>> Get()
        {
            var printers = await _printDeviceRepository.Get();
            return Ok(printers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Printer>> Get(string id)
        {
            var product = await _printDeviceRepository.Get(id);
            return Ok(product);
        }
        /*public async Task<IActionResult> Index()
        {
            //var r = _printDeviceRepository.AsQueryable().ToList();
            var r = _printDeviceRepository.FindOneAsync(e => e.Id == new ObjectId("6515676f1f4718bd8b859c9a")).ToJson();

            return Ok(r);
        }*/
    }
}