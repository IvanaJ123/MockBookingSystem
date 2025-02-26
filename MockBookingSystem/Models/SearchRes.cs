namespace MockBookingSystem.Models
{
    public class SearchRes
    {
        public List<Option> Options { get; set; } = new List<Option>();
    }

    public class Option
    {
        public string OptionCode { get; set; }
        public string HotelCode { get; set; }
        public string FlightCode { get; set; }
        public string ArrivalAirport { get; set; }
        public double Price { get; set; }
    }
}
