using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ReturnBookViewModel
    {
        public int BookBarcode { get; set; }
        public bool ButtonClicked { get; set; }
        public bool Success { get; set; }
    }
}
