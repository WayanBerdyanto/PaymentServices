using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentServices.DTO
{
    public class UserUpdateBalanceDTO
    {
        public string UserName { get; set; }
        public decimal Balance { get; set; }
    }
}