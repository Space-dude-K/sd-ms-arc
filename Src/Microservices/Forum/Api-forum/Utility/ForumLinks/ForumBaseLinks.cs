using Interfaces;
using Entities.DTO.ForumDto;
using Entities.LinksModels;
using Entities.Models;
using Microsoft.Net.Http.Headers;

namespace Forum.Utility.ForumLinks
{
    public class ForumBaseLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<ForumBaseDto> _dataShaper;
        public ForumBaseLinks(LinkGenerator linkGenerator, IDataShaper<ForumBaseDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }
        public LinkResponse TryGenerateLinks(IEnumerable<ForumBaseDto> forumsDto, int forumCategoryId, string fields, HttpContext httpContext, 
            IEnumerable<int>? collectionIds = null)
        {
            var shapedForums = ShapeData(forumsDto, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedForums(forumsDto, forumCategoryId, fields, httpContext, shapedForums, collectionIds);

            return ReturnShapedForums(shapedForums);
        }
        private List<Entity> ShapeData(IEnumerable<ForumBaseDto> forumsDto, string fields)
        {
            return _dataShaper.ShapeData(forumsDto, fields)
             .Select(e => e.Entity)
             .ToList();
        }
        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }
        private LinkResponse ReturnShapedForums(List<Entity> shapedForums)
        {
            return new LinkResponse { ShapedEntities = shapedForums };
        }
        private LinkResponse ReturnLinkdedForums(IEnumerable<ForumBaseDto> forumsDto, int forumCategoryId, string fields, HttpContext httpContext, 
            List<Entity> shapedForums,
            IEnumerable<int>? collectionIds = null)
        {
            var forumsDtoList = forumsDto.ToList();

            for (var index = 0; index < forumsDtoList.Count(); index++)
            {
                var forumLinks = CreateLinksForForum(httpContext, forumCategoryId, forumsDtoList[index].Id, fields);
                shapedForums[index].Add("Links", forumLinks);
            }

            var forumCollection = new LinkCollectionWrapper<Entity>(shapedForums);
            var linkedForums = CreateLinksForForums(httpContext, forumCollection, forumCategoryId, collectionIds);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedForums };
        }
        private List<Link> CreateLinksForForum(HttpContext httpContext, int categoryId, int forumId, string fields = "")
        {
            var links = new List<Link>
            {
                 new Link(_linkGenerator.GetUriByAction(httpContext, "GetForumForCategory", values: new { categoryId, forumId, fields }), "self", "GET"),
                 new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateForumForCategory", values: new { categoryId, forumId }), "update_forum", "PUT"),
                 new Link(_linkGenerator.GetUriByAction(httpContext, "PartiallyUpdateForumForCategory", values: new { categoryId, forumId }), "partially_update_forum", "PATCH"),
                 new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteForumForCategory", values: new { categoryId, forumId }), "delete_forum", "DELETE"),
             };

            return links;
        }
        private LinkCollectionWrapper<Entity> CreateLinksForForums(HttpContext httpContext, LinkCollectionWrapper<Entity> forumsWrapper, int categoryId,
            IEnumerable<int>? collectionIds = null)
        {
            if (collectionIds == null)
            {
                forumsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetForumsForCategory", values: new { categoryId }), "self", "GET"));
            }
            else
            {
                string ids = string.Join(",", collectionIds);
                forumsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetForumCollection", values: new { categoryId, ids }), "self", "GET"));
            }

            return forumsWrapper;
        }
    }
}