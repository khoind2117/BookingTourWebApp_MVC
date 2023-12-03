namespace BookingTourWebApp_MVC.ViewModels
{
    public class TicketHistoryViewModel
    {
        public int FlightId { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public int BusinessTickets { get; set; }
        public int EconomyTickets { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
