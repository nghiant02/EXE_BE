using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using MCC.DAL.Repository.Implements;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(EXE201Context context) : base(context)
        {
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
