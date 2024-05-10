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
    public class RentalOrderDetailServices : IRentalOrderDetailServices
    {
        private readonly IRentalOrderDetailRepository _rentalOrderDetailRepository;
        private readonly IMapper _mapper;

        public RentalOrderDetailServices(IRentalOrderDetailRepository rentalOrderDetailRepository, IMapper mapper)
        {
            _rentalOrderDetailRepository = rentalOrderDetailRepository;
            _mapper = mapper;
        }
    }
}
