using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DataAccess.InterfaceRepos;

namespace DataAccess.Repos
{
    public class UserRepos : IUserRepos<Person>
    {
        public Func<IDbConnection> Connection { get; set; }

        public Person Create(Person entity)
        {
            throw new NotImplementedException();
        }

        public Person Delete(Person entity)
        {
            throw new NotImplementedException();
        }

        public Person Get(Person entity)
        {
            throw new NotImplementedException();
        }

        public Librarian GetLibrarianById(int employee_id)
        {
            var librarian = new Librarian {EmployeeID = employee_id };
            var library = new Library();
            const string sql = "SELECT l.name, r.periodType, r.durationDays FROM Librarian lb inner join library l on lb.library_name = l.name" +
                                               " inner join rules r on r.name = l.name where lb.employeeid = @employeeid";
                                     
            using (var connection = Connection())
            {
                connection.Query<Library, Rules, Library>(sql,
                    (Library, Rules) =>
                    {
                        library.Name = Library.Name;
                        library.Rules.Add(new Rules { PeriodType = Rules.PeriodType, DurationDays = Rules.DurationDays });
                        return library;
                    },
                    new { employeeid = employee_id },
                    splitOn: "Name, periodType");
            }
            librarian.Library = library;
            return librarian;
        }

        public Person GetUserByCardID(int id)
        {
            const string selectPersonSql = "SELECT * FROM Person WHERE card_id = @id;";
            using (var connection = Connection())
            {
                return connection.Query<Person>(selectPersonSql, new { id }).Single();
            }
        }

        public Person Update(Person entity)
        {
            throw new NotImplementedException();
        }
    }
}
