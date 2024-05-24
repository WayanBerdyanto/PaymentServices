using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentServices.DTO
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public decimal Balance { get; set; }
    }
}