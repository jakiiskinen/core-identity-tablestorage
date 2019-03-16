using Microsoft.Azure.Cosmos.Table;

namespace Core.Identity.TableStorage
{
    public interface IRole : ITableEntity
    {
        string Id { get; set; }
        string Name { get; set; }
        string NormalizedName { get; set; }
    }
}