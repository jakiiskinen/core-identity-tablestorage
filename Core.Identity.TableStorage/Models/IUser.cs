using Microsoft.Azure.Cosmos.Table;

namespace Core.Identity.TableStorage
{
    public interface IUser : ITableEntity
    {
        string Id { get; set; }
        string UserName { get; set; }
        string NormalizedUserName { get; set; }
        string Email { get; set; }
        string NormalizedEmail { get; set; }
        bool EmailConfirmed { get; set; }
        string Password { get; set; }
        string DisplayName { get; set; }
    }
}