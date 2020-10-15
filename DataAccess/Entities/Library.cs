using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class Library
    {
        public string Name { get; set; }
        public List<Rules> Rules { get; set; }

        public Library()
        {
            Rules = new List<Rules>();
        }
    }
}