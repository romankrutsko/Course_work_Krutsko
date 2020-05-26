using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportWebAplication.Models
{
    public class Api
    {
        public int results { get; set; }
        public TopScorers[] topscorers { get; set; }
    }
}
