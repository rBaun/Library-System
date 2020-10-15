using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DataAccess.Entities;
using DataAccess.InterfaceRepos;

namespace DataAccess.Repos
{
    public class GuestLibraryRepos : IGuestLibraryRepos<Person>
    {
        public Func<IDbConnection> Connection { get; set; }

        public Person GetPersonByCardId(int card_id)
        {
            var c = new Card{ID = card_id};
            Person p;
            const string selectPersonSql = "SELECT * FROM PersonInfo WHERE card_id = @card_id;";
            using (var connection = Connection())
            {
                p = connection.Query<Person>(selectPersonSql, new { card_id }).Single();
            }
            p.Card = c;
            return p;
        }

        public List<Book> GetCatalogOfBooks()
        {
            var books = new List<Book>();
            const string selectCatalogOfBooks = "SELECT TOP 10000 * FROM CatalogOfBooks";
            using (var connection = Connection())
            {
                Book result = null;
                connection.Query<Book, Copy, Book>(selectCatalogOfBooks,
                    (bookInside, copyInside) =>
                    {
                        Book b = null;
                        if (books.All(x => x.ISBN != bookInside.ISBN))
                        {
                            b = bookInside;
                            result = bookInside;
                            books.Add(result);
                        }
                        else
                        {
                            result = books.Single(x => x.ISBN == bookInside.ISBN);
                        }

                        if (result.Copies.All(x => x.Barcode != copyInside.Barcode))
                        {
                            result.Copies.Add(new Copy{ IsAvailable = copyInside.IsAvailable, Barcode = copyInside.Barcode });
                        }

                        return result;
                    }, splitOn: "isAvailable");
            }

            return books;
        }

        public List<Person> GetTop10ActiveMembers()
        {
            const string selectTop10ActiveMembers = "SELECT * FROM Top10ActiveMembers";
            using (var connection = Connection())
            {
                return connection.Query<Person>(selectTop10ActiveMembers).ToList();
            }
        }

        public List<Book> GetTop10Books()
        {
            const string selectTop10Books = "SELECT * FROM Top10Books";
            using (var connection = Connection())
            {
                return connection.Query<Book>(selectTop10Books).ToList();
            }
        }

        public int GetAverageLoanTime()
        {
            const string selectAvgLoanTime = @"
                SELECT
	                AVG(DATEDIFF (day, l.StartDate, lc.ReturnDate)) as AverageLoanTime
                FROM Loan AS l
                INNER JOIN LoanCopy AS lc ON l.ID = lc.loanID;";
            using (var connection = Connection())
            {
                return connection.Query<int>(selectAvgLoanTime).Single();
            }
        }

        public int GetMemberAverageLoanTime(bool isProfessor)
        {
            const string selectProfessorAvgLoanTime = @"
                SELECT
	                AVG(DATEDIFF (day, l.StartDate, lc.ReturnDate)) as AverageProfessorLoanTime
                FROM Loan AS l
	            INNER JOIN LoanCopy AS lc ON l.ID = lc.loanID
	            INNER JOIN Person AS p ON l.card_id = p.card_id
                WHERE
	                p.isProfessor = @isProfessor;";
            using (var connection = Connection())
            {
                return connection.Query<int>(selectProfessorAvgLoanTime, new { isProfessor }).Single();
            }
        }
    }
}
