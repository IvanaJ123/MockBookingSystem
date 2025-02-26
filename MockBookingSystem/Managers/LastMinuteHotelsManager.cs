using MockBookingSystem.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MockBookingSystem.Managers
{
    public class LastMinuteHotelsManager : BaseManager
    {
        public LastMinuteHotelsManager(HttpClient httpClient) : base(httpClient) { }

        public override async Task<SearchRes> Search(SearchReq request)
        {
            var response = await _httpClient.GetAsync($"https://tripx-test-functions.azurewebsites.net/api/SearchHotels?destinationCode={request.Destination}");
            response.EnsureSuccessStatusCode();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
            };
            var hotels = await response.Content.ReadFromJsonAsync<List<Hotel>>(options);

            var searchRes = new SearchRes
            {
                Options = hotels.Select(h => new Option
                {
                    OptionCode = Guid.NewGuid().ToString(),
                    HotelCode = h.HotelCode,
                    FlightCode = "",
                    ArrivalAirport = request.Destination,
                    Price = h.Price * 0.9 // 10% discount for last-minute bookings
                }).ToList()
            };

            return searchRes;
        }
    }
}
