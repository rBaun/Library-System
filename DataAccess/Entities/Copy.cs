using System;

namespace DataAccess.Entities
{
    public class Copy
    {
        public int Barcode { get; set; }
        public bool IsAvailable { get; set; }
        public Book Book { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}