namespace Core.Identity.TableStorage.Repositories
{
    public interface IUserRepository<T> : IRepositoryBase<T> where T : class, IUser, new()
    {
        T FindByName(string name);
        T FindByEmail(string email);
    }
}