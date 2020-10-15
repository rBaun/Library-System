using DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.InterfaceRepos;

namespace DataAccess
{
    public static class RepositoryFactory<TEntity>
    {
        public static ILoanRepos<TEntity> CreateLoanRepos()
        {
            return new LoanRepos() as ILoanRepos<TEntity>;
        }

        public static IUserRepos<TEntity> CreateUserRepos()
        {
            return new UserRepos() as IUserRepos<TEntity>;
        }

        public static ICardRepos<TEntity> CreateCardRepos()
        {
            return new CardRepos() as ICardRepos<TEntity>;
        }

        public static IGuestLibraryRepos<TEntity> CreateGuestRepos()
        {
            return new GuestLibraryRepos() as IGuestLibraryRepos<TEntity>;
        }

        public static IBookRepos<TEntity> CreateBookRepos()
        {
            return new BookRepos() as IBookRepos<TEntity>;
        }
    }
}
