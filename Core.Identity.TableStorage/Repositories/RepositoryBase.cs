using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Identity.TableStorage.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, ITableEntity, new()
    {
        protected IDataContextSettingsProvider DataContextSettingsProvider { get; private set; }

        private CloudTableClient _client;

        public RepositoryBase(IDataContextSettingsProvider dataContextSettingsProvider, ILogger logger)
        {
            DataContextSettingsProvider = dataContextSettingsProvider;
            Logger = logger;
            CreateClient();
        }

        private void CreateClient()
        {
            var credentials = new StorageCredentials(DataContextSettingsProvider.AccountName, DataContextSettingsProvider.KeyValue);
            CloudStorageAccount c = new CloudStorageAccount(credentials, DataContextSettingsProvider.TableStorageUri);
            _client = c.CreateCloudTableClient();
        }

        protected abstract string TableName { get; }

        protected ILogger Logger { get; }

        protected CloudTable GetTable()
        {
            var table = _client.GetTableReference(TableName);
            table.CreateIfNotExists();
            return table;
        }

        protected virtual void BeforeInsert(T entity)
        {
            entity.PartitionKey = DataContextSettingsProvider.PartitionKey;
            entity.ETag = "*";
        }

        public async Task<T> InsertAsync(T entity)
        {
            try
            {
                var tbl = GetTable();
                BeforeInsert(entity);

                var operation = TableOperation.Insert(entity);

                var result = await tbl.ExecuteAsync(operation);
                return result.Result as T;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error when inserting entity");
                throw;
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                var tbl = GetTable();

                var operation = TableOperation.Replace(entity);
                var result = await tbl.ExecuteAsync(operation);

                return result.Result as T;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error when updating entity");
                throw;
            }
        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                var tbl = GetTable();

                var operation = TableOperation.Delete(entity);
                await tbl.ExecuteAsync(operation);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error when deleting entity");
                throw;
            }
        }

        public async Task<T> GetAsync(string id)
        {
            try
            {
                var tbl = GetTable();

                var operation = TableOperation.Retrieve<T>(DataContextSettingsProvider.PartitionKey, id);

                var result = (await tbl.ExecuteAsync(operation)).Result;

                return result as T;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error when getting entity");
                throw;
            }
        }

        public List<T> GetAll()
        {
            try
            {
                var tbl = GetTable();
                var query = new TableQuery<T>()
                    .Where(TableQuery.GenerateFilterCondition(nameof(IUser.PartitionKey), QueryComparisons.Equal, DataContextSettingsProvider.PartitionKey));

                var result = tbl .ExecuteQuery(query).ToList();

                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error when getting all entities");
                throw;
            }
        }
    }
}
