using Entities.LinksModels;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    [ApiVersion("2.0", Deprecated = true)]
    [Route("api")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class RootV2Controller : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;
        public RootV2Controller(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
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
                        Rel = "categories_V2",
                        Method = "GET"
                    },
                    new Link
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, "CreateCategory", new {}),
                        Rel = "create_category_V2",
                        Method = "POST"
                    }
                };

                return Ok(list);
            }

            return NoContent();
        }
    }
}