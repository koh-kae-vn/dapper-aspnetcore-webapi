using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DapperASPNetCore.Helpers
{
    public static class Helper
    {
		public static IEnumerable<Dictionary<string, object>> ToDictionary(this DataTable table)
		{
			string[] columns = table.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
			IEnumerable<Dictionary<string, object>> result = table.Rows.Cast<DataRow>()
					.Select(dr => columns.ToDictionary(c => c, c => dr[c]));
			return result;
		}

        public static DataSet ConvertDataReaderToDataSet(IDataReader data)
        {
            DataSet ds = new DataSet();
            int i = 0;
            while (!data.IsClosed)
            {
                ds.Tables.Add("Table" + (i + 1));
                ds.EnforceConstraints = false;
                ds.Tables[i].Load(data);
                i++;
            }
            return ds;
        }

        private static byte[] key = new byte[] { };
        private static byte[] IV = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private const string EncryptionKey = "abcdefgh";

        public static string Decrypt(string stringToDecrypt)
        {
            try
            {
                byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
                key = System.Text.Encoding.UTF8.GetBytes(EncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
