using Microsoft.AspNetCore.Mvc;
using BhagiRadhaSwayamKrushi.Data;
using BhagiRadhaSwayamKrushi.Models;
using Microsoft.EntityFrameworkCore;

namespace BhagiRadhaSwayamKrushi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IHttpClientFactory _httpClientFactory;

        public LocationController(AppDbContext db, IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _httpClientFactory = httpClientFactory;
        }

        // 🔹 Existing Search API
        [HttpGet("search")]
        public async Task<IActionResult> SearchLocation(string query)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("PureDropApp/1.0 (bskwater7@gmail.com)");
                client.Timeout = TimeSpan.FromSeconds(10);

                // Replace with your delivery area logic if needed
                var url = $"https://nominatim.openstreetmap.org/search?format=json&q={query}&limit=5&addressdetails=1";

                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return BadRequest(new { message = "Search failed" });

                var content = await response.Content.ReadAsStringAsync();
                return Content(content, "application/json");
            }
            catch
            {
                return StatusCode(500, new { message = "Location service unavailable" });
            }
        }

        // 🔹 Existing Reverse Geocoding API
        [HttpGet("reverse")]
        public async Task<IActionResult> Reverse(double lat, double lon)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("PureDropApp/1.0 (bskwater7@gmail.com)");
                client.Timeout = TimeSpan.FromSeconds(10);

                var url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={lat}&lon={lon}";

                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return BadRequest(new { message = "Unable to fetch address" });

                var content = await response.Content.ReadAsStringAsync();
                return Content(content, "application/json");
            }
            catch
            {
                return StatusCode(500, new { message = "Location service unavailable" });
            }
        }

        // 🔹 New API: Get Order's User Location for Tracking
        [HttpGet("order-location/{orderId}")]
        public async Task<IActionResult> GetOrderLocation(int orderId)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null) return NotFound(new { message = "Order not found" });

            if (order.Latitude.HasValue && order.Longitude.HasValue)
            {
                return Ok(new
                {
                    orderId = order.Id,
                    name = order.Name,
                    mobileNumber = order.MobileNumber,
                    address = order.Address,
                    lat = order.Latitude.Value,
                    lng = order.Longitude.Value
                });
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("PureDropApp/1.0 (bskwater7@gmail.com)");
            client.Timeout = TimeSpan.FromSeconds(10);

            var url = $"https://nominatim.openstreetmap.org/search?format=json&q={order.Address}&limit=1";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return BadRequest(new { message = "Unable to fetch coordinates" });

            var content = await response.Content.ReadAsStringAsync();
            var results = System.Text.Json.JsonSerializer.Deserialize<List<NominatimResult>>(content);

            if (results == null || results.Count == 0)
                return BadRequest(new { message = "No coordinates found for this address" });

            var firstResult = results[0];
            if (!double.TryParse(firstResult.lat, out double lat) || !double.TryParse(firstResult.lon, out double lng))
                return BadRequest(new { message = "Invalid coordinates returned from geocoding service" });

            order.Latitude = lat;
            order.Longitude = lng;
            await _db.SaveChangesAsync();

            return Ok(new
            {
                orderId = order.Id,
                name = order.Name,
                mobileNumber = order.MobileNumber,
                address = order.Address,
                lat = lat,
                lng = lng
            });
        }


        // Helper class for OpenStreetMap results
        public class NominatimResult
        {
            public string lat { get; set; }
            public string lon { get; set; }
            public string display_name { get; set; }
        }
    }
}
