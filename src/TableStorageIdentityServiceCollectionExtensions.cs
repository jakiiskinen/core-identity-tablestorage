using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace Core.Identity.TableStorage
{
    public static class TableStorageIdentityServiceCollectionExtensions
    {
        /// <summary>
        /// Register required services for table storage provider.
        /// </summary>
        /// <typeparam name="TUser">Type of the IUser implementation.</typeparam>
        /// <param name="identityBuilder">Identity builder instance.</param>
        /// <returns>IdentityBuilder instance.</returns>
        public static IdentityBuilder AddTableStorageIdentity<TUser>(this IdentityBuilder identityBuilder) where TUser : class, IUser, new()
        {
            identityBuilder.Services.AddTransient<IUserRepository<TUser>, UserRepository<TUser>>();
            identityBuilder.Services.AddTransient<UserStore<TUser>>();
            
            identityBuilder.AddUserStore<UserStore<TUser>>();

            return identityBuilder;
        }
    }
}
