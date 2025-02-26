using MockBookingSystem.Interfaces;
using MockBookingSystem.Models;

namespace MockBookingSystem.Managers
{
    public static class ManagerFactory
    {
        public static IManager CreateManager(SearchReq request, HttpClient httpClient)
        {
            if (request.DepartureAirport == null)
            {
                if ((request.FromDate - DateTime.Now).TotalDays <= 45)
                {
                    return new LastMinuteHotelsManager(httpClient);
                }
                return new HotelOnlyManager(httpClient);
            }
            return new HotelAndFlightManager(httpClient);
        }
    }
}
