using Dapper;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataAccess.InterfaceRepos;

namespace DataAccess.Repos
{
    public class LoanRepos : ILoanRepos<Loan>
    {
        public Func<IDbConnection> Connection { get; set; }

        public bool CheckForStatusOnCopy(Copy copy)
        {
            using (var connection = Connection())
            {
                const string sql = "select isAvailable from copy where barcode = @barcode";
                return connection.Query<bool>(sql, new { barcode = copy.Barcode }).FirstOrDefault();
            }
        }

        public Loan Create(Loan entity)
        {
            using (var connection = Connection())
            {
                const string insertLoanSql = "INSERT INTO Loan (startDate, dueDate, isActive, librarian_id, card_id)" +
                                             " VALUES (@StartDate, @DueDate, 1, @librarian_id, @Card_id) SELECT CAST(SCOPE_IDENTITY() as int)";
                entity.ID = connection.Query<int>(insertLoanSql, new
                {
                    entity.StartDate,
                    entity.DueDate,
                    librarian_id = entity.Librarian.EmployeeID,
                    Card_id = entity.Card.ID
                }).Single();

                if (entity.ID <= 0) return entity;
                const string insertLoanCopySql = "INSERT INTO LoanCopy (loanID, copyBarcode) VALUES (@LoanID, @CopyBarcode)";
                const string updateCopySql = "UPDATE Copy SET isAvailable = @IsAvailable WHERE barcode = @Barcode";
                foreach (var copy in entity.Copies.Where(copy => copy.Barcode > 0))
                {
                    connection.Execute(insertLoanCopySql, new
                    {
                        LoanID = entity.ID,
                        CopyBarcode = copy.Barcode
                    });

                    copy.IsAvailable = false;
                    connection.Execute(updateCopySql, new
                    {
                        IsAvailable = copy.IsAvailable,
                        Barcode = copy.Barcode
                    });
                }
            }
            return entity;
        }

        public Loan Delete(Loan entity)
        {
            throw new NotImplementedException();
        }

        public Loan Get(Loan entity)
        {
            throw new NotImplementedException();
        }

        //Kalder Stored Procedure til at update returnDate. En trigger i DB sætter
        //Kopiens status til at være tilgængelig igen.
        public bool ReturnBook(Copy copy)
        {
            using (var connection = Connection())
            {
                const string sql = "EXEC returnBook @returnDate, @barcode";
                var result = connection.Execute(sql, new { returnDate = copy.ReturnDate, barcode = copy.Barcode });
                return result != 0;
            }
        }

        public Loan Update(Loan entity)
        {
            throw new NotImplementedException();
        }
    }
}
