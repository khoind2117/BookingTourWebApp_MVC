﻿using System.ComponentModel.DataAnnotations;

namespace BookingTourWebApp_MVC.ViewModels.VMofStatistical
{
    public class StatisticalTotalSaleMonth
    {
        [Key]
        public int Id { get; set; }
        public int CountFlight { get; set; }
        public decimal TotalSales { get; set; }
    }
}
