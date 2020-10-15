using BusinessLogic;
using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using DataAccess.InterfaceRepos;
using Xunit;

namespace TestProject
{
    public class CreateLoan : IDisposable
    {
        private Person Person { get; set; }
        private Person PersonCurrentNoLoans { get; set; }
        private Librarian Lib { get; }
        public CreateLoan()
        {
            Lib = new Librarian
            {
                EmployeeID = 1,
                Library = new Library
                {
                    Name = "GTL",
                    Rules = new List<Rules>
                    {
                        new Rules {PeriodType = PeriodType.MemberLoanDuration, DurationDays = 21},
                        new Rules {PeriodType = PeriodType.ProfessorLoanDuration, DurationDays = 90}
                    }
                }
            };
            Person = new Person
            {
                FirstName = "Test Navn",
                LastName = "Test Efternavn",
                IsProfessor = false,
                Card = new Card
                {
                    ID = 1,
                    Expires = DateTime.Now,
                    Loans = new List<Loan>
                    {
                        new Loan { IsActive = true, Copies = new List<Copy>
                            {
                                new Copy { Barcode = 1, IsAvailable = false },
                                new Copy { Barcode = 2, IsAvailable = false },
                            }
                        },
                        new Loan { IsActive = true, Copies = new List<Copy>
                            {
                                new Copy { Barcode = 3, IsAvailable = false },
                                new Copy { Barcode = 4, IsAvailable = false },
                            }
                        }
                    }
                }
            };
            PersonCurrentNoLoans = new Person
            {
                FirstName = "Test Navn",
                LastName = "Test Efternavn",
                IsProfessor = false,
                Card = new Card
                {
                    ID = 2,
                    Expires = DateTime.Now,
                    Loans = new List<Loan>()
                }
            };
        }

        public void Dispose()
        {

        }

        [Fact(DisplayName = "Validere kopiens status")]
        public void TC3_3()
        {
            //Arrange
            Loan loan = new Loan();
            var loanRepos = new Mock<ILoanRepos<Loan>>();
            var cardRepos = new Mock<ICardRepos<Card>>();
            var userRepos = new Mock<IUserRepos<Person>>();
            loanRepos.Setup(x => x.CheckForStatusOnCopy(It.IsAny<Copy>())).Returns(false);
            loanRepos.Setup(x => x.Create(loan)).Returns(loan);
            cardRepos.Setup(x => x.Get(It.IsAny<Card>())).Returns(Person.Card);
            userRepos.Setup(x => x.GetUserByCardID(1)).Returns(Person);
            userRepos.Setup(x => x.GetLibrarianById(1)).Returns(Lib);
            var lc = new LoanLogic(loanRepos.Object, cardRepos.Object, userRepos.Object);

            //Act
            var createdLoan = lc.CreateLoan(1, 5, 0, 0, 0, 0, 1);

            //Assert
            Assert.NotNull(createdLoan.ErrorMessage);
        }

        [Fact(DisplayName = "Opret lån integration")]
        public void TC1_4()
        {
            //Arrange
            var loanRepos = new Mock<ILoanRepos<Loan>>();
            var cardRepos = new Mock<ICardRepos<Card>>();
            var userRepos = new Mock<IUserRepos<Person>>();
            loanRepos.Setup(x => x.CheckForStatusOnCopy(It.IsAny<Copy>())).Returns(true);
            loanRepos.Setup(x => x.Create(It.IsAny<Loan>())).Returns((Loan loan) => loan);
            cardRepos.Setup(x => x.Get(It.IsAny<Card>())).Returns(Person.Card);
            userRepos.Setup(x => x.GetUserByCardID(1)).Returns(Person);
            userRepos.Setup(x => x.GetLibrarianById(1)).Returns(Lib);
            var lc = new LoanLogic(loanRepos.Object, cardRepos.Object, userRepos.Object);

            //Act
            var createdLoan = lc.CreateLoan(1, 5, 0, 0, 0, 0, 1);

            //Assert
            Assert.True(createdLoan.StartDate == DateTime.Now.Date);
        }

        [Theory(DisplayName = "Validere afleveringsdato")]
        [InlineData(false, 21)]
        [InlineData(true, 90)]
        public void TC5_1(bool type, int addDays)
        {
            //Arrange
            var lc = new LoanLogic(null, null, null);
            Person.IsProfessor = type;
            var expectedDueDate = DateTime.Now.AddDays(addDays).Date;

            //Act
            var calculatedDueDate = lc.CalculateDueDate(Person, Lib.Library);

            //Assert
            Assert.Equal(expectedDueDate, calculatedDueDate.Date);
        }

        [Fact(DisplayName = "Validere kortets udløbsdato - Forventet: Ikke udløbet")]
        public void TC4_1_False()
        {
            //Arrange
            var lc = new LoanLogic(null, null, null);

            //Act
            var result = lc.IsCardExpired(Person.Card);

            //Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Validere kortets udløbsdato - Forventet: Udløbet")]
        public void TC4_1_True()
        {
            //Arrange
            var lc = new LoanLogic(null, null, null);
            Person.Card.Expires = DateTime.Now.AddDays(-1);

            //Act
            var result = lc.IsCardExpired(Person.Card);

            //Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Validere kortets antal udlånte bøger - Forventet: VALID")]
        [InlineData(0, 5)] //2.3
        [InlineData(1, 4)] //2.4
        [InlineData(2, 3)] //2.5
        [InlineData(3, 2)] //2.6
        [InlineData(4, 1)] //2.7
        public void TC2_3To2_7True(int currentLoans, int countToBorrow)
        {
            //Arrange
            var lc = new LoanLogic(null, null, null);
            for (var i = 1; i <= currentLoans; i++)
            {
                PersonCurrentNoLoans.Card.Loans.Add(new Loan
                {
                    IsActive = true,
                    Copies = new List<Copy>
                    {
                        new Copy { Barcode = i, IsAvailable = false }
                    }
                });
            }

            //Act
            var result = lc.CanBorrow(PersonCurrentNoLoans.Card, countToBorrow);

            //Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Validere kortets antal udlånte bøger - Forventet: INVALID")]
        [InlineData(0, 6)] //2.8
        [InlineData(1, 5)] //2.9
        [InlineData(2, 4)] //2.10
        [InlineData(3, 3)] //2.11
        [InlineData(4, 2)] //2.12
        public void TC2_8To2_13_False(int currentLoans, int countToBorrow)
        {
            //Arrange
            var lc = new LoanLogic(null, null, null);
            for (var i = 1; i <= currentLoans; i++)
            {
                PersonCurrentNoLoans.Card.Loans.Add(new Loan { IsActive = true, Copies = new List<Copy> { new Copy { Barcode = i, IsAvailable = false } } });
            }

            //Act
            var result = lc.CanBorrow(PersonCurrentNoLoans.Card, countToBorrow);

            //Assert
            Assert.False(result);
        }
    }
}
