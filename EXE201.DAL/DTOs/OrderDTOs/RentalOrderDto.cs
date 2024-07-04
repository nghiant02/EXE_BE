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
    }
}
