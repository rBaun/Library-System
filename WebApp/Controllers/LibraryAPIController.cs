using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Entities;
using DataAccess.InterfaceRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryAPIController : ControllerBase
    {
        private readonly IGuestLibraryRepos<Person> _guestRepos;
        private readonly ILoanLogic _loanLogic;

        public LibraryAPIController(IGuestLibraryRepos<Person> guestRepos, ILoanLogic loanLogic)
        {
            _guestRepos = guestRepos;
            _loanLogic = loanLogic;
        }

        // GET: api/LibraryAPI/GetPersonByCardId
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetPersonByCardId([FromQuery] int card_id)
        {
            var p = _guestRepos.GetPersonByCardId(card_id);
            return Ok(new { p.Card.ID, p.FirstName, p.LastName, p.IsProfessor });
        }

        // GET: api/LibraryAPI/GetBookCatalog
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetBookCatalog()
        {
            return Ok(_guestRepos.GetCatalogOfBooks());
        }

        // POST: api/LibraryAPI/CreateLoan
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateLoan(int card_id, int barcode1, int barcode2, int barcode3, int barcode4,
            int barcode5, int employee_id)
        {
            var createdLoan =
                _loanLogic.CreateLoan(card_id, barcode1, barcode2, barcode3, barcode4, barcode5, employee_id);
            string response;
            if (string.IsNullOrEmpty(createdLoan.ErrorMessage))
            {
                response = "The loan was created for >>" + createdLoan.Card.ID + "<< and was made by >>" + createdLoan.Librarian.EmployeeID + "<<";
            }
            else
            {
                response = createdLoan.ErrorMessage;
            }
            return Ok(response);
        }

        // GET: api/LibraryAPI/GetTop10ActiveMembers
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetTop10ActiveMembers()
        {
            var response = new List<object>();
            var members = _guestRepos.GetTop10ActiveMembers();
            foreach (var member in members)
                response.Add(new { member.FirstName, member.LastName, member.IsProfessor, member.AmountOfLoans });

            return Ok(response);
        }

        // GET: api/LibraryAPI/GetTop10Books
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetTop10Books()
        {
            var response = new List<object>();
            var books = _guestRepos.GetTop10Books();
            foreach (var book in books)
                response.Add(new { book.ISBN, book.Author, book.Title, book.AmountOfLoans });

            return Ok(response);
        }

        // GET: api/LibraryAPI/GetAverageLoanTime
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAverageLoanTime()
        {
            return Ok("Gennemsnitlige lånetid: " + _guestRepos.GetAverageLoanTime());
        }

        // GET: api/LibraryAPI/GetMemberAverageLoanTime
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetMemberAverageLoanTime([FromQuery] bool isProfessor)
        {
            var response = "Gennemsnitlige lånetid for ";
            if (isProfessor)
                response += "professors: " + _guestRepos.GetMemberAverageLoanTime(true) + " dage";
            else
                response += "students: " + _guestRepos.GetMemberAverageLoanTime(false) + " dage";

            return Ok(response);
        }
    }
}