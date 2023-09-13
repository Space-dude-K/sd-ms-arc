using Entities.LinksModels;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    [ApiVersion("1.0")]
    [Route("api")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class RootController : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly ILogger<RootController> _logger;

        public RootController(LinkGenerator linkGenerator, ILogger<RootController> logger)
        {
            _linkGenerator = linkGenerator;
            _logger = logger;
        }
        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            if (mediaType.Contains("application/sd.k.apiroot"))
            {
                var list = new List<Link>
                {
                    new Link
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), new {}),
                        Rel = "self",
                        Method = "GET"
                    },
                    new Link
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, "GetCategories", new {}),
                        Rel = "categories",
                        Method = "GET"
                    },
                    new Link
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, "CreateCategory", new {}),
                        Rel = "create_category",
                        Method = "POST"
                    }
                };

                return Ok(list);
            }

            return NoContent();
        }
    }
}