using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class Loan
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public Librarian Librarian { get; set; }
        public Card Card { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<Copy> Copies { get; set; }

        public string ErrorMessage { get; set; }
        public bool IsActive { get; set; }

        public Loan()
        {
            Notifications = new List<Notification>();
            Copies = new List<Copy>();
        }
    }
}
