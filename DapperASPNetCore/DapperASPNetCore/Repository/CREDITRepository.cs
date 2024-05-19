using Dapper;
using DapperASPNetCore.Context;
using DapperASPNetCore.Contracts;
using DapperASPNetCore.Dto;
using DapperASPNetCore.Entities;
using DapperASPNetCore.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DapperASPNetCore.Repository
{
    public class CREDITRepository : ICreditRepository
    {
        private readonly CREDITContext _context;

        public CREDITRepository(CREDITContext context)
        {
            _context = context;
        }
        public async Task<(bool?, string)> ExecCmdAdv(string strQuery)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    int value = await connection.ExecuteAsync(strQuery, commandType: CommandType.Text);
                    if (value > 0)
                        return (true, "");
                    else
                        return (null, strQuery);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }

        public async Task<(bool, string)> ExecCmd(string strQuery)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    int value = await connection.ExecuteAsync(strQuery, commandType: CommandType.Text);
                    return (true, "");
                    //if (value > 0)
                    //	return true;
                    //else
                    //	return false;
                }

            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }

        public async Task<(bool, string)> ExecCmd(string spName, Dictionary<string, object> para, int commandType = 0)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var dmPara = new DynamicParameters();
                    foreach (var item in para.Keys)
                    {
                        dmPara.Add(item, para[item].ToString());
                    }
                    int value = 0;
                    if (commandType == 0)
                    {
                        value = await connection.ExecuteAsync(spName, dmPara, commandType: CommandType.Text);
                    }
                    else if (commandType == 1)
                    {
                        value = await connection.ExecuteAsync(spName, dmPara, commandType: CommandType.StoredProcedure);
                    }

                    return (true, "");
                    //if (value > 0)
                    //	return true;
                    //else
                    //	return false;
                }

            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }

        public async Task<(DataRow, string)> ExecReturnDr(string spName, Dictionary<string, object> para, int commandType = 0)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var dmPara = new DynamicParameters();
                    foreach (var item in para.Keys)
                    {
                        dmPara.Add(item, para[item].ToString());
                    }
                    IDataReader reader = null;
                    if (commandType == 0)
                    {
                        reader = await connection.ExecuteReaderAsync(spName, dmPara, commandType: CommandType.Text);
                    }
                    else if (commandType == 1)
                    {
                        reader = await connection.ExecuteReaderAsync(spName, dmPara, commandType: CommandType.StoredProcedure);
                    }
                    DataTable table = new DataTable();
                    table.Load(reader);
                    if (table.Rows.Count > 0)
                    {
                        return (table.Rows[0], "");
                    }
                    return (null, "");
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(DataRow, string)> ExecReturnDr(string strQuery)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    IDataReader reader = null;
                    reader = await connection.ExecuteReaderAsync(strQuery, commandType: CommandType.Text);

                    DataTable table = new DataTable();
                    table.Load(reader);
                    if (table.Rows.Count > 0)
                    {
                        return (table.Rows[0], "");
                    }
                    return (null, "");
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }

        }

        public async Task<(object, string)> ExecReturnValue(string strQuery)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {

                    object obj = null;
                    obj = await connection.ExecuteScalarAsync(strQuery, commandType: CommandType.Text);

                    return (obj, "");
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }

        }

        public async Task<(object, string)> ExecReturnValue(string spName, Dictionary<string, object> para, int commandType = 0)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var dmPara = new DynamicParameters();
                    foreach (var item in para.Keys)
                    {
                        dmPara.Add(item, para[item].ToString());
                    }
                    object obj = null;
                    if (commandType == 0)
                    {
                        obj = await connection.ExecuteScalarAsync(spName, dmPara, commandType: CommandType.Text);
                    }
                    else if (commandType == 1)
                    {
                        obj = await connection.ExecuteScalarAsync(spName, dmPara, commandType: CommandType.StoredProcedure);
                    }
                    return (obj, "");
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }

        }

        public async Task<(DataTable, string)> ExecReturnDt(string spName, Dictionary<string, object> para, int commandType = 0)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var dmPara = new DynamicParameters();
                    foreach (var item in para.Keys)
                    {
                        dmPara.Add(item, para[item].ToString());
                    }
                    IDataReader reader = null;
                    if (commandType == 0)
                    {
                        reader = await connection.ExecuteReaderAsync(spName, dmPara, commandType: CommandType.Text);
                    }
                    else if (commandType == 1)
                    {
                        reader = await connection.ExecuteReaderAsync(spName, dmPara, commandType: CommandType.StoredProcedure);
                    }
                    DataTable table = new DataTable();
                    table.Load(reader);
                    return (table, "");
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }

        }

        public async Task<(DataTable, string)> ExecReturnDt(string strQuery)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    IDataReader reader = null;
                    reader = await connection.ExecuteReaderAsync(strQuery, commandType: CommandType.Text);

                    DataTable table = new DataTable();
                    table.Load(reader);
                    return (table, "");
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(DataSet, string)> ExecReturnDs(string spName, Dictionary<string, object> para, int commandType = 0)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var dmPara = new DynamicParameters();
                    foreach (var item in para.Keys)
                    {
                        dmPara.Add(item, para[item].ToString());
                    }
                    IDataReader reader = null;
                    if (commandType == 0)
                    {
                        reader = await connection.ExecuteReaderAsync(spName, dmPara, commandType: CommandType.Text);
                    }
                    else if (commandType == 1)
                    {
                        reader = await connection.ExecuteReaderAsync(spName, dmPara, commandType: CommandType.StoredProcedure);
                    }
                    var dataset = Helper.ConvertDataReaderToDataSet(reader);
                    return (dataset, "");
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(DataSet, string)> ExecReturnDs(string strQuery)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    IDataReader reader = null;
                    reader = await connection.ExecuteReaderAsync(strQuery, commandType: CommandType.Text);

                    var dataset = Helper.ConvertDataReaderToDataSet(reader);
                    return (dataset, "");
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            var query = "SELECT Id, Name, Address, Country FROM Companies";

            using (var connection = _context.CreateConnection())
            {
                var companies = await connection.QueryAsync<Company>(query);
                return companies.ToList();
            }
        }

        public async Task<Company> GetCompany(int id)
        {
            var query = "SELECT * FROM Companies WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                var company = await connection.QuerySingleOrDefaultAsync<Company>(query, new { id });

                return company;
            }
        }

        public async Task<Company> CreateCompany(CompanyForCreationDto company)
        {
            var query = "INSERT INTO Companies (Name, Address, Country) VALUES (@Name, @Address, @Country)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var createdCompany = new Company
                {
                    Id = id,
                    Name = company.Name,
                    Address = company.Address,
                    Country = company.Country
                };

                return createdCompany;
            }
        }

        public async Task UpdateCompany(int id, CompanyForUpdateDto company)
        {
            var query = "UPDATE Companies SET Name = @Name, Address = @Address, Country = @Country WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteCompany(int id)
        {
            var query = "DELETE FROM Companies WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

        public async Task<Company> GetCompanyByEmployeeId(int id)
        {
            var procedureName = "ShowCompanyForProvidedEmployeeId";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                var company = await connection.QueryFirstOrDefaultAsync<Company>
                    (procedureName, parameters, commandType: CommandType.StoredProcedure);

                return company;
            }
        }

        public async Task<Company> GetCompanyEmployeesMultipleResults(int id)
        {
            var query = "SELECT * FROM Companies WHERE Id = @Id;" +
                        "SELECT * FROM Employees WHERE CompanyId = @Id";

            using (var connection = _context.CreateConnection())
            using (var multi = await connection.QueryMultipleAsync(query, new { id }))
            {
                var company = await multi.ReadSingleOrDefaultAsync<Company>();
                if (company != null)
                    company.Employees = (await multi.ReadAsync<Employee>()).ToList();

                return company;
            }
        }

        public async Task<List<Company>> GetCompaniesEmployeesMultipleMapping()
        {
            var query = "SELECT * FROM Companies c JOIN Employees e ON c.Id = e.CompanyId";

            using (var connection = _context.CreateConnection())
            {
                var companyDict = new Dictionary<int, Company>();

                var companies = await connection.QueryAsync<Company, Employee, Company>(
                    query, (company, employee) =>
                    {
                        if (!companyDict.TryGetValue(company.Id, out var currentCompany))
                        {
                            currentCompany = company;
                            companyDict.Add(currentCompany.Id, currentCompany);
                        }

                        currentCompany.Employees.Add(employee);
                        return currentCompany;
                    }
                );

                return companies.Distinct().ToList();
            }
        }

        public async Task CreateMultipleCompanies(List<CompanyForCreationDto> companies)
        {
            var query = "INSERT INTO Companies (Name, Address, Country) VALUES (@Name, @Address, @Country)";

            using (var connection = _context.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var company in companies)
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("Name", company.Name, DbType.String);
                        parameters.Add("Address", company.Address, DbType.String);
                        parameters.Add("Country", company.Country, DbType.String);

                        await connection.ExecuteAsync(query, parameters, transaction: transaction);
                        //throw new Exception();
                    }

                    transaction.Commit();
                }
            }
        }
    }
}
