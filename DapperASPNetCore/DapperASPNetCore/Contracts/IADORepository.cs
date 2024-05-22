using DapperASPNetCore.Dto;
using DapperASPNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Contracts
{
	public interface IADORepository
    {
		public Task<(bool?, string)> ExecCmdAdv(string strQuery);

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
	}
}
