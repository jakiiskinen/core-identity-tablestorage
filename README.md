# Core.Identity.TableStorage
Table storage provider for AspNet Core Identity

## How to configure
- Create an instance of `IDataContextSettingsProvider`, set the settings correctly and add it as a service.
- Call `IdentityBuilder` extension method `AddTableStorageIdentity<TUser>()`
- Call` IdentityBuilder.AddUserStore<UserStore<TUser>>()`

With these configurations, the basic user management features should be available.

`TUser` must be a type implementing `IUser` interface. The package contains default implementation `User`.

NuGet feed: https://www.nuget.org/packages/Core.Identity.TableStorage
