using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.DTOs.UserDTOs
{
    public class UpdateProfileUserDTO
    {
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required int Gender { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }


        // Address fields
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
