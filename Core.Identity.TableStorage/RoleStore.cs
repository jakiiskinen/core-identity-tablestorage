using Core.Identity.TableStorage.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Identity.TableStorage
{
    public class RoleStore<T> : IRoleStore<T> where T : class, IRole, new()
    {
        private readonly IRoleRepository<T> _roleRepository;

        public RoleStore(IRoleRepository<T> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IdentityResult> CreateAsync(T role, CancellationToken cancellationToken)
        {
            await _roleRepository.InsertAsync(role);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(T role, CancellationToken cancellationToken)
        {
            await _roleRepository.DeleteAsync(role);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        public async Task<T> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await _roleRepository.GetAsync(roleId);
        }

        public Task<T> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(_roleRepository.FindByName(normalizedRoleName));
        }

        public Task<string> GetNormalizedRoleNameAsync(T role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(T role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(T role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(T role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(T role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(T role, CancellationToken cancellationToken)
        {
            await _roleRepository.UpdateAsync(role);
            return IdentityResult.Success;
        }
    }
}
