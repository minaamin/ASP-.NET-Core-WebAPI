using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAPI.Models
{
    public class MovieStatsDetail
    {
        public int? MovieId { get; set; }
        public Double? AverageWatchDurationS { get; set; }
        public int? Watches { get; set; }
    }
}
