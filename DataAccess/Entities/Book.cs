using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class Book : Material
    {
        public int ISBN { get; set; }
        public string BookDescription { get; set; }
        public List<Copy> Copies { get; set; }
        public int AmountOfLoans { get; set; }

        public Book(string author, string title, string subjectArea, int ISBN)
            : base(author, title, subjectArea)
        {
            this.ISBN = ISBN;
            Copies = new List<Copy>();
        }

        public Book(string author, string title, string subjectArea, string bookDescription)
             : base(author, title, subjectArea)
        {
            this.BookDescription = bookDescription;
            Copies = new List<Copy>();
        }

        public Book(int ISBN) {
            this.ISBN = ISBN;
            Copies = new List<Copy>();
        }

        public Book()
        {
            Copies = new List<Copy>();
        }
    }
}