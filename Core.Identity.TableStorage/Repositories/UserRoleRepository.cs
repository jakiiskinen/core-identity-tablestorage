using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;

namespace Core.Identity.TableStorage.Repositories
{
    public class UserRoleRepository<T> : RepositoryBase<T>, IUserRoleRepository<T>
        where T : class, IUserRole, new()
    {
        public UserRoleRepository(IDataContextSettingsProvider dataContextSettingsProvider, ILogger<IUserRoleRepository<T>> logger)
            : base(dataContextSettingsProvider, logger)
        {
        }

        protected override string TableName => "UserRoles";

        protected override void BeforeInsert(T entity)
        {
            base.BeforeInsert(entity);
            entity.RowKey = entity.RoleId + "_" + entity.UserId;
        }

        public List<T> GetByRole(string roleId)
        {
            try
            {
                var tbl = GetTable();
                var query = new TableQuery<T>()
                    .Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition(nameof(IUserRole.RoleId), QueryComparisons.Equal, roleId),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition(nameof(IUserRole.PartitionKey), QueryComparisons.Equal, DataContextSettingsProvider.PartitionKey)));

                var result = tbl.ExecuteQuery(query).ToList();

                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error when updating entity");
                throw;
            }
        }

        public List<T> GetByUser(string userId)
        {
            try
            {
                var tbl = GetTable();
                var query = new TableQuery<T>()
                    .Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition(nameof(IUserRole.UserId), QueryComparisons.Equal, userId),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition(nameof(IUserRole.PartitionKey), QueryComparisons.Equal, DataContextSettingsProvider.PartitionKey)));

                var result = tbl.ExecuteQuery(query).ToList();

                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error when updating entity");
                throw;
            }
        }
    }
}
