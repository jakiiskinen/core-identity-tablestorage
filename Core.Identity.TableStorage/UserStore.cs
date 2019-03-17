using Core.Identity.TableStorage.Repositories;
using Core.Identity.TableStorage.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Identity.TableStorage
{
    public class UserStore<TUser, TRole, TUserRole> : IUserStore<TUser>, IUserPasswordStore<TUser>, IUserEmailStore<TUser>, IUserRoleStore<TUser> 
        where TUser : class, IUser, new()
        where TRole : class, IRole, new()
        where TUserRole : class, IUserRole, new()
    {
        private readonly IUserRepository<TUser> _userRepository;
        private readonly IUserRoleService<TUser, TRole, TUserRole> _userRoleService;

        public UserStore(IUserRepository<TUser> userRepository, IUserRoleService<TUser, TRole, TUserRole> userRoleService)
        {
            _userRepository = userRepository;
            _userRoleService = userRoleService;
        }

        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            var result = await _userRepository.InsertAsync(user);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(userId);
            return user;
        }

        public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _userRepository.FindByName(normalizedUserName);
            return Task.FromResult(user);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            await _userRepository.UpdateAsync(user);
            return IdentityResult.Success;
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Password = passwordHash;
            return Task.CompletedTask;
        }

        public async Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            var u = await _userRepository.GetAsync(user.Id);
            return u?.Password;
        }

        public async Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            var u = await _userRepository.GetAsync(user.Id);
            return !string.IsNullOrWhiteSpace(u.Password);
        }

        public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {            
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.FromResult(_userRepository.FindByEmail(normalizedEmail));
        }

        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            await _userRoleService.AddUserToRoleAsync(user, roleName);
        }

        public async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            await _userRoleService.RemoveUserFromRoleAsync(user, roleName);
        }

        public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            var roles = await _userRoleService.GetRolesByUser(user.Id);
            return roles.Select(r => r.Name).ToList();
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(_userRoleService.UserHasRole(user.Id, roleName));
        }

        public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return await _userRoleService.GetUsersByRole(roleName);
        }

        public List<TUser> GetAllUsersAsync()
        {
            return _userRepository.GetAll();
        }
    }
}
