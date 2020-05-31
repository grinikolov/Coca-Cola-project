using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BarCrawlers.Services.DTOs
{
    public class LocationDTO
    {
        //public Guid? LocationId { get; set; }
        [JsonPropertyName("lat")]
        public string Lat { get; set; }
        [JsonPropertyName("lon")]
        public string Lon { get; set; }
    }
}