using Microsoft.Azure.Cosmos.Table;

namespace Core.Identity.TableStorage
{
    public interface IUserRole : ITableEntity
    {
        string RoleId { get; set; }
        string UserId { get; set; }
    }
}