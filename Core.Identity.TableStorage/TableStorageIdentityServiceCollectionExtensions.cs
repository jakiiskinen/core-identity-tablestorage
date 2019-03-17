using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Core.Identity.TableStorage.Repositories;
using Core.Identity.TableStorage.Services;

namespace Core.Identity.TableStorage
{
    public static class TableStorageIdentityServiceCollectionExtensions
    {
        /// <summary>
        /// Register required services for table storage provider.
        /// </summary>
        /// <typeparam name="TUser">Type of the IUser implementation.</typeparam>
        /// <typeparam name="TRole">Type of the IRole implementation.</typeparam>
        /// <param name="identityBuilder">Identity builder instance.</param>
        /// <returns>IdentityBuilder instance.</returns>
        public static IdentityBuilder AddTableStorageIdentity<TUser, TRole, TUserRole>(this IdentityBuilder identityBuilder) 
            where TUser : class, IUser, new()
            where TRole : class, IRole, new()
            where TUserRole : class, IUserRole, new()
        {

            identityBuilder.Services.AddTransient<UserStore<TUser, TRole, TUserRole>>();

            identityBuilder.Services.AddTransient<IUserRepository<TUser>, UserRepository<TUser>>();            
            identityBuilder.Services.AddTransient<IRoleRepository<TRole>, RoleRepository<TRole>>();
            identityBuilder.Services.AddTransient<IUserRoleRepository<TUserRole>, UserRoleRepository<TUserRole>>();

            identityBuilder.Services.AddTransient<IUserRoleService<TUser, TRole, TUserRole>, UserRoleService<TUser, TRole, TUserRole>>();

            return identityBuilder;
        }
    }
}
