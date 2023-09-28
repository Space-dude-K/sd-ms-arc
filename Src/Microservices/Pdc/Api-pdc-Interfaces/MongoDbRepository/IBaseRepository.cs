namespace Api_pdc_Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Get(string id);
        Task<IEnumerable<TEntity>> Get();
    }
}