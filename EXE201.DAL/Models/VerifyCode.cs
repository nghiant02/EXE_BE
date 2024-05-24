using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.Models
{
    public class VerifyCode
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public required string Code { get; set; }
        public required DateTime CreateAt { get; set; }
    }
}
