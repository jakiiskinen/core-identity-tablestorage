using Core.Identity.TableStorage.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Identity.TableStorage
{
    public class UserStore<T> : IUserStore<T>, IUserPasswordStore<T>, IUserEmailStore<T>, IUserRoleStore<T> where T : class, IUser, new()
    {
        private readonly IUserRepository<T> _userRepository;

        public UserStore(IUserRepository<T> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IdentityResult> CreateAsync(T user, CancellationToken cancellationToken)
        {
            var result = await _userRepository.InsertAsync(user);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
        }

        public async Task<T> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(userId);
            return user;
        }

        public Task<T> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _userRepository.FindByName(normalizedUserName);
            return Task.FromResult(user);
        }

        public Task<string> GetNormalizedUserNameAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(T user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(T user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(T user, CancellationToken cancellationToken)
        {
            await _userRepository.UpdateAsync(user);
            return IdentityResult.Success;
        }

        public Task SetPasswordHashAsync(T user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Password = passwordHash;
            return Task.CompletedTask;
        }

        public async Task<string> GetPasswordHashAsync(T user, CancellationToken cancellationToken)
        {
            var u = await _userRepository.GetAsync(user.Id);
            return u?.Password;
        }

        public async Task<bool> HasPasswordAsync(T user, CancellationToken cancellationToken)
        {
            var u = await _userRepository.GetAsync(user.Id);
            return !string.IsNullOrWhiteSpace(u.Password);
        }

        public Task SetEmailAsync(T user, string email, CancellationToken cancellationToken)
        {            
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(T user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task<T> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.FromResult(_userRepository.FindByEmail(normalizedEmail));
        }

        public Task<string> GetNormalizedEmailAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(T user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task AddToRoleAsync(T user, string roleName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveFromRoleAsync(T user, string roleName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(T user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(T user, string roleName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<T>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
