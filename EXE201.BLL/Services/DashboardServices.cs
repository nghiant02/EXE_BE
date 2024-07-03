using EXE201.BLL.Interfaces;
using EXE201.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Services
{
    public class DashboardServices : IDashboardServices
    {
        private readonly IRentalOrderRepository _rentalOrderRepository;

        public DashboardServices(IRentalOrderRepository rentalOrderRepository)
        {
            _rentalOrderRepository = rentalOrderRepository;
        }

        public decimal GetTotalRevenueAllTime()
        {
            return _rentalOrderRepository.GetTotalRevenueAllTime();
        }

        public Dictionary<string, decimal> GetMonthlyRevenue2024()
        {
            return _rentalOrderRepository.GetMonthlyRevenue2024();
        }

        public Dictionary<DateTime, decimal> GetRevenueLast7Days()
        {
            return _rentalOrderRepository.GetRevenueLast7Days();
        }
    }
}
