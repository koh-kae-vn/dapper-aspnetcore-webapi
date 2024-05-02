using DapperASPNetCore.Dto;
using DapperASPNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Contracts
{
	public interface ICompanyRepository
	{
		public Task<(bool,string)> ExecCmd(string strQuery);
		public Task<(bool, string)> ExecCmd(string spName, Dictionary<string, object> para, int commandType = 0);


		public Task<(DataRow, string)> ExecReturnDr(string spName, Dictionary<string, object> para, int commandType = 0);
		public Task<(DataRow, string)> ExecReturnDr(string strQuery);

		public Task<(object, string)> ExecReturnValue(string spName, Dictionary<string, object> para, int commandType = 0);
		public Task<(object,string)> ExecReturnValue(string strQuery);

		public Task<(DataTable, string)> ExecReturnDt(string spName, Dictionary<string, object> para, int commandType = 0);
		public Task<(DataTable,string)> ExecReturnDt(string strQuery);

		public Task<(DataSet, string)> ExecReturnDs(string spName, Dictionary<string, object> para, int commandType = 0);

		public Task<(DataSet,string)> ExecReturnDs(string strQuery);


		public Task<IEnumerable<Company>> GetCompanies();
		public Task<Company> GetCompany(int id);
		public Task<Company> CreateCompany(CompanyForCreationDto company);
		public Task UpdateCompany(int id, CompanyForUpdateDto company);
		public Task DeleteCompany(int id);
		public Task<Company> GetCompanyByEmployeeId(int id);
		public Task<Company> GetCompanyEmployeesMultipleResults(int id);
		public Task<List<Company>> GetCompaniesEmployeesMultipleMapping();
		public Task CreateMultipleCompanies(List<CompanyForCreationDto> companies);
	}
}
