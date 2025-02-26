namespace MockBookingSystem.Models
{
    public class CheckStatusRes
    {
        public BookingStatusEnum Status { get; set; }
    }

    public enum BookingStatusEnum
    {
        Success,
        Failed,
        Pending
    }
}
