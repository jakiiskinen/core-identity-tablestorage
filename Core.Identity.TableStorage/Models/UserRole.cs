using Microsoft.Azure.Cosmos.Table;

namespace Core.Identity.TableStorage
{
    public class UserRole : TableEntity, IUserRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
