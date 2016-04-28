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
    [Route("api/lastfm")]
    public class LastFmController : Controller
    {
        private const string LAST_FM_API_KEY = "b46bf61085abeaaad64c2e795c90e976";
        private const string SCROBBLER_BASE_ADDR = "http://ws.audioscrobbler.com/2.0/";
        private const string SCROBBLER_GET_USER_ADDR = "?method=user.getinfo&user={0}&api_key={1}&format=json";

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return this.HttpBadRequest("User ID cannot be null/empty");
            
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(SCROBBLER_BASE_ADDR);
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

            return string.Format(SCROBBLER_GET_USER_ADDR, userId, LAST_FM_API_KEY);
        }
    }
}
