using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentServices.DTO
{
    public class InsertDetailPaymentDTO
    {
        public string PaymentMethod { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTopUp { get; set; }
    }
}