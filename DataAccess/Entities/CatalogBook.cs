using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class CatalogBook
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string SubjectArea { get; set; }
        public string ISBN { get; set; }
        public string BookDescription { get; set; }
        public bool IsAvailable { get; set; }
        public List<int> AvailableBarcodes { get; set; }
        public List<int> LoanedOutBarcodes { get; set; }
        public string Available { get; set; }
        public string LoanedOut { get; set; }
        public int AmountOfLoans { get; set; }
    }
}
