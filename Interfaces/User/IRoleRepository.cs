using Entities.Models;

namespace Interfaces.User
{
    public interface IRoleRepository
    {
        Task<List<AppRole>> GetAllRolesAsync(bool trackChanges);
    }
}