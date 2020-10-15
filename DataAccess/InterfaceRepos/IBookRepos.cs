using System.Collections.Generic;
using DataAccess.Entities;

namespace DataAccess.InterfaceRepos
{
    public interface IBookRepos<TEntity> : IRepository<TEntity>
    {
        List<Book> GetBooksBySearchWord(string searchWord);
    }
}
