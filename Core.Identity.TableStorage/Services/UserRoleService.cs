using Core.Identity.TableStorage.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Identity.TableStorage.Services
{
    internal class UserRoleService<TUser, TRole, TUserRole> : IUserRoleService<TUser, TRole, TUserRole>
        where TUser : class, IUser, new()
        where TRole : class, IRole, new()
        where TUserRole : class, IUserRole, new()
    {
        private readonly IUserRepository<TUser> _userRepository;
        private readonly IUserRoleRepository<TUserRole> _userRoleRepository;
        private readonly IRoleRepository<TRole> _roleRepository;

        public UserRoleService(IUserRepository<TUser> userRepository, IUserRoleRepository<TUserRole> userRoleRepository, IRoleRepository<TRole> roleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
        }

        public async Task AddUserToRoleAsync(TUser user, string roleName)
        {

            var role = _roleRepository.FindByName(roleName);
            if (role == null)
            {
                throw new ArgumentException($"Invalid role {roleName}", nameof(roleName));
            }
            
            if (UserHasRole(user.Id, roleName))
            {
                return;
            }

            var userRole = new TUserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };

            await _userRoleRepository.InsertAsync(userRole);            
        }

        public async Task RemoveUserFromRoleAsync(TUser user, string roleName)
        {
            var role = _roleRepository.FindByName(roleName);
            if (role == null)
            {
                return;
            }

            var userRole = GetUserRole(user.Id, role.Id);

            if (userRole == null)
            {
                return;
            }

            await _userRoleRepository.DeleteAsync(userRole);
        }

        public async Task<List<TRole>> GetRolesByUser(string userId)
        {
            var result = new List<TRole>();

            var userRoles = _userRoleRepository.GetByUser(userId);
            foreach (var userRole in userRoles)
            {
                var role = await _roleRepository.GetAsync(userRole.RoleId);
                result.Add(role);
            }
            return result;
        }

        public async Task<List<TUser>> GetUsersByRole(string roleName)
        {
            var result = new List<TUser>();

            var role = _roleRepository.FindByName(roleName);
            if (role == null)
            {
                return result;
            }

            var userRoles = _userRoleRepository.GetByRole(role.Id);
            foreach (var userRole in userRoles)
            {
                var user = await _userRepository.GetAsync(userRole.UserId);
                result.Add(user);
            }
            return result;
        }

        public bool UserHasRole(string userId, string roleName)
        {
            var role = _roleRepository.FindByName(roleName);
            if (role == null)
            {
                throw new ArgumentException($"Invalid role {roleName}", nameof(roleName));
            }

            return UserHasRole(userId, role);
        }

        public bool UserHasRole(string userId, TRole role)
        {
            var userRoles = _userRoleRepository.GetByUser(userId);
            return userRoles.Any(r => r.RoleId == role.Id);
        }

        private TUserRole GetUserRole(string userId, string roleId)
        {
            var userRoles = _userRoleRepository.GetByUser(userId);
            return userRoles.FirstOrDefault(r => r.RoleId == roleId);
        }
    }
}
