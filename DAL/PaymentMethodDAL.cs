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
    public class PaymentMethodDAL : IPaymentMethod
    {

        private readonly IConfiguration _config;
        private readonly Connect _conn;

        public PaymentMethodDAL(IConfiguration config)
        {
            _config = config;
            _conn = new Connect(_config);
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = _conn.GetConnectDb())
            {
                var strSql = @"DELETE FROM PaymentMethod WHERE IdPayment = @IdPayment";
                try
                {
                    conn.Execute(strSql, new { IdPayment = id });
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
            }
        }

        public IEnumerable<PaymentMethod> GetAll()
        {
            using (SqlConnection conn = _conn.GetConnectDb())
            {
                var strSql = @"SELECT * FROM PaymentMethod";
                var payment = conn.Query<PaymentMethod>(strSql);
                return payment;
            }
        }

        public PaymentMethod GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Insert(PaymentMethod obj)
        {
            using (SqlConnection conn = _conn.GetConnectDb())
            {
                var strSql = @"INSERT INTO PaymentMethod (NamePayment) VALUES (@NamePayment); SELECT CAST(SCOPE_IDENTITY() as int);";
                var param = new { NamePayment = obj.NamePayment };
                try
                {
                    obj.IdPayment = conn.Query<int>(strSql, param).Single();
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
            }
        }

        public void Update(PaymentMethod obj)
        {
            using (SqlConnection conn = _conn.GetConnectDb())
            {
                var strSql = @"UPDATE PaymentMethod SET NamePayment = @NamePayment";
                var param = new { IdPayment = obj.IdPayment, NamePayment = obj.NamePayment };
                try
                {
                    var newId = conn.ExecuteScalar<int>(strSql, param);
                    obj.IdPayment = newId;
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
            }
        }
    }
}