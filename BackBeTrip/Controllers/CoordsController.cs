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
using System.Globalization;
using System.Web.Http.Cors;

namespace BackBeTrip.Controllers
{

    public class CoordsController : ApiController
    {

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public HttpResponseMessage Get(string from, string to, int radius, string types)
        {
            string [] spl = from.Split(',');
            WayPoint fromWp = new WayPoint(Double.Parse(spl[0], CultureInfo.InvariantCulture), Double.Parse(spl[1], CultureInfo.InvariantCulture));
            spl = to.Split(',');
            WayPoint toWp = new WayPoint(Double.Parse(spl[0], CultureInfo.InvariantCulture), Double.Parse(spl[1], CultureInfo.InvariantCulture));

            List<Place> waypoints = RouteController.CreateNiceRoute(fromWp, toWp, radius, types.Split(',').ToList());
       
            var resp = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(waypoints))
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;         
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
