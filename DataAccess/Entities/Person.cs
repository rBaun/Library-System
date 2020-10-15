using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class Person
    {
        public int SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CampusAddress { get; set; }
        public string Email { get; set; }
        public bool IsProfessor { get; set; }
        public string HomeAddress { get; set; }
        public Card Card { get; set; }
        public List<int> Phones { get; set; }
        public int AmountOfLoans { get; set; }
    }
}
