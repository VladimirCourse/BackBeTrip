using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Web.Http.Results;

using System.Web.Mvc;
using Newtonsoft.Json;
using BackBeTrip.Models;

namespace BackBeTrip.Controllers
{

    public class ValuesController : ApiController
    {
        // GET api/values
        public HttpResponseMessage Get()
        {

            string key = "AIzaSyAc5uRZlE8iXemDa8OhBLoE0I7M-kI9ZKU";
            // string url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=-33.8670,151.1957&radius=500&types=food&name=cruise&key=" + key;
            string key2 = "AIzaSyBUS26j6H336Tzz_lllTlOmuU2JHbmmgRk";
            string url = "https://maps.googleapis.com/maps/api/directions/json?origin=Kazan&destination=Moscow&key=" + key2;
            WebRequest request = WebRequest.Create(url);

            WebResponse response = request.GetResponse();

            Stream data = response.GetResponseStream();

            StreamReader reader = new StreamReader(data);

            // json-formatted string from maps api
            string responseFromServer = reader.ReadToEnd();
            response.Close();
            System.Diagnostics.Debug.WriteLine(responseFromServer);

            dynamic obj = JsonConvert.DeserializeObject(responseFromServer);
            JArray rsp = obj.routes[0].legs[0].steps;

            List<WayPoint> waypoints = new List<WayPoint>();

            for (int i = 0; i < rsp.Count; i++)
            {
                dynamic t = rsp[i];

                WayPoint tmp = JsonConvert.DeserializeObject<WayPoint>(t.end_location.ToString());
                waypoints.Add(tmp);
            }
            //   System.Diagnostics.Debug.WriteLine(rsp.ToString());

            waypoints = new List<WayPoint>();
            WayPoint t1 = new WayPoint();
            t1.Long = 47.5345;
            t1.Lat = 56.3758;


            waypoints.Add(t1);

         
            var resp = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(waypoints))
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
              
            // System.Diagnostics.Debug.WriteLine(responseFromServer);

            // return Json(responseFromServer, JsonRequestBehavior.AllowGet);

          /*  return new JsonResult()
            {
                Data = waypoints,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };*/
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
