using DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class RareMaterial : Material
    {
        public Rarity Rarity { get; set; }

        public RareMaterial(string title, string author, string subjectArea, Rarity rarity)
            : base(title, author, subjectArea)
        {
            Rarity = rarity;
        }
    }


}
