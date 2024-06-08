using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentServices.DTO;
using PaymentServices.Models;

namespace PaymentServices.Services
{
    public interface IUserServices
    {
        Task<IEnumerable<User>> GetAllUser();
        Task<User> GetUserByName(string username);
        Task TopUpBalanceAsync(UserUpdateBalanceDTO userUpdateBalance);
    }
}