using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.DTOs.OrderDTOs
{
    public class RentalOrderDto
    {
        public string OrderCode { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime PaymentTime { get; set; }
        public string OrderStatus { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
    }
}
