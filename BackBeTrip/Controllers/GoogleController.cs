using BackBeTrip.Models;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace BackBeTrip.Controllers
{
 
    public class GoogleController
    {
        private static string keyPlaces = "AIzaSyAc5uRZlE8iXemDa8OhBLoE0I7M-kI9ZKU";
        private static string keyDirections = "AIzaSyBUS26j6H336Tzz_lllTlOmuU2JHbmmgRk";
        private static string keyGeocoding = "AIzaSyDEcYIVNxT6bSakYG9NhS3NjxfejjE10_A";

        private static string places = "amusement_park|aquarium|art_gallery|casino|church|city_hall|courthouse|hindu_temple|library|museum|night_club|park|stadium|synagogue|university|zoo";

        public static List <WayPoint> GetRoute(WayPoint from, WayPoint to, List <WayPoint> places)
        {
            string[] waypoints = new string[places.Count];
            for (int i = 0; i < places.Count; i++)
            {
                waypoints[i] = places[i].ToString();
            }

            string url = "https://maps.googleapis.com/maps/api/directions/json?origin={0}&destination={1}&waypoints={2}&key={3}";

            string address = string.Format(
                from.ToString(),
                to.ToString(),
                String.Join("|", waypoints),
                keyDirections
            );

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader reader = new StreamReader(data);
            string responseFromServer = reader.ReadToEnd();
            response.Close();

            dynamic obj = JsonConvert.DeserializeObject(responseFromServer);

            //TODO: End it
            //Route contains array of legs
            //Each leg contain array of steps
            //Each step contain start and end point
            //Look at google api

            //Example for one leg
            JArray rsp = obj.routes[0].legs[0].steps;

            List<WayPoint> res = new List<WayPoint>();

            for (int i = 0; i < rsp.Count; i++)
            {
                dynamic t = rsp[i];
                WayPoint tmp = JsonConvert.DeserializeObject<WayPoint>(t.end_location.ToString());
                res.Add(tmp);
            }

            return res;
        }
        

        public static WayPoint GetAddress(string addr)
        {
            string url = "https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}";
            string address = string.Format(
                url,
                addr,
                keyGeocoding
            );

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(address);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader reader = new StreamReader(data);
            string responseFromServer = reader.ReadToEnd();
            response.Close();

            WayPoint res = new WayPoint();

            if ((int)response.StatusCode == 200)
            {
                dynamic jsonobj = JsonConvert.DeserializeObject(responseFromServer);
                if (jsonobj.status == "OK")
                {
                    res.Lat = jsonobj.results[0].geometry.location.lat;
                    res.Long = jsonobj.results[0].geometry.location.lng;
                }
            }
            return res;
        }

        // достопримечательности
        public static List <Place> GetAttractions(WayPoint location, int radius, List <string> types)
        {
            string url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?language=RU&location={0}&radius={1}&types={2}&key={3}";
            string address = string.Format(
                url,
                location.ToString(),
                radius,
                String.Join("|", types.ToArray()),
                keyPlaces
            );

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(address);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader reader = new StreamReader(data);
            string responseFromServer = reader.ReadToEnd();
            response.Close();

            List<Place> res = new List<Place>();
            if ((int)response.StatusCode == 200)
            {
                dynamic jsonobj = JsonConvert.DeserializeObject(responseFromServer);
                if (jsonobj.status == "OK")
                {
                    var results = jsonobj.results;
                    foreach (dynamic place in results)
                    {
                        Place tmp = new Place();
                        WayPoint loc = new WayPoint();

                        tmp.PlaceId = place.place_id;
                        tmp.Name = place.name;
                        loc.Lat = place.geometry.location.lat;
                        loc.Long = place.geometry.location.lng;
                        tmp.Location = loc;
                        try
                        {
                            tmp.PhotoId = place.photos[0].photo_reference;
                        }
                        catch {
                            tmp.PhotoId = "noimg";
                        }
                        try
                        {
                            tmp.Rating = place.rating;
                        }
                        catch (RuntimeBinderException) { }
                        try
                        {
                            tmp.Type = place.types[0];
                        }
                        catch (Exception) { }
                        res.Add(tmp);

                    }
                    
                }

            }
            res.Sort((x, y) => (Int32)y.Rating.CompareTo(x.Rating));
            return res;
        }
    }
}