using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Scrobblespector.Controllers.api
{
    [Route("api/lastfm/users")]
    public class LastFmUsersController : Controller
    {
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return this.HttpBadRequest("User ID cannot be null/empty");
            
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(SharedConfigs.SCROBBLER_BASE_ADDR);
                HttpResponseMessage response = client.GetAsync(GetUserRequestPath(id)).Result;
                response.EnsureSuccessStatusCode();

                if(response.StatusCode != HttpStatusCode.OK)
                    return Json(new { errorMessage = "Wrong data received from LastFM server." });

                string userJsonString = response.Content.ReadAsStringAsync().Result;
                
                dynamic userJson = JObject.Parse(userJsonString);

                return Json(userJson.user);
            }
        }

        private string GetUserRequestPath(string userId)
        {
            if(string.IsNullOrEmpty(userId))
                throw new ArgumentNullException("userId");

            return string.Format(SharedConfigs.SCROBBLER_GET_USER_PATH, userId);
        }
    }
}
