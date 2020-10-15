using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DataAccess.InterfaceRepos;

namespace DataAccess
{
    public static class RepositoryExtensions
    {
        public static ILoanRepos<TEntity> WithLoan<TEntity>(this ILoanRepos<TEntity> repository, Func<IDbConnection> connectionFactory)
        {
            repository.Connection = connectionFactory;
            return repository;
        }

        public static ICardRepos<TEntity> WithCard<TEntity>(this ICardRepos<TEntity> repository, Func<IDbConnection> connectionFactory)
        {
            repository.Connection = connectionFactory;
            return repository;
        }

        public static IUserRepos<TEntity> WithUser<TEntity>(this IUserRepos<TEntity> repository, Func<IDbConnection> connectionFactory)
        {
            repository.Connection = connectionFactory;
            return repository;
        }

        public static IGuestLibraryRepos<TEntity> With<TEntity>(this IGuestLibraryRepos<TEntity> repository, Func<IDbConnection> connectionFactory)
        {
            repository.Connection = connectionFactory;
            return repository;
        }

        public static IBookRepos<TEntity> WithBook<TEntity>(this IBookRepos<TEntity> repository, Func<IDbConnection> connectionFactory)
        {
            repository.Connection = connectionFactory;
            return repository;
        }
    }
}
