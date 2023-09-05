namespace Entities.RequestFeatures.User
{
    public class UserParameters : RequestParameters
    {
        public UserParameters(string fields)
        {
            Fields = fields;
        }
        public UserParameters()
        {
            OrderBy = "UserName";
        }
    }
}