using MockBookingSystem.Interfaces;
using MockBookingSystem.Models;

namespace MockBookingSystem.Managers
{
    public abstract class BaseManager : IManager
    {
        protected readonly HttpClient _httpClient;
        protected static readonly Dictionary<string, BookingInfo> _bookings = new Dictionary<string, BookingInfo>();
        protected readonly int _lastMinuteLimitationDays = 45;

        protected BaseManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public abstract Task<SearchRes> Search(SearchReq request);

        public virtual async Task<BookRes> Book(BookReq request)
        {
            var bookingCode = GenerateBookingCode();
            var sleepTime = new Random().Next(30, 61);
            var bookingTime = DateTime.Now;

            _bookings[bookingCode] = new BookingInfo
            {
                SleepTime = sleepTime,
                BookingTime = bookingTime,
                SearchType = DetermineSearchType(request.SearchReq)
            };

            return new BookRes
            {
                BookingCode = bookingCode,
                BookingTime = bookingTime
            };
        }

        public virtual async Task<CheckStatusRes> CheckStatus(CheckStatusReq request)
        {
            if (!_bookings.TryGetValue(request.BookingCode, out var bookingInfo))
            {
                return new CheckStatusRes { Status = BookingStatusEnum.Failed };
            }
            
            var elapsedTime = (DateTime.Now - bookingInfo.BookingTime).TotalSeconds;
            if (elapsedTime < bookingInfo.SleepTime)
            {
                return new CheckStatusRes { Status = BookingStatusEnum.Pending };
            }

            if (bookingInfo.SearchType == SearchType.LastMinuteHotels)
            {
                return new CheckStatusRes { Status = BookingStatusEnum.Failed };
            }

            return new CheckStatusRes { Status = BookingStatusEnum.Success };
        }

        protected string GenerateBookingCode()
        {
            return new string(Enumerable.Repeat("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", 6)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        protected SearchType DetermineSearchType(SearchReq searchReq)
        {
            if (searchReq.DepartureAirport == null)
            {
                if ((searchReq.FromDate - DateTime.Now).TotalDays <= _lastMinuteLimitationDays)
                {
                    return SearchType.LastMinuteHotels;
                }
                return SearchType.HotelOnly;
            }
            return SearchType.HotelAndFlight;
        }
    }

    public class BookingInfo
    {
        public int SleepTime { get; set; }
        public DateTime BookingTime { get; set; }
        public SearchType SearchType { get; set; }
    }

    public enum SearchType
    {
        HotelOnly,
        HotelAndFlight,
        LastMinuteHotels
    }
}
