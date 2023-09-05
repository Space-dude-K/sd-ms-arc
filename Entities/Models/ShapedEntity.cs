namespace Entities.Models
{
    public class ShapedEntity
    {
        public ShapedEntity()
        {
            Entity = new Entity();
        }
        public object Id { get; set; }
        public Entity Entity { get; set; }
    }
}