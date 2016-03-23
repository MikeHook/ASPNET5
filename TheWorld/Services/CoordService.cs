using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Services
{
    using System.Net;
    using System.Net.Http;
    using Newtonsoft.Json.Linq;

    public interface ICoordService
    {
        Task<CoordServiceResult> Lookup(string location);
    }

    public class CoordService : ICoordService
    {
        public async Task<CoordServiceResult> Lookup(string location)
        {
            var result = new CoordServiceResult()
            {
                Success = false,
                Message = "Unkown failure looking up coordinates"
            };

            var encodedName = WebUtility.UrlDecode(location);
            var bingKey = Startup.Configuration["TheWorld:BingKey"];
            var url = $"http://dev.virtualearth.net/REST/v1/Locations?q={encodedName}&key={bingKey}";

            result.Latitude = 25.774810791015625;
            result.Longitude = -80.1977310180664;
            result.Success = true;
            result.Message = "Success";

            return result;

            var client = new HttpClient();
            var json = await client.GetStringAsync(url);

            // Read out the results
            // Fragile, might need to change if the Bing API changes
            var results = JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];
            if (!resources.HasValues)
            {
                result.Message = $"Could not find '{location}' as a location";
            }
            else
            {
                var confidence = (string)resources[0]["confidence"];
                if (confidence != "High")
                {
                    result.Message = $"Could not find a confident match for '{location}' as a location";
                }
                else
                {
                    var coords = resources[0]["geocodePoints"][0]["coordinates"];
                    result.Latitude = (double)coords[0];
                    result.Longitude = (double)coords[1];
                    result.Success = true;
                    result.Message = "Success";
                }
            }

            return result;
        }
    }
}
