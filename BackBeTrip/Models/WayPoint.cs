using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BackBeTrip.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WayPoint
    {
        [JsonProperty("lng")]
        public double Long { get; set; } // долгота 
        [JsonProperty("lat")]
        public double Lat { get; set; } // широта 

        public WayPoint()
        {

        }

        public WayPoint(double Lat, double Long)
        {
            this.Lat = Lat;
            this.Long = Long;
        }

        public override string ToString()
        {
            return Lat.ToString(CultureInfo.InvariantCulture) + "," + Long.ToString(CultureInfo.InvariantCulture);
        }
    }

}