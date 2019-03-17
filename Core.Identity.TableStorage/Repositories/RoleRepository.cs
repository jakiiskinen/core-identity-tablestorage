using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Core.Identity.TableStorage.Repositories
{
    public class RoleRepository<T> : RepositoryBase<T>, IRoleRepository<T> where T : class, IRole, new()
    {
        public RoleRepository(IDataContextSettingsProvider dataContextSettingsProvider, ILogger<RoleRepository<T>> logger)
            : base(dataContextSettingsProvider, logger)
        {
        }

        protected override string TableName => "Roles";

        protected override void BeforeInsert(T entity)
        {
            base.BeforeInsert(entity);
            entity.Id = Guid.NewGuid().ToString();
            entity.RowKey = entity.Id;
        }

        public T FindByName(string normalizedName)
        {
            try
            {
                var tbl = GetTable();
                var query = new TableQuery<T>()
                    .Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition(nameof(IRole.NormalizedName), QueryComparisons.Equal, normalizedName),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition(nameof(IRole.PartitionKey), QueryComparisons.Equal, DataContextSettingsProvider.PartitionKey)));

                var result = tbl.ExecuteQuery(query).ToList();

                return result.FirstOrDefault();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error when updating entity");
                throw;
            }
        }
    }
}
