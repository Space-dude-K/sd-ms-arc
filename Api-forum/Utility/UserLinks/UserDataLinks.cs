using Entities.DTO.UserDto;
using Entities.LinksModels;
using Entities.Models;
using Interfaces;
using Microsoft.Net.Http.Headers;

namespace Forum.Utility.UserLinks
{
    public class UserDataLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<ForumUserDto> _dataShaper;
        public UserDataLinks(LinkGenerator linkGenerator, IDataShaper<ForumUserDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }
        public LinkResponse TryGenerateLinks(IEnumerable<ForumUserDto> usersDto, string fields, HttpContext httpContext)
        {
            var shapedUsers = ShapeData(usersDto, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedUsers(usersDto, fields, httpContext, shapedUsers);

            return ReturnShapedUsers(shapedUsers);
        }
        private List<Entity> ShapeData(IEnumerable<ForumUserDto> usersDto, string fields)
        {
            return _dataShaper.ShapeData(usersDto, fields)
             .Select(e => e.Entity)
             .ToList();
        }
        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }
        private LinkResponse ReturnShapedUsers(List<Entity> shapedUsers)
        {
            return new LinkResponse { ShapedEntities = shapedUsers };
        }
        private LinkResponse ReturnLinkdedUsers(IEnumerable<ForumUserDto> usersDto, string fields, HttpContext httpContext, List<Entity> shapedUsers)
        {
            var usersDtoList = usersDto.ToList();

            for (var index = 0; index < usersDtoList.Count(); index++)
            {
                var userLinks = CreateLinksForUser(httpContext, usersDtoList[index].Id, fields);
                shapedUsers[index].Add("Links", userLinks);
            }

            var userCollection = new LinkCollectionWrapper<Entity>(shapedUsers);
            var linkedUsers = CreateLinksForUsers(httpContext, userCollection);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedUsers };
        }
        private List<Link> CreateLinksForUser(HttpContext httpContext, int userId, string fields = "")
        {
            var links = new List<Link>
            {
                 new Link(_linkGenerator.GetUriByAction(httpContext, "GetUser", values: new { userId, fields }), "self", "GET"),
             };

            return links;
        }
        private LinkCollectionWrapper<Entity> CreateLinksForUsers(HttpContext httpContext, LinkCollectionWrapper<Entity> usersWrapper)
        {
            usersWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetUsers", values: new { }), "self", "GET"));

            return usersWrapper;
        }
    }
}