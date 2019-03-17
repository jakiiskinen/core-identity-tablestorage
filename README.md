# Core.Identity.TableStorage
Table storage provider for AspNet Core Identity

## How to configure
- Create an instance of `DataContextSettingsProvider` (or roll your own by implementing `IDataContextSettingsProvider`), set the properties correctly and add it as a service.
- Call `IdentityBuilder` extension method `AddTableStorageIdentity<TUser, TRole, TUserRole>()`
- Call `IdentityBuilder.AddUserStore<UserStore<TUser, TRole, TUserRole>>()`

With these configurations, the basic user management features should be available.

`TUser` must be a type implementing `IUser` interface. The package contains default implementation `User`. Same with `TRole`. 

NuGet feed: https://www.nuget.org/packages/Core.Identity.TableStorage

## Versions

0.0.2 - Added Role support, refactored repositories
