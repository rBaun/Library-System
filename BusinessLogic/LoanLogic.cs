using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataAccess.InterfaceRepos;

namespace BusinessLogic
{
    public class LoanLogic : ILoanLogic
    {
        private readonly ILoanRepos<Loan> _loanRepository;
        private readonly ICardRepos<Card> _cardRepository;
        private readonly IUserRepos<Person> _userRepository;

        public LoanLogic(ILoanRepos<Loan> loanRepository, 
                            ICardRepos<Card> cardRepository, IUserRepos<Person> userRepository)
        {
            _loanRepository = loanRepository;
            _cardRepository = cardRepository;
            _userRepository = userRepository;
        }

        public Loan CreateLoan(int card_id, int barcode1, int barcode2, int barcode3, int barcode4, int barcode5,
            int employee_id)
        {
            var loan = new Loan
            {
                Card = _cardRepository.Get(new Card { ID = card_id })
            };

            if (loan.Card.Expires == new DateTime())
            {
                loan.ErrorMessage = "Ugyldigt lånerkort";
                return loan;
            }


            loan.Copies = new List<Copy>{ new Copy { Barcode = barcode1},
                new Copy { Barcode = barcode2},
                new Copy { Barcode = barcode3},
                new Copy { Barcode = barcode4},
                new Copy { Barcode = barcode5}};

            int countToBorrow = loan.Copies.Count(x => x.Barcode != 0);

            if (!CanBorrow(loan.Card, countToBorrow))
            {
                loan.ErrorMessage = "Der kan ikke lånes mere end 5 bøger pr. kort";
                return loan;
            }

            foreach (var item in loan.Copies)
            {
                if(item.Barcode > 0 && !_loanRepository.CheckForStatusOnCopy(item))
                {
                    loan.ErrorMessage = "Bogen med barcode: " + item.Barcode + " er ikke tilgængelig";
                    return loan;
                }
            }


            var person = _userRepository.GetUserByCardID(loan.Card.ID);
            var librarian = _userRepository.GetLibrarianById(employee_id);
            loan.DueDate = CalculateDueDate(person, librarian.Library);
            loan.Librarian = librarian;
            loan.StartDate = DateTime.Now.Date;

            return _loanRepository.Create(loan);
        }

        public bool CanBorrow(Card c, int countToBorrow)
        {
            var currentBorrowedBooks = c.Loans.Where(x => x.IsActive == true)
                                        .SelectMany(x => x.Copies).Count(z => z.IsAvailable == false);
            return currentBorrowedBooks + countToBorrow <= 5;
        }

        public bool IsCardExpired(Card card)
        {
            var isExpired = true;
            var dt = DateTime.Now.Date;

            if (dt.CompareTo(card.Expires) == -1 || dt.CompareTo(card.Expires) == 0)
                isExpired = false;

            return isExpired;
        }

        public DateTime CalculateDueDate(Person person, Library library)
        {
            var dt = DateTime.Now.Date;
            foreach (var rule in library.Rules)
            {
                if(person.IsProfessor && rule.PeriodType == PeriodType.ProfessorLoanDuration)
                    return dt.AddDays(rule.DurationDays);
                if(!person.IsProfessor && rule.PeriodType == PeriodType.MemberLoanDuration)
                    return dt.AddDays(rule.DurationDays);
            }

            return dt;
        }

        public bool ReturnBook(int barcode)
        {
            var time = DateTime.Now.Date;
            var c = new Copy { Barcode = barcode, ReturnDate = time };
            return _loanRepository.ReturnBook(c);
        }
    }
}
