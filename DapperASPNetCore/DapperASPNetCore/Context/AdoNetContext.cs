using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DapperASPNetCore.Context
{
	public class ADONetContext
    {
		private readonly IConfiguration _configuration;
		private readonly string _connectionString;

		public ADONetContext(IConfiguration configuration)
		{
			_configuration = configuration;
			_connectionString = _configuration.GetConnectionString("ADOSqlConnection");
		}

		public IDbConnection CreateConnection()
			=> new SqlConnection(_connectionString);
	}
}
