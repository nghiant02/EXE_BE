using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.DTOs.UserDTOs
{
    public class UpdateProfileUserDTO
    {
        public required string UserName { get; set; }
        public required string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string? ProfileImage { get; set; }
    }
}
