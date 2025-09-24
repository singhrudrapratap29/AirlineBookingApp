using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingApp.Domain.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; } 
        public decimal? DiscountPercent { get; set; } 
        public bool IsActive { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
