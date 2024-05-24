using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentServices.Models;

namespace PaymentServices.DAL.Interfaces
{
    public interface IUsers : ICrud<Users>
    {
        Users? ValidateUser(string username, string password);

        Task UpdateBalancekAsync(string username, decimal balance);

    }
}