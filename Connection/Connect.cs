using System.Data.SqlClient;

namespace PaymentServices.Connection
{
public class Connect
    {
        private readonly IConfiguration _config;
        private readonly SqlConnection _connection;

        public Connect(IConfiguration config)
        {
            _config = config;
            _connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        public SqlConnection GetConnectDb()
        {
            return _connection;
        }
    }
}