using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class Card
    {
        public int ID { get; set; }
        public DateTime Expires { get; set; }
        public string PhotoURL { get; set; }
        public List<Loan> Loans { get; set; }

        public Card()
        {
            Loans = new List<Loan>();
        }
    }
}