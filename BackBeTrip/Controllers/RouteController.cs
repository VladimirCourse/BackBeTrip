using BackBeTrip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackBeTrip.Controllers
{
    public class RouteController
    {

        public static List <Place> CreateNiceRoute(WayPoint from, WayPoint to, int radius, List <string> types)
        {
            HashSet<Place> places = new HashSet<Place>();

            double dist = GeoController.Distance(from, to);
            double ds = dist / 10.0;
            double cur = ds;

            for (int i = 0; i < 10; i++)
            {
                WayPoint mid = GeoController.OffsetPoint(from, to, cur);
                List<Place> midPlaces = GoogleController.GetAttractions(mid, radius, types);

                foreach (Place place in midPlaces)
                {
                    if (!places.Contains(place))
                    {
                        places.Add(place);
                        break;
                    }
                }
                cur += ds;
            }
            List<Place> lstPlaces = places.ToList();
            return lstPlaces;
        }

    }
}