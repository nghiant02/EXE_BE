using AutoMapper;
using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs.RentalOrderDTOs;
using EXE201.DAL.DTOs;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RentalOrderDetailServices(IUserRepository userRepository,
            IRentalOrderDetailRepository rentalOrderDetailRepository, IMapper mapper)
        {
            _rentalOrderDetailRepository = rentalOrderDetailRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<RentalOrderDetail> GetRentalOrderDetailById(int id)
        {
            try
            {
                var checkOrderId = await _rentalOrderDetailRepository.GetRentalOrderDetail(id);
                if (checkOrderId == null)
                {
                    throw new ArgumentException($"OrderId {id} is does not exist");
                }

                return checkOrderId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PagingResponse> GetRentalOrderByStaff(int pageNumber, int pageSize)
        {
            var listInventoriesRentalOrderByStaff = await _rentalOrderDetailRepository.GetRentalOrderByStaff(pageNumber, pageSize);
            var rentalOrders = new PagingResponse
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecord = listInventoriesRentalOrderByStaff.Item1,
                TotalPage = listInventoriesRentalOrderByStaff.Item2,
                Data = listInventoriesRentalOrderByStaff.Item3
            };
            return rentalOrders;
        }

        public Task<IEnumerable<ViewRentalOrderDetail>> GetRentalOrderDetailByUserId(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<PagedResponseDTO<RentalOrderDetailResponseDTO>> GetPagedRentalOrderDetailsByUserId(int userId,
            int pageNumber, int pageSize)
        {
            return await _rentalOrderDetailRepository.GetPagedRentalOrderDetailsByUserId(userId, pageNumber, pageSize);
        }
    }
}