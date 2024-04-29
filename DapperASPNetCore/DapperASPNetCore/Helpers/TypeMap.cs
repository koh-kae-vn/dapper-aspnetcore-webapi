using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Helpers
{
    /// <summary>
    ///     Type Map class for database provider specific code
    /// </summary>
    internal abstract class TypeMap
    {
        /// <summary>
        /// Only Non Input Parameters collection
        /// </summary>
        public abstract Dictionary<string, object> NonInputParameterCollection { get; set; }

        /// <summary>
        /// Method to execute the DML via TypeMap
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="dapperParams"></param>
        /// <returns></returns>
        public abstract int Execute(IDbConnection connection,
                                    string sql,
                                    CommandType commandType,
                                    IEnumerable<DapperParam> dapperParams);

        /// <summary>
        /// Method to execute the Select to fetch IEnumerable via TypeMap
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="dapperParams"></param>
        /// <returns></returns>
        public abstract IEnumerable<T> Query<T>(IDbConnection connection,
                                                string sql,
                                                CommandType commandType,
                                                IEnumerable<DapperParam> dapperParams) where T : new();

        /// <summary>
        /// Fetch the relevant TypeMap
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static TypeMap GetTypeMap(string provider)
        {
            TypeMap typeMap = null;

            switch (provider)
            {
                case "System.Data.SqlClient":
                    typeMap = new SqlTypeMap();
                    break;
                default:
                    // SQl Server TypeMap
                    typeMap = new SqlTypeMap();
                    break;
            }

            return (typeMap);
        }
    }

    /// <summary>
    ///     SQL Server provider type map
    /// </summary>
    internal class SqlTypeMap : TypeMap
    {
        public SqlTypeMap()
        {
            NonInputParameterCollection = new Dictionary<string, object>();
        }

        public override sealed Dictionary<string, object> NonInputParameterCollection { get; set; }

        /// <summary>
        ///     Data Type to Db Type mapping dictionary for SQL Server
        /// https://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx
        /// </summary>

        public static readonly Dictionary<Type, SqlDbType> TypeToSqlDbType = new Dictionary<Type, SqlDbType>
            {
              // Mapping C# types to Ado.net SqlDbType enumeration
                {typeof (byte), SqlDbType.TinyInt},
                {typeof (sbyte), SqlDbType.TinyInt},
                {typeof (short), SqlDbType.SmallInt},
                {typeof (ushort), SqlDbType.SmallInt},
                {typeof (int), SqlDbType.Int},
                {typeof (uint), SqlDbType.Int},
                {typeof (long), SqlDbType.BigInt},
                {typeof (ulong), SqlDbType.BigInt},
                {typeof (float), SqlDbType.Float},
                {typeof (double), SqlDbType.Float},
                {typeof (decimal), SqlDbType.Decimal},
                {typeof (bool), SqlDbType.Bit},
                {typeof (string), SqlDbType.VarChar},
                {typeof (char), SqlDbType.Char},
                {typeof (Guid), SqlDbType.UniqueIdentifier},
                {typeof (DateTime), SqlDbType.DateTime},
                {typeof (DateTimeOffset), SqlDbType.DateTimeOffset},
                {typeof (byte[]), SqlDbType.VarBinary},
                {typeof (byte?), SqlDbType.TinyInt},
                {typeof (sbyte?), SqlDbType.TinyInt},
                {typeof (short?), SqlDbType.SmallInt},
                {typeof (ushort?), SqlDbType.SmallInt},
                {typeof (int?), SqlDbType.Int},
                {typeof (uint?), SqlDbType.Int},
                {typeof (long?), SqlDbType.BigInt},
                {typeof (ulong?), SqlDbType.BigInt},
                {typeof (float?), SqlDbType.Float},
                {typeof (double?), SqlDbType.Float},
                {typeof (decimal?), SqlDbType.Decimal},
                {typeof (bool?), SqlDbType.Bit},
                {typeof (char?), SqlDbType.Char},
                {typeof (Guid?), SqlDbType.UniqueIdentifier},
                {typeof (DateTime?), SqlDbType.DateTime},
                {typeof (DateTimeOffset?), SqlDbType.DateTimeOffset},
                //{typeof (System.Data.Linq.Binary), SqlDbType.Binary},
                {typeof (IEnumerable<>), SqlDbType.Structured},
                {typeof (List<>), SqlDbType.Structured},
                {typeof (DataTable), SqlDbType.Structured},

            };

        public override int Execute(IDbConnection connection, string sql, CommandType commandType, IEnumerable<DapperParam> dapperParams)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<T> Query<T>(IDbConnection connection, string sql, CommandType commandType, IEnumerable<DapperParam> dapperParams)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class Map
    {
        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<Type, DbType> TypeToDbType = new Dictionary<Type, DbType>()
        {
            {typeof (byte), DbType.Byte},
            {typeof (sbyte), DbType.Byte},
            {typeof (short), DbType.Int16},
            {typeof (ushort), DbType.Int16},
            {typeof (int), DbType.Int32},
            {typeof (uint), DbType.Int32},
            {typeof (long), DbType.Int64},
            {typeof (ulong), DbType.Int64},
            {typeof (float), DbType.Single},
            {typeof (double), DbType.Double},
            {typeof (decimal), DbType.Decimal},
            {typeof (bool), DbType.Boolean},
            {typeof (string), DbType.String},
            {typeof (char), DbType.StringFixedLength},
            {typeof (Guid), DbType.Guid},
            {typeof (DateTime), DbType.DateTime},
            {typeof (DateTimeOffset), DbType.DateTimeOffset},
            {typeof (byte[]), DbType.Binary},
            {typeof (byte?), DbType.Byte},
            {typeof (sbyte?), DbType.Byte},
            {typeof (short?), DbType.Int16},
            {typeof (ushort?), DbType.Int16},
            {typeof (int?), DbType.Int32},
            {typeof (uint?), DbType.Int32},
            {typeof (long?), DbType.Int64},
            {typeof (ulong?), DbType.Int64},
            {typeof (float?), DbType.Single},
            {typeof (double?), DbType.Double},
            {typeof (decimal?), DbType.Decimal},
            {typeof (bool?), DbType.Boolean},
            {typeof (char?), DbType.StringFixedLength},
            {typeof (Guid?), DbType.Guid},
            {typeof (DateTime?), DbType.DateTime},
            {typeof (DateTimeOffset?), DbType.DateTimeOffset},
            //{typeof (System.Data.Linq.Binary), DbType.Binary}
        };

        ///// <summary>
        /////     Parameter Direction for Stored Procedure
        ///// </summary>
        //public static readonly Dictionary<string, ParameterDirection> DirectionMap =
        //       new Dictionary<string, ParameterDirection>(StringComparer.InvariantCultureIgnoreCase)
        //    {
        //        {ParamDirectionConstants.Input, ParameterDirection.Input},
        //        {ParamDirectionConstants.Output, ParameterDirection.Output},
        //        {ParamDirectionConstants.InputOutput, ParameterDirection.InputOutput},
        //        {ParamDirectionConstants.ReturnValue, ParameterDirection.ReturnValue}
        //    };
    }
}
