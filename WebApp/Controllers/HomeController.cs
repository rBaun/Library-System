using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic;
using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Entities;
using DataAccess.InterfaceRepos;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILoanLogic _loanLogic;
        private readonly IBookRepos<Book> _bookRepos;
        private readonly ILoanRepos<Loan> _loanRepos;

        public HomeController(ILoanLogic loanLogic, IBookRepos<Book> bookRepos, ILoanRepos<Loan> loanRepos)
        {
            _loanLogic = loanLogic;
            _bookRepos = bookRepos;
            _loanRepos = loanRepos;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateLoan(CreateLoanViewModel model)
        {
            model.Loan = _loanLogic.CreateLoan(model.Card_ID, model.Barcode1, model.Barcode2, model.Barcode3, model.Barcode4, model.Barcode5, 1);
            return View("Create", model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ReturnBook()
        {
            return View();
        }

        public IActionResult ReturnBookModel(ReturnBookViewModel model)
        {
            model.ButtonClicked = true;
            model.Success = _loanLogic.ReturnBook(model.BookBarcode);
            return View("ReturnBook", model);
        }

        public IActionResult SearchBook(string searchString)
        {
            if(Int32.TryParse(searchString, out int value))
            {
                Book b = new Book(value);
                return View(new List<Book> { _bookRepos.Get(b) });
            }
            return View(_bookRepos.GetBooksBySearchWord(searchString));
        }

        public IActionResult Details(int isbn)
        {
            return View(_bookRepos.Get(new Book(isbn)));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
