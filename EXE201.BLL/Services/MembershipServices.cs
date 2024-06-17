using AutoMapper;
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
    public class MembershipServices : IMembershipServices

    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IMapper _mapper;

        public MembershipServices(IMembershipRepository membershipRepository, IMapper mapper)
        {
            _membershipRepository = membershipRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Membership>> GetMemberships()
        {
            return await _membershipRepository.GetAll();
        }
    }
}
