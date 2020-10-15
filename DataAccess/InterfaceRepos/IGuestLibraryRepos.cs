using System;
using System.Collections.Generic;
using System.Data;
using DataAccess.Entities;

namespace DataAccess.InterfaceRepos
{
    public interface IGuestLibraryRepos<out TEntity>
    {
        Func<IDbConnection> Connection { get; set; }
        TEntity GetPersonByCardId(int card_id);
        List<Book> GetCatalogOfBooks();
        List<Person> GetTop10ActiveMembers();
        List<Book> GetTop10Books();
        int GetAverageLoanTime();
        int GetMemberAverageLoanTime(bool isProfessor);
    }
}
