using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DapperASPNetCore.Context
{
	public class CREDITContext
	{
		private readonly IConfiguration _configuration;
		private readonly string _connectionString;

		public CREDITContext(IConfiguration configuration)
		{
			_configuration = configuration;
			_connectionString = _configuration.GetConnectionString("CRESqlConnection");
		}

		public IDbConnection CreateConnection()
			=> new SqlConnection(_connectionString);
	}
}
