using Interfaces;
using Entities.DTO.ForumDto;
using Entities.LinksModels;
using Entities.Models;
using Microsoft.Net.Http.Headers;

namespace Forum.Utility.ForumLinks
{
    public class TopicLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<ForumTopicDto> _dataShaper;
        public TopicLinks(LinkGenerator linkGenerator, IDataShaper<ForumTopicDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }
        public LinkResponse TryGenerateLinks(IEnumerable<ForumTopicDto> topicsDto, 
            int forumCategoryId, int forumBaseId, string fields, HttpContext httpContext,
            IEnumerable<int>? collectionIds = null)
        {
            var shapedTopics = ShapeData(topicsDto, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedTopics(topicsDto, forumCategoryId, forumBaseId, fields, httpContext, shapedTopics, collectionIds);

            return ReturnShapedTopics(shapedTopics);
        }
        private List<Entity> ShapeData(IEnumerable<ForumTopicDto> topicsDto, string fields)
        {
            return _dataShaper.ShapeData(topicsDto, fields)
             .Select(e => e.Entity)
             .ToList();
        }
        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }
        private LinkResponse ReturnShapedTopics(List<Entity> shapedTopics)
        {
            return new LinkResponse { ShapedEntities = shapedTopics };
        }
        private LinkResponse ReturnLinkdedTopics(IEnumerable<ForumTopicDto> topicsDto, 
            int forumCategoryId, int forumBaseId, 
            string fields, HttpContext httpContext,
            List<Entity> shapedTopics,
            IEnumerable<int>? collectionIds = null)
        {
            var topicsDtoList = topicsDto.ToList();

            for (var index = 0; index < topicsDtoList.Count(); index++)
            {
                var topicLinks = CreateLinksForTopic(httpContext, forumCategoryId, forumBaseId, topicsDtoList[index].Id, fields);
                shapedTopics[index].Add("Links", topicLinks);
            }

            var topicCollection = new LinkCollectionWrapper<Entity>(shapedTopics);
            var linkedTopics = CreateLinksForTopics(httpContext, topicCollection, forumCategoryId, forumBaseId, collectionIds);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedTopics };
        }
        private List<Link> CreateLinksForTopic(HttpContext httpContext, 
            int categoryId, int forumId, int topicId, string fields = "")
        {
            var links = new List<Link>
            {
                 new Link(_linkGenerator.GetUriByAction(httpContext, 
                 "GetTopicForForum", values: new { categoryId, forumId, topicId, fields }), "self", "GET"),

                 new Link(_linkGenerator.GetUriByAction(httpContext, 
                 "UpdateTopicForForum", values: new { categoryId, forumId, topicId }), "update_topic", "PUT"),

                 new Link(_linkGenerator.GetUriByAction(httpContext, 
                 "PartiallyUpdateTopicForForum", values: new { categoryId, forumId, topicId }), "partially_update_topic", "PATCH"),

                 new Link(_linkGenerator.GetUriByAction(httpContext, 
                 "DeleteTopicForForum", values: new { categoryId, forumId, topicId }), "delete_topic", "DELETE"),
             };

            return links;
        }
        private LinkCollectionWrapper<Entity> CreateLinksForTopics(HttpContext httpContext, LinkCollectionWrapper<Entity> topicsWrapper, 
            int forumCategoryId, int forumBaseId,
            IEnumerable<int>? collectionIds = null)
        {
            if (collectionIds == null)
            {
                topicsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, 
                    "GetTopicsForForum", values: new { forumCategoryId, forumBaseId }), "self", "GET"));
            }
            else
            {
                string ids = string.Join(",", collectionIds);
                topicsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetTopicCollection", 
                    values: new { forumCategoryId, forumBaseId, ids }), "self", "GET"));
            }

            return topicsWrapper;
        }
    }
}