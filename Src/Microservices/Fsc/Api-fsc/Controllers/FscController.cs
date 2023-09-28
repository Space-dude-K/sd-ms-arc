using Microsoft.AspNetCore.Mvc;

namespace Api_fsc.Controllers
{
    public class FscController : Controller
    {
        private readonly ILogger<FscController> _logger;

        public FscController(ILogger<FscController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {


            return View();
        }
    }
}
