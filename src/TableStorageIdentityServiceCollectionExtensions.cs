using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace Core.Identity.TableStorage
{
    public static class TableStorageIdentityServiceCollectionExtensions
    {
        public static IdentityBuilder AddTableStorageIdentity<TUser>(this IdentityBuilder identityBuilder) where TUser : class, IUser, new()
        {
            identityBuilder.Services.AddTransient<IUserRepository<TUser>, UserRepository<TUser>>();
            identityBuilder.Services.AddTransient<UserStore<TUser>>();
            
            identityBuilder.AddUserStore<UserStore<TUser>>();

            return identityBuilder;
        }
    }
}
