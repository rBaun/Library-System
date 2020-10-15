using DataAccess.Entities;

namespace DataAccess.InterfaceRepos
{
    public interface IUserRepos<TEntity> : IRepository<TEntity>
    {
        TEntity GetUserByCardID(int id);
        Librarian GetLibrarianById(int employee_id);
    }
}
