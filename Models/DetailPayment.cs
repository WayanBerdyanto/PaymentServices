using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentServices.Models
{
    public class DetailPayment
    {
        public int DetailPaymentID { get; set; }
        public int IdPayment { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTopUp { get; set; }
    }
}