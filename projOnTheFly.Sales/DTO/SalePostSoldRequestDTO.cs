﻿using projOnTheFly.Models;

namespace projOnTheFly.Sales.DTO
{
    public class SalePostSoldRequestDTO
    {
        public string Iata { get; set; }
        public string Rab { get; set; }
        public DateTime Schedule { get; set; }
        public List<string> Passengers { get; set; }
        public bool Sold { get; set; }
    }
}