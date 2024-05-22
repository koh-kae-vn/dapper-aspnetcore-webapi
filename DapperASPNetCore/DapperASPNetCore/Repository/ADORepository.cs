using DapperASPNetCore.Context;
using DapperASPNetCore.Contracts;
using DapperASPNetCore.Dto;
using DapperASPNetCore.Entities;
using DapperASPNetCore.Helpers;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DapperASPNetCore.Repository
{
    public class ADORepository : IADORepository
    {
        private readonly ADONetContext _context;

        public ADORepository(ADONetContext context)
        {
            _context = context;
        }
        public async Task<(bool?, string)> ExecCmdAdv(string strQuery)
        {
            try
            {
                using var connection = (SqlConnection)_context.CreateConnection();
                await connection.OpenAsync();

                var cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strQuery;

                var result = await cmd.ExecuteNonQueryAsync();
                if (result > 0)
                    return (true, "");
                else
                    return (null, strQuery);
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
                using var connection = (SqlConnection)_context.CreateConnection();
                await connection.OpenAsync();

                var cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strQuery;

                var result = await cmd.ExecuteNonQueryAsync();
                return (true, "");

            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }

        public async Task<(bool, string)> ExecCmd(string strQuery, Dictionary<string, object> para, int commandType = 0)
        {
            try
            {
                using var connection = (SqlConnection)_context.CreateConnection();
                await connection.OpenAsync();

                var cmd = connection.CreateCommand();
                if (commandType == 0)
                {
                    cmd.CommandType = CommandType.Text;
                }
                else if (commandType == 1)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                cmd.CommandText = strQuery;
                foreach (var item in para.Keys)
                {
                    cmd.Parameters.AddWithValue("@" + item, para[item].ToString());
                }

                var result = await cmd.ExecuteNonQueryAsync();
                return (true, "");

            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }

        public async Task<(DataRow, string)> ExecReturnDr(string strQuery, Dictionary<string, object> para, int commandType = 0)
        {
            try
            {
                using var connection = (SqlConnection)_context.CreateConnection();
                await connection.OpenAsync();

                var cmd = connection.CreateCommand();
                if (commandType == 0)
                {
                    cmd.CommandType = CommandType.Text;
                }
                else if (commandType == 1)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                cmd.CommandText = strQuery;
                foreach (var item in para.Keys)
                {
                    cmd.Parameters.AddWithValue("@" + item, para[item].ToString());
                }
                var result = await cmd.ExecuteReaderAsync();
                DataTable table = new DataTable();
                table.Load(result);
                if (table.Rows.Count > 0)
                {
                    return (table.Rows[0], "");
                }
                return (null, "");
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
                using var connection = (SqlConnection)_context.CreateConnection();
                await connection.OpenAsync();

                var cmd = connection.CreateCommand();
                cmd.CommandText = strQuery;

                IDataReader reader = null;
                reader = await cmd.ExecuteReaderAsync();

                DataTable table = new DataTable();
                table.Load(reader);
                if (table.Rows.Count > 0)
                {
                    return (table.Rows[0], "");
                }
                return (null, "");
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
                using var connection = (SqlConnection)_context.CreateConnection();
                await connection.OpenAsync();

                var cmd = connection.CreateCommand();
                cmd.CommandText = strQuery;
                object obj = null;
                obj = await cmd.ExecuteScalarAsync();

                return (obj, "");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }

        }

        public async Task<(object, string)> ExecReturnValue(string strQuery, Dictionary<string, object> para, int commandType = 0)
        {
            try
            {
                using var connection = (SqlConnection)_context.CreateConnection();
                await connection.OpenAsync();
                var cmd = connection.CreateCommand();
                cmd.CommandText = strQuery;

                if (commandType == 0)
                    cmd.CommandType = CommandType.Text;
                else if (commandType == 1)
                    cmd.CommandType = CommandType.StoredProcedure;
                else if (commandType == 2)
                    cmd.CommandType = CommandType.TableDirect;
                object obj = null;
                obj = await cmd.ExecuteScalarAsync();

                return (obj, "");

            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }

        }

        public async Task<(DataTable, string)> ExecReturnDt(string strQuery, Dictionary<string, object> para, int commandType = 0)
        {
            try
            {
                using var connection = (SqlConnection)_context.CreateConnection();
                await connection.OpenAsync();
                var cmd = connection.CreateCommand();
                cmd.CommandText = strQuery;

                if (commandType == 0)
                    cmd.CommandType = CommandType.Text;
                else if (commandType == 1)
                    cmd.CommandType = CommandType.StoredProcedure;
                else if (commandType == 2)
                    cmd.CommandType = CommandType.TableDirect;
                IDataReader reader = null;
                reader = await cmd.ExecuteReaderAsync();
                DataTable table = new DataTable();
                table.Load(reader);
                return (table, "");
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
                using var connection = (SqlConnection)_context.CreateConnection();
                await connection.OpenAsync();
                var cmd = connection.CreateCommand();
                cmd.CommandText = strQuery;
                IDataReader reader = null;
                reader = await cmd.ExecuteReaderAsync();
                DataTable table = new DataTable();
                table.Load(reader);
                return (table, "");

            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(DataSet, string)> ExecReturnDs(string strQuery, Dictionary<string, object> para, int commandType = 0)
        {
            try
            {
                using var connection = (SqlConnection)_context.CreateConnection();
                await connection.OpenAsync();
                var cmd = connection.CreateCommand();

                if (commandType == 0)
                    cmd.CommandType = CommandType.Text;
                else if (commandType == 1)
                    cmd.CommandType = CommandType.StoredProcedure;
                else if (commandType == 2)
                    cmd.CommandType = CommandType.TableDirect;

                cmd.CommandText = strQuery;
                IDataReader reader = null;
                reader = await cmd.ExecuteReaderAsync();
                var dataset = Helper.ConvertDataReaderToDataSet(reader);
                return (dataset, "");

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
                using var connection = (SqlConnection)_context.CreateConnection();
                await connection.OpenAsync();
                var cmd = connection.CreateCommand();
                cmd.CommandText = strQuery;
                IDataReader reader = null;
                reader = await cmd.ExecuteReaderAsync();
                var dataset = Helper.ConvertDataReaderToDataSet(reader);
                return (dataset, "");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
    }
}
