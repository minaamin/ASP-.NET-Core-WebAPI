using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
       
        private IRepository _repo;
        public MoviesController(IRepository repo)
        {
            _repo = repo;           
        }

        [HttpGet("stats")]
        public IEnumerable<MovieStatsResponse> Stats()
        {
            var movieStatsDetail = _repo.StatsRep.GroupBy(s => new { s.MovieId }).
                            Select(gs => new MovieStatsDetail { MovieId = gs.Key.MovieId, AverageWatchDurationS = gs.Average(_ => _.WatchDurationMs)/1000, Watches = gs.Count() }).ToList();

          
            var distinctMovieIds = _repo.MetaDataRepo.Select(r => r.MovieId).Distinct();

            var movieStatsResponse = new List<MovieStatsResponse>();
            foreach(var movieId in distinctMovieIds)
            {
                var statsDetail = movieStatsDetail.FirstOrDefault(_ => _.MovieId == movieId);
                var metaData = _repo.MetaDataRepo.FirstOrDefault(_ => _.MovieId == movieId);
                var msr = new MovieStatsResponse {
                    MovieId = movieId, 
                    AverageWatchDurationS =Math.Round((statsDetail?.AverageWatchDurationS ?? 0)),
                    Watches = (statsDetail?.Watches ?? 0),
                    Title = metaData.Title,
                    ReleaseYear = metaData.ReleaseYear
                };

                movieStatsResponse.Add(msr);
            }

            movieStatsResponse.OrderBy(_ => _.Watches).OrderBy(_ => _.ReleaseYear);

            return movieStatsResponse;
        }
    }
}
