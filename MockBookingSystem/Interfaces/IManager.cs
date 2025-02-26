using MockBookingSystem.Models;

namespace MockBookingSystem.Interfaces
{
    public interface IManager
    {
        Task<SearchRes> Search(SearchReq request);
        Task<BookRes> Book(BookReq request);
        Task<CheckStatusRes> CheckStatus(CheckStatusReq request);
    }
}
