using System;

namespace BarCrawlers.Data.DBModels
{
    public class Location
    {
        public Guid Id { get; set; }
        public double Lattitude { get; set; }
        public double Longtitude { get; set; }
    }
}