using EXE201.DAL.DTOs.UserDTOs;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using LMSystem.Repository.Helpers;
using MCC.DAL.Repository.Implements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<User> AddNewUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetLatestUser()
        {
            return await _context.Users.OrderByDescending(x => x.UserId).FirstOrDefaultAsync();
        }

        public async Task<User> ChangeStatusUserToNotActive(int id)
        {
            var checkUser = await _context.Users.Where(x => x.UserId == id).FirstOrDefaultAsync();
            if (checkUser != null)
            {
                checkUser.AccountStatus = "Inactive";

                _context.Users.Update(checkUser);
                await _context.SaveChangesAsync();
                return checkUser;
            }
            return null;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users
                .Include(x => x.Addresses)
                .Include(x => x.Carts)
                .Include(x => x.Deposits)
                .Include(x => x.Feedbacks)
                .Include(x => x.Memberships)
                .Include(x => x.Notifications)
                .Include(x => x.Payments)
                .Include(x => x.RentalOrders)
                .Include(x => x.Ratings)
                .OrderByDescending(x => x.UserId)
                .ToListAsync();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<User> UpdateUser(User user)
        {
            var existUser = await _context.Users.Where(x => x.UserId.Equals(user.UserId)).FirstOrDefaultAsync();
            if (existUser != null)
            {
                existUser.UserId = user.UserId;
                existUser.UserName = user.UserName;
                existUser.Password = user.Password;
                existUser.FullName = user.FullName;
                existUser.Phone = user.Phone;
                existUser.Gender = user.Gender;
                existUser.Email = user.Email;
                existUser.DateOfBirth = user.DateOfBirth;
                existUser.ProfileImage = user.ProfileImage;
                existUser.AccountStatus = user.AccountStatus;
                existUser.Deposits = user.Deposits;
                existUser.Addresses = user.Addresses;
                existUser.Carts = user.Carts;
                existUser.Feedbacks = user.Feedbacks;
                existUser.Memberships = user.Memberships;
                existUser.Roles = user.Roles;
                existUser.Notifications = user.Notifications;
                existUser.Payments = user.Payments;
                existUser.Ratings = user.Ratings;
                existUser.RentalOrders = user.RentalOrders;

                _context.Users.Update(existUser);
                await _context.SaveChangesAsync();
                return existUser;
            }
            return null;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Role> GetRoleById(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<PagedList<UserListDTO>> GetFilteredUser(UserFilterDTO filter)
        {
            var query = _context.Users
                .Include(u => u.Memberships)
                .ThenInclude(m => m.MembershipType)
                .Select(u => new UserListDTO
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    FullName = u.FullName,
                    Password = u.Password,
                    Phone = u.Phone,
                    Gender = u.Gender,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email,
                    ProfileImage = u.ProfileImage,
                    AccountStatus = u.AccountStatus,
                    MembershipTypeName = u.Memberships.FirstOrDefault().MembershipType.MembershipTypeName
                })
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(u => u.UserName.Contains(filter.Search) || u.FullName.Contains(filter.Search));
            }

            if (filter.DateOfBirth.HasValue)
            {
                query = query.Where(u => u.DateOfBirth == filter.DateOfBirth);
            }

            if (filter.Gender.HasValue)
            {
                query = query.Where(u => u.Gender == filter.Gender);
            }

            if (!string.IsNullOrEmpty(filter.MembershipTypeName))
            {
                query = query.Where(u => u.MembershipTypeName == filter.MembershipTypeName);
            }

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                switch (filter.SortBy.ToLower())
                {
                    case "username":
                        query = filter.Sort ? query.OrderByDescending(u => u.UserName) : query.OrderBy(u => u.UserName);
                        break;
                    case "fullname":
                        query = filter.Sort ? query.OrderByDescending(u => u.FullName) : query.OrderBy(u => u.FullName);
                        break;
                    case "dateofbirth":
                        query = filter.Sort ? query.OrderByDescending(u => u.DateOfBirth) : query.OrderBy(u => u.DateOfBirth);
                        break;
                    case "membershiptypename":
                        query = filter.Sort ? query.OrderByDescending(u => u.MembershipTypeName) : query.OrderBy(u => u.MembershipTypeName);
                        break;
                    default:
                        query = query.OrderBy(u => u.UserId); // Default sort order
                        break;
                }
            }
            else
            {
                query = query.OrderBy(u => u.UserId); // Default sort order
            }

            var users = await query.ToListAsync();
            return PagedList<UserListDTO>.ToPagedList(users, filter.PageNumber, filter.PageSize);
        }
    }
}
