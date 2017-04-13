using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackBeTrip.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Place
    {
        public Place()
        {
            Rating = 0;
            Type = "any";
        }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }
        [JsonProperty("location")]
        public WayPoint Location { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("rating")]
        public double Rating { get; set; }
    }
}