using BackBeTrip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackBeTrip.Controllers
{
    //Operations with coordinates
    public class GeoController
    {

        private static double EarthRad = 6378000.1;

        private static double ToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        private static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static WayPoint DestByBearing(WayPoint from, double angle, double distance)
        {
            double brng = ToRadians(angle);
            double lat1 = ToRadians(from.Lat);
            double lon1 = ToRadians(from.Long);
            double lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(distance / EarthRad) + Math.Cos(lat1) * Math.Sin(distance / EarthRad) * Math.Cos(brng));
            double lon2 = lon1 + Math.Atan2(Math.Sin(brng) * Math.Sin(distance / EarthRad) * Math.Cos(lat1), Math.Cos(distance / EarthRad) - Math.Sin(lat1) * Math.Sin(lat2));

            return new WayPoint(ToDegrees(lat2), ToDegrees(lon2));
        }

        public static double Bearing(WayPoint from, WayPoint to)
        {
            double y = to.Long - from.Long;
            double x = Math.Log(Math.Tan(to.Lat / 2.0 + Math.PI / 4.0) / Math.Tan(from.Lat / 2.0 + Math.PI / 4.0));
            if (Math.Abs(y) > Math.PI)
            {
                if (y > 0.0)
                {
                    y = -(2.0 * Math.PI - y);
                }
                else
                {
                    y = (2.0 * Math.PI + y);
                }
            }
            double brng = Math.Atan2(y, x);
            brng = ToDegrees(brng);
            brng = (brng + 360) % 360;
            return brng;
        }

        public static WayPoint OffsetPoint(WayPoint from, WayPoint to, double distance)
        {
            //return DestByBearing(from, Bearing(from, to), distance);

            var dLon = ToRadians(to.Long - from.Long);
            var dPhi = Math.Log(
                Math.Tan(ToRadians(to.Lat) / 2 + Math.PI / 4) / Math.Tan(ToRadians(from.Lat) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            var initialBearingRadians = (ToDegrees(Math.Atan2(dLon, dPhi)) + 360) % 360;

            return DestByBearing(from, initialBearingRadians, distance);
           
        }

        public static double Distance(WayPoint from, WayPoint to)
        {
            double lat1 = from.Lat;
            double lon1 = from.Long;
            double lat2 = to.Lat;
            double lon2 = to.Long;

            double theta = ToRadians(lon1 - lon2);
            lat1 = ToRadians(lat1);
            lon1 = ToRadians(lon1);
            lat2 = ToRadians(lat2);
            lon2 = ToRadians(lon2);

            double dist = Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(theta);
            dist = ToDegrees(Math.Acos(dist)) * 60 * 1.1515 * 1.609344 * 1000;

            return dist;
        }

    }
}