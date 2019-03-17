using System.Collections.Generic;

namespace Core.Identity.TableStorage.Repositories
{
    public interface IUserRoleRepository<T> : IRepositoryBase<T> where T : class, IUserRole, new()
    {
        List<T> GetByUser(string userId);
        List<T> GetByRole(string roleId);
    }
}
