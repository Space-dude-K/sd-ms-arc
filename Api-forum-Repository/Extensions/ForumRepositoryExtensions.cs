using Entities.Models.Forum;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Entities.Models;

namespace Repository.Extensions
{
    public static class ForumRepositoryExtensions
    {
        public static IQueryable<ForumPost> FilterPosts(this IQueryable<ForumPost> posts, uint minLikes, uint maxLikes)
        {
            return posts.Where(e => (e.Likes >= minLikes && e.Likes <= maxLikes));
        }
        public static IQueryable<ForumPost> FilterPosts(this IQueryable<ForumPost> posts, int userId)
        {
            return posts.Where(e => e.ForumUserId == userId);
        }
        public static IQueryable<ForumTopic> Search(this IQueryable<ForumTopic> topics, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return topics;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return topics.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<ForumBase> Search(this IQueryable<ForumBase> forums, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return forums;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return forums.Where(e => e.ForumTitle.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<ForumCategory> Search(this IQueryable<ForumCategory> categories, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return categories;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return categories.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<AppUser> Search(this IQueryable<AppUser> users, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return users;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return users.Where(e => e.UserName.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<ForumCategory> Sort(this IQueryable<ForumCategory> categories, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return categories.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<ForumCategory>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return categories.OrderBy(e => e.Name);

            return categories.OrderBy(orderQuery); 
        }
        public static IQueryable<ForumBase> Sort(this IQueryable<ForumBase> forums, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return forums.OrderBy(e => e.ForumTitle);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<ForumBase>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return forums.OrderBy(e => e.ForumTitle);

            return forums.OrderBy(orderQuery);
        }
        public static IQueryable<ForumTopic> Sort(this IQueryable<ForumTopic> topics, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return topics.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<ForumTopic>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return topics.OrderBy(e => e.Name);

            return topics.OrderBy(orderQuery);
        }
        public static IQueryable<ForumPost> Sort(this IQueryable<ForumPost> posts, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return posts.OrderBy(e => e.Id);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<ForumPost>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return posts.OrderBy(e => e.Id);

            return posts.OrderBy(orderQuery);
        }
        public static IQueryable<AppUser> Sort(this IQueryable<AppUser> users, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return users.OrderBy(e => e.UserName);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<AppUser>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return users.OrderBy(e => e.UserName);

            return users.OrderBy(orderQuery);
        }
        // TODO. Generic way?
        public static IQueryable<T> SortGeneric<T>(this IQueryable<T> categories, string propertyName, string orderByQueryString)
        {
            var property = typeof(T).GetProperty(propertyName);

            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return categories.OrderBy(e => (string)property.GetValue(e, null));

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<T>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return categories.OrderBy(e => (string)property.GetValue(e, null));

            return categories.OrderBy(orderQuery);
        }
        public static IQueryable<T> SearchGeneric<T>(this IQueryable<T> data, string searchField, string searchTerm) where T : class
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            return data;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return data.Where(e => e.GetType().GetProperty(searchField).GetValue(e).ToString().ToLower().Contains(lowerCaseTerm)).AsQueryable<T>();
        }
    }
}