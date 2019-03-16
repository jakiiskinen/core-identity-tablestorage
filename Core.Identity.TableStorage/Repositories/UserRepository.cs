using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Core.Identity.TableStorage.Repositories
{
    internal class UserRepository<T> : RepositoryBase<T>, IUserRepository<T> where T : class, IUser, new()
    {        
        protected override string TableName => "Users";

        public UserRepository(IDataContextSettingsProvider dataContextSettingsProvider, ILogger<UserRepository<T>> logger)
            : base(dataContextSettingsProvider, logger)
        {
        }

        protected override void BeforeInsert(T user)
        {
            user.Id = Guid.NewGuid().ToString();
            user.PartitionKey = DataContextSettingsProvider.PartitionKey;
            user.ETag = "*";
        }

        public T FindByName(string nomalizedName)
        {
            var tbl = GetTable();
            var query = new TableQuery<T>()
                .Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition(nameof(IUser.NormalizedUserName), QueryComparisons.Equal, nomalizedName),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition(nameof(IUser.PartitionKey), QueryComparisons.Equal, DataContextSettingsProvider.PartitionKey)));

            var result = tbl.ExecuteQuery(query).ToList();

            return result.FirstOrDefault();
        }

        public T FindByEmail(string normalizedEmail)
        {
            var tbl = GetTable();
            var query = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition(nameof(IUser.NormalizedEmail), QueryComparisons.Equal, normalizedEmail))
                .Where(TableQuery.GenerateFilterCondition(nameof(IUser.PartitionKey), QueryComparisons.Equal, DataContextSettingsProvider.PartitionKey));

            var result = tbl.ExecuteQuery(query).ToList();

            return result.FirstOrDefault();
        }
    }
}

