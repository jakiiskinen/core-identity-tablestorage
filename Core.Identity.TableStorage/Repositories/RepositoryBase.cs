using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Identity.TableStorage.Repositories
{
    internal abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, ITableEntity, new()
    {
        protected IDataContextSettingsProvider DataContextSettingsProvider { get; private set; }
        private readonly ILogger _logger;
        private CloudTableClient _client;

        public RepositoryBase(IDataContextSettingsProvider dataContextSettingsProvider, ILogger logger)
        {
            DataContextSettingsProvider = dataContextSettingsProvider;
            _logger = logger;
            CreateClient();
        }
               
        private void CreateClient()
        {
            var credentials = new StorageCredentials(DataContextSettingsProvider.AccountName, DataContextSettingsProvider.KeyValue);
            CloudStorageAccount c = new CloudStorageAccount(credentials, DataContextSettingsProvider.TableStorageUri);
            _client = c.CreateCloudTableClient();
        }

        protected abstract string TableName { get;  }

        protected CloudTable GetTable()
        {         
            return _client.GetTableReference(TableName);
        }

        protected virtual void BeforeInsert(T entity)
        {
        }

        public async Task<T> InsertAsync(T entity)
        {
            var tbl = GetTable();
            BeforeInsert(entity);

            var operation = TableOperation.Insert(entity);

            var result = await tbl.ExecuteAsync(operation);

            return result.Result as T;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var tbl = GetTable();

            var operation = TableOperation.Replace(entity);
            var result = await tbl.ExecuteAsync(operation);

            return result.Result as T;
        }

        public async Task<T> GetAsync(string id)
        {
            var tbl = GetTable();

            var operation = TableOperation.Retrieve<T>(DataContextSettingsProvider.PartitionKey, id);

            var result = (await tbl.ExecuteAsync(operation)).Result;

            return result as T;
        }

        public List<T> GetAll()
        {
            var tbl = GetTable();

            var result = tbl.ExecuteQuery(new TableQuery<T>()).ToList();

            return result;
        }
    }
}
