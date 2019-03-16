using Microsoft.Azure.Cosmos.Table;

namespace Core.Identity.TableStorage
{
    public class Role : TableEntity, IRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
