using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Identity.TableStorage
{
    public interface IUserRepository<T> where T : class, IUser, new()
    {
        T FindByName(string name);
        T FindByEmail(string email);
        List<T> GetAllUsers();
        Task<T> GetAsync(string id);
        Task<T> InsertAsync(T user);
        Task<T> UpdateAsync(T user);
    }
}