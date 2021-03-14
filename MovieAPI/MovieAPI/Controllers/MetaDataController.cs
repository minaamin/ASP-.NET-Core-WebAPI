using CsvHelper;
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
    public class MetaDataController : ControllerBase
    {
        
        private IRepository _repo;
        public MetaDataController(IRepository repo)
        {           
            _repo = repo;
        }
              

        [HttpGet("{movieId}")]
        public ActionResult<List<MetaDataBase>> Get(int movieId)
        {
            var recordIds = from r in _repo.MetaDataRepo where r.MovieId == movieId && !string.IsNullOrWhiteSpace(r.Title) && !string.IsNullOrWhiteSpace(r.Language)
                            group r by  r.Language  into gr
                           select new { Id = gr.Max(x => x.Id) };

            var records = from r1 in _repo.MetaDataRepo
                          join r2 in recordIds
                          on r1.Id equals r2.Id
                          orderby r1.Language
                          select new MetaDataBase() { MovieId = r1.MovieId, Title = r1.Title, Language = r1.Language, Duration = r1.Duration, ReleaseYear = r1.ReleaseYear };
                                

            if(!records.Any())
            {
                return NotFound();
            }
            
            return records.ToList();
        }

        [HttpPost]
        public ActionResult Post([FromBody] MetaDataBase metadata)
        {
            //find the existing metadata with max id, we need to increment the max id for the new insert into _allRecords
            var recordWithMaxId = _repo.MetaDataRepo.OrderByDescending(_ => _.Id).First();
            var newMetaData = new MetaData { Id = recordWithMaxId.Id + 1, MovieId = metadata.MovieId, Title = metadata.Title, Language = metadata.Language, Duration = metadata.Duration, ReleaseYear = metadata.ReleaseYear };
            _repo.MetaDataRepo.Add(newMetaData);     

            return StatusCode(201);

        }

      
    }
}
