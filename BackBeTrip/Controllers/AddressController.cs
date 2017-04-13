using BackBeTrip.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace BackBeTrip.Controllers
{
    public class AddressController : ApiController
    {
        // GET: Address
        public HttpResponseMessage Get(string from, string to, int radius, string types)
        {
            WayPoint fromWp = GoogleController.GetAddress(from);
            WayPoint toWp = GoogleController.GetAddress(to);

            List<Place> waypoints = RouteController.CreateNiceRoute(fromWp, toWp, radius, types.Split(',').ToList());

            var resp = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(waypoints))
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }

    }
}