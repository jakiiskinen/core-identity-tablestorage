using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Core.Identity.TableStorage.Repositories
{
    public class UserRepository<T> : RepositoryBase<T>, IUserRepository<T> where T : class, IUser, new()
    {
        protected override string TableName => "Users";

        public UserRepository(IDataContextSettingsProvider dataContextSettingsProvider, ILogger<UserRepository<T>> logger)
            : base(dataContextSettingsProvider, logger)
        {
        }

        protected override void BeforeInsert(T user)
        {
            base.BeforeInsert(user);
            user.Id = Guid.NewGuid().ToString();
        }

        public T FindByName(string normalizedName)
        {
            try
            {
                var tbl = GetTable();
                var query = new TableQuery<T>()
                    .Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition(nameof(IUser.NormalizedUserName), QueryComparisons.Equal, normalizedName),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition(nameof(IUser.PartitionKey), QueryComparisons.Equal, DataContextSettingsProvider.PartitionKey)));

                var result = tbl.ExecuteQuery(query).ToList();

                return result.FirstOrDefault();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error when updating entity");
                throw;
            }
        }

        public T FindByEmail(string normalizedEmail)
        {
            try
            {
                var tbl = GetTable();
                var query = new TableQuery<T>()
                    .Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition(nameof(IUser.NormalizedEmail), QueryComparisons.Equal, normalizedEmail),                    
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition(nameof(IUser.PartitionKey), QueryComparisons.Equal, DataContextSettingsProvider.PartitionKey)));

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

