using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Core.Identity.TableStorage.Repositories
{
    internal class RoleRepository<T> : RepositoryBase<T>, IRoleRepository<T> where T : class, IRole, new()
    {
        public RoleRepository(IDataContextSettingsProvider dataContextSettingsProvider, ILogger<RoleRepository<T>> logger)
            : base(dataContextSettingsProvider, logger)
        {
        }

        protected override string TableName => "Roles";

        public T FindByName(string normalizedName)
        {
            var tbl = GetTable();
            var query = new TableQuery<T>()
                .Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition(nameof(IRole.Name), QueryComparisons.Equal, normalizedName),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition(nameof(IRole.PartitionKey), QueryComparisons.Equal, DataContextSettingsProvider.PartitionKey)));

            var result = tbl.ExecuteQuery(query).ToList();

            return result.FirstOrDefault();
        }
    }
}
