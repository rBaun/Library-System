using DataAccess.Entities;

namespace DataAccess.InterfaceRepos
{
    public interface ILoanRepos<TEntity> : IRepository<TEntity>
    {
        bool ReturnBook(Copy copy);
        bool CheckForStatusOnCopy(Copy copy);
    }
}
