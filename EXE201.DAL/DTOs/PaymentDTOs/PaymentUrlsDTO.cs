using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.DTOs.PaymentDTOs
{
    public class PaymentUrlsDTO
    {
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public string PaymentLink { get; set; }
    }

}
