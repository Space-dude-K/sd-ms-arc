using Interfaces;
using Entities.DTO.ForumDto;
using Entities.LinksModels;
using Entities.Models;
using Microsoft.Net.Http.Headers;

namespace Forum.Utility.ForumLinks
{
    public class PostLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<ForumPostDto> _dataShaper;
        public PostLinks(LinkGenerator linkGenerator, IDataShaper<ForumPostDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }
        public LinkResponse TryGenerateLinks(IEnumerable<ForumPostDto> postsDto, 
            int forumCategoryId, int forumBaseId, int forumTopicId, 
            string fields, HttpContext httpContext,
            IEnumerable<int>? collectionIds = null)
        {
            var shapedPosts = ShapeData(postsDto, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedPosts(postsDto, forumCategoryId, forumBaseId, forumTopicId, fields, httpContext, shapedPosts, collectionIds);

            return ReturnShapedPosts(shapedPosts);
        }
        private List<Entity> ShapeData(IEnumerable<ForumPostDto> postsDto, string fields)
        {
            return _dataShaper.ShapeData(postsDto, fields)
             .Select(e => e.Entity)
             .ToList();
        }
        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }
        private LinkResponse ReturnShapedPosts(List<Entity> shapedPosts)
        {
            return new LinkResponse { ShapedEntities = shapedPosts };
        }
        private LinkResponse ReturnLinkdedPosts(IEnumerable<ForumPostDto> postsDto,
            int forumCategoryId, int forumBaseId, int forumTopicId,
            string fields, HttpContext httpContext,
            List<Entity> shapedPosts,
            IEnumerable<int>? collectionIds = null)
        {
            var postsDtoList = postsDto.ToList();

            for (var index = 0; index < postsDtoList.Count(); index++)
            {
                var topicLinks = CreateLinksForPost(httpContext, forumCategoryId, forumBaseId, forumTopicId, postsDtoList[index].Id, fields);
                shapedPosts[index].Add("Links", topicLinks);
            }

            var postCollection = new LinkCollectionWrapper<Entity>(shapedPosts);
            var linkedPosts = CreateLinksForPosts(httpContext, postCollection, forumCategoryId, forumBaseId, forumTopicId, collectionIds);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedPosts };
        }
        private List<Link> CreateLinksForPost(HttpContext httpContext,
            int categoryId, int forumId, int topicId, int postId, 
            string fields = "")
        {
            var links = new List<Link>
            {
                 new Link(_linkGenerator.GetUriByAction(httpContext, "GetPostForTopic", 
                 values: new { categoryId, forumId, topicId, postId, fields }), "self", "GET"),

                 new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateTopicForForum", 
                 values: new { categoryId, forumId, topicId, postId }), "update_topic", "PUT"),

                 new Link(_linkGenerator.GetUriByAction(httpContext, "PartiallyUpdatePostForTopic", 
                 values: new { categoryId, forumId, topicId, postId }), "partially_update_topic", "PATCH"),

                 new Link(_linkGenerator.GetUriByAction(httpContext, "DeletePostForTopic", 
                 values: new { categoryId, forumId, topicId, postId }), "delete_topic", "DELETE"),
             };

            return links;
        }
        private LinkCollectionWrapper<Entity> CreateLinksForPosts(HttpContext httpContext, LinkCollectionWrapper<Entity> topicsWrapper,
            int forumCategoryId, int forumBaseId, int forumTopicId,
            IEnumerable<int>? collectionIds = null)
        {
            if (collectionIds == null)
            {
                topicsWrapper.Links
                    .Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetPostsForTopic", values: new { forumCategoryId, forumBaseId, forumTopicId }), "self", "GET"));
            }
            else
            {
                string ids = string.Join(",", collectionIds);
                topicsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetPostCollection",
                    values: new { forumCategoryId, forumBaseId, forumTopicId, ids }), "self", "GET"));
            }

            return topicsWrapper;
        }
    }
}