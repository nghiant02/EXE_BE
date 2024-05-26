using EXE201.BLL.Interfaces;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Services
{
    public class ForgotPasswordService : IForgotPawwordService
    {
        private readonly IVerifyCodeRepository _verifyCodeRepository;
        private readonly IUserRepository _userRepository;
        public ForgotPasswordService(IVerifyCodeRepository verifyCodeRepository, IUserRepository userRepository)
        {
            _verifyCodeRepository = verifyCodeRepository;
            _userRepository = userRepository;
        }
        public Task<VerifyCode> AddCode(string code, string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsCodeExist(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> VerifyCode(string code, string email)
        {
            throw new NotImplementedException();
        }
    }
}
