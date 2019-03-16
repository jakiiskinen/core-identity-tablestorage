namespace Core.Identity.TableStorage.Repositories
{
    public interface IRoleRepository<T> : IRepositoryBase<T> where T : class, IRole, new()
    {
        T FindByName(string normalizedName);
    }
}
