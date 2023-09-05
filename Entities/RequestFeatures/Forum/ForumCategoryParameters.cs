namespace Entities.RequestFeatures.Forum
{
    public class ForumCategoryParameters : RequestParameters
    {
        public ForumCategoryParameters(string fields)
        {
            Fields = fields;
        }
        public ForumCategoryParameters()
        {
            OrderBy = "Name";
        }
    }
}