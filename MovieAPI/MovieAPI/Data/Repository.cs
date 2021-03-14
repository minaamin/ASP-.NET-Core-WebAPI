
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace MovieAPI.Models
{
    public interface IRepository
    {
        public List<MetaData> MetaDataRepo { get; }
        public List<MovieStats> StatsRep { get; }
    }
    public class Repository : IRepository
    {
        private IWebHostEnvironment _environment;             
        private List<MetaData> _allRecords = new List<MetaData>();        
        private List<MovieStats> _allStats = new List<MovieStats>();

        List<MetaData> IRepository.MetaDataRepo => _allRecords;
        List<MovieStats> IRepository.StatsRep => _allStats;
        public Repository(IWebHostEnvironment environment)
        {
            _environment = environment;            
            string contentPath = _environment.ContentRootPath;
            string metaDataFilePath = Path.Combine(contentPath, "Data", "metadata.csv");
            string movieStatsFilePath = Path.Combine(contentPath, "Data", "stats.csv");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };

            using (var reader = new StreamReader(metaDataFilePath))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<MetaData>();
                _allRecords = records.ToList();
            }

            using (var reader = new StreamReader(movieStatsFilePath))
            using (var csv = new CsvReader(reader, config))
            {
                var stats = csv.GetRecords<MovieStats>();
                _allStats = stats.ToList();

            }
        }   
    }
}
