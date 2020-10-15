using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace WebApp.Models
{
    public class CreateLoanViewModel
    {
        public int Card_ID { get; set; }
        public int Barcode1 { get; set; }
        public int Barcode2 { get; set; }
        public int Barcode3 { get; set; }
        public int Barcode4 { get; set; }
        public int Barcode5 { get; set; }
        public Loan Loan { get; set; }
    }
}
