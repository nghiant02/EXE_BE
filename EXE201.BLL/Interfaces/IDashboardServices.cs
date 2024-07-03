using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Interfaces
{
    public interface IDashboardServices
    {
        decimal GetTotalRevenueAllTime();
        Dictionary<string, decimal> GetMonthlyRevenue2024();
        Dictionary<DateTime, decimal> GetRevenueLast7Days();
    }
}
