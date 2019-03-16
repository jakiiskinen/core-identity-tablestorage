using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Identity.TableStorage
{
    public class UserRepository<T> : IUserRepository<T> where T : class, IUser, new()
    {
        private readonly IDataContextSettingsProvider _dataContextSettingsProvider;
        private readonly ILogger<UserRepository<T>> _logger;
        private CloudTableClient _client;

        public UserRepository(IDataContextSettingsProvider dataContextSettingsProvider, ILogger<UserRepository<T>> logger)
        {
            _dataContextSettingsProvider = dataContextSettingsProvider;
            _logger = logger;
            CreateClient();
        }

        private void CreateClient()
        {
            var credentials = new StorageCredentials(_dataContextSettingsProvider.AccountName, _dataContextSettingsProvider.KeyValue);
            CloudStorageAccount c = new CloudStorageAccount(credentials, _dataContextSettingsProvider.TableStorageUri);
            _client = c.CreateCloudTableClient();
        }

        private void SetInternalFields(T user)
        {
            user.Id = Guid.NewGuid().ToString();
            user.PartitionKey = _dataContextSettingsProvider.PartitionKey;
            user.ETag = "*";
        }

        public async Task<T> InsertAsync(T user)
        {
            var tbl = GetUserTable();
            SetInternalFields(user);

            var operation = TableOperation.Insert(user);

            var result = await tbl.ExecuteAsync(operation);

            return result.Result as T;
        }

        public async Task<T> UpdateAsync(T user)
        {
            var tbl = GetUserTable();

            var operation = TableOperation.Replace(user);
            var result = await tbl.ExecuteAsync(operation);

            return result.Result as T;
        }

        public async Task<T> GetAsync(string id)
        {
            var tbl = GetUserTable();

            var operation = TableOperation.Retrieve<T>(_dataContextSettingsProvider.PartitionKey, id);

            var result = (await tbl.ExecuteAsync(operation)).Result;

            return result as T;
        }

        public List<T> GetAllUsers()
        {
            var tbl = GetUserTable();

            var result = tbl.ExecuteQuery(new TableQuery<T>()).ToList();

            return result;
        }

        public T FindByName(string name)
        {
            var tbl = GetUserTable();
            var query = new TableQuery<T>()
                .Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition(nameof(IUser.NormalizedUserName), QueryComparisons.Equal, name),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition(nameof(IUser.PartitionKey), QueryComparisons.Equal, _dataContextSettingsProvider.PartitionKey)));

            var result = tbl.ExecuteQuery(query).ToList();

            return result.FirstOrDefault();
        }

        public T FindByEmail(string email)
        {
            var tbl = GetUserTable();
            var query = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition(nameof(IUser.NormalizedEmail), QueryComparisons.Equal, email))
                .Where(TableQuery.GenerateFilterCondition(nameof(IUser.PartitionKey), QueryComparisons.Equal, _dataContextSettingsProvider.PartitionKey));

            var result = tbl.ExecuteQuery(query).ToList();

            return result.FirstOrDefault();
        }

        private CloudTable GetUserTable()
        {
            var userTable = _client.GetTableReference("Users");
            return userTable;
        }
    }
}

