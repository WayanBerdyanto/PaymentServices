using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using PaymentServices.Connection;
using PaymentServices.DAL.Interfaces;
using PaymentServices.Models;

namespace PaymentServices.DAL
{
    public class DetailPaymentDAL : IDetailPayment
    {
        private readonly IConfiguration _config;
        private readonly Connect _conn;

        public DetailPaymentDAL(IConfiguration config)
        {
            _config = config;
            _conn = new Connect(_config);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DetailPayment> GetAll()
        {
            using (SqlConnection conn = _conn.GetConnectDb())
            {
                var strSql = @"SELECT * FROM DetailPayment ORDER BY DetailPaymentId ASC";
                var detailPayment = conn.Query<DetailPayment>(strSql);
                return detailPayment;
            }
        }

        public DetailPayment GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Insert(DetailPayment obj)
        {
            using (SqlConnection conn = _conn.GetConnectDb())
            {
                var strSql = @"INSERT INTO DetailPayment (PaymentMethod, UserName, Amount, DateTopUp) VALUES (@PaymentMethod, @UserName, @Amount, @DateTopUp); select @@IDENTITY";
                var param = new
                {
                    PaymentMethod = obj.PaymentMethod,
                    UserName = obj.UserName,
                    Amount = obj.Amount,
                    DateTopUp = obj.DateTopUp
                };
                try
                {
                    conn.Execute(strSql, param);

                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException(sqlEx.Message);
                }
            }
        }

        public void Update(DetailPayment obj)
        {
            throw new NotImplementedException();
        }
    }
}