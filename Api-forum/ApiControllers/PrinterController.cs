using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    [Route("api/printers")]
    [ApiController]
    public class PrinterController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public PrinterController(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult GetForums()
        {
            var printerDevices = _repository.PrinterDevice.GetAllPrinterDevices(trackChanges: false);
            return Ok(printerDevices);
        }
    }
}
