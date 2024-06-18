using EXE201.DAL.Models;
using MCC.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXE201.DAL.DTOs.UserDTOs;

namespace EXE201.DAL.Interfaces
{
    public interface IMembershipRepository : IGenericRepository<MembershipPolicy>
    {
        Task<IEnumerable<MembershipPolicy>> GetAll();
        Task<MembershipUserDto> GetMembershipByUserId(int userId);
    }
}
