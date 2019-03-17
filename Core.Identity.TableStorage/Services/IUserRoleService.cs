using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Identity.TableStorage.Services
{
    public interface IUserRoleService<TUser, TRole, TUserRole>
        where TUser : class, IUser, new()
        where TRole : class, IRole, new()
        where TUserRole : class, IUserRole, new()
    {
        bool UserHasRole(string userId, TRole role);
        bool UserHasRole(string userId, string roleName);
        Task AddUserToRoleAsync(TUser user, string roleName);
        Task RemoveUserFromRoleAsync(TUser user, string roleName);
        Task<List<TRole>> GetRolesByUser(string userId);
        Task<List<TUser>> GetUsersByRole(string roleName);
    }
}