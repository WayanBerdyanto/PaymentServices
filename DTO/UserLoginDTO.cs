using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentServices.DTO
{
    public class UserLoginDTO
    {
        public string UserName { get; set; }
        public string? Password { get; set; }
    }
}