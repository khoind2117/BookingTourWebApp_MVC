namespace BookingTourWebApp_MVC.Models
{
    public class UserTour
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public int? TourId { get; set; }
        public virtual Tour? Tour { get; set; }
    }
}
