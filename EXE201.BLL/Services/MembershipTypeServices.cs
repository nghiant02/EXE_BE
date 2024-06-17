using AutoMapper;
using EXE201.BLL.Interfaces;
using EXE201.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Services
{
    public class MembershipTypeServices : IMembershipTypeServices
    {
        private readonly IMembershipTypeRepository _membershipTypeRepository;
        private readonly IMapper _mapper;

        public MembershipTypeServices(IMembershipTypeRepository membershipTypeRepository, IMapper mapper)
        {
            _membershipTypeRepository = membershipTypeRepository;
            _mapper = mapper;
        }
    }
}
