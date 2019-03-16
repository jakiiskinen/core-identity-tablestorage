using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace Core.Identity.TableStorage.Repositories
{
    public interface IRepositoryBase<T> where T : class, ITableEntity, new()
    {
        List<T> GetAll();
        Task<T> GetAsync(string id);
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(T entity);
    }
}