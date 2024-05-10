using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using MCC.DAL.Repository.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.Repository
{
    public class CartRepostiory : GenericRepository<Cart>, ICartRepository
    {
        public CartRepostiory(EXE201Context context) : base(context)
        {
        }

    }
}
