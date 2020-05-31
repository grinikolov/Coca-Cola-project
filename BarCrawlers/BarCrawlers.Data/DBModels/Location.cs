using System;

namespace BarCrawlers.Data.DBModels
{
    public class Location
    {
        public Guid Id { get; set; }
        public string Lattitude { get; set; }
        public string Longtitude { get; set; }
    }
}