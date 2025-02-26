using MockBookingSystem.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MockBookingSystem.Managers
{
    public class HotelAndFlightManager : BaseManager
    {
        public HotelAndFlightManager(HttpClient httpClient) : base(httpClient) { }

        public override async Task<SearchRes> Search(SearchReq request)
        {
            var hotelTask = _httpClient.GetAsync($"https://tripx-test-functions.azurewebsites.net/api/SearchHotels?destinationCode={request.Destination}");
            var flightTask = _httpClient.GetAsync($"https://tripx-test-functions.azurewebsites.net/api/SearchFlights?departureAirport={request.DepartureAirport}&arrivalAirport={request.Destination}");

            await Task.WhenAll(hotelTask, flightTask);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
            };

            var hotels = await hotelTask.Result.Content.ReadFromJsonAsync<List<Hotel>>(options);
            var flights = await flightTask.Result.Content.ReadFromJsonAsync<List<Flight>>(options);

            var searchRes = new SearchRes
            {
                Options = hotels.SelectMany(h => flights.Select(f => new Option
                {
                    OptionCode = Guid.NewGuid().ToString(),
                    HotelCode = h.HotelCode,
                    FlightCode = f.FlightCode,
                    ArrivalAirport = request.Destination,
                    Price = h.Price + f.Price
                })).ToList()
            };

            return searchRes;
        }
    }
}
