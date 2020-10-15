using Dapper;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DataAccess.InterfaceRepos;

namespace DataAccess.Repos
{
    public class BookRepos : IBookRepos<Book>
    {
        public Func<IDbConnection> Connection { get; set; }

        public Book Create(Book entity)
        {
            throw new NotImplementedException();
        }

        public Book Delete(Book entity)
        {
            throw new NotImplementedException();
        }

        public Book Get(Book entity)
        {
            const string sql = "select m.author, m.title, m.subjectArea, b.bookdescription, c.isAvailable, c.barcode from book b inner join copy c on c.book_id = b.ID inner join Material m on m.id = b.id where b.ISBN = @isbn";
            using (var connection = Connection())
            {
                connection.Query<Book, Copy, Book>(sql,
                    (Book, Copy) =>
                    {
                        entity.Copies.Add(Copy);
                        entity.BookDescription = Book.BookDescription;
                        entity.Author = Book.Author;
                        entity.SubjectArea = Book.SubjectArea;
                        entity.Title = Book.Title;

                        return entity;
                    },
                new { isbn = entity.ISBN }, splitOn: "isAvailable");
            }
            return entity;
        }

        public List<Book> GetBooksBySearchWord(string searchWord)
        {
            var selectSql = "select top 20 m.author, m.title, m.subjectArea, b.ISBN from Material m inner join Book b on m.ID = b.ID";
            if(!string.IsNullOrEmpty(searchWord))
            {
                selectSql += " where m.author LIKE @search or m.title LIKE @search";
            }

            using (var connection = Connection())
            {
                return connection.Query<Book>(selectSql, new { search = "%" + searchWord + "%" }).AsList();
            }
        }

        public Book Update(Book entity)
        {
            throw new NotImplementedException();
        }
    }
}
