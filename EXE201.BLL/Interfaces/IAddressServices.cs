﻿using EXE201.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Interfaces
{
    public interface IAddressServices
    {
        Task<IEnumerable<Address>> GetAddressByUserIdAsync(int userId);
        Task<IEnumerable<Address>> GetAllAddressAsync();
    }
}
