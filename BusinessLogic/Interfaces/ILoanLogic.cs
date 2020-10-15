using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Interfaces
{
    public interface ILoanLogic
    {
        Loan CreateLoan(int card_id, int barcode1, int barcode2, int barcode3, int barcode4, int barcode5,
            int employee_id);
        bool ReturnBook(int barcode);
        bool CanBorrow(Card card, int countToBorrow);
        bool IsCardExpired(Card card);
        DateTime CalculateDueDate(Person person, Library library);
    }
}
