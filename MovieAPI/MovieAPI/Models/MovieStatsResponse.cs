using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAPI.Models
{
    public class MovieStatsResponse : MovieStatsDetail
    {
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
    }
}
