using System;
using System.Data;

namespace LearActionPlans.Utilities
{
    public static class DatabaseReader
    {
        public static int? ConvertInteger(IDataRecord reader, string column)
        {
            return reader[column] == DBNull.Value ? (int?)null : Convert.ToInt32(reader[column]);
        }

        public static int ConvertIntegerBezNull(IDataRecord reader, string column)
        {
            return Convert.ToInt32(reader[column]);
        }

        public static int? ConvertIntegerRow(DataRow reader, string column)
        {
            return reader[column] == DBNull.Value ? (int?)null : Convert.ToInt32(reader[column]);
        }

        public static decimal? ConvertDecimal(IDataRecord reader, string column)
        {
            return reader[column] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader[column]);
        }

        public static string ConvertString(IDataRecord reader, string column)
        {
            return reader[column] == DBNull.Value ? string.Empty : reader[column].ToString();
        }

        public static bool? ConvertBoolean(IDataRecord reader, string column)
        {
            return reader[column] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader[column]);
        }

        public static bool ConvertBooleanBezNull(IDataRecord reader, string column)
        {
            return Convert.ToBoolean(reader[column]);
        }

        public static byte? ConvertByte(IDataRecord reader, string column)
        {
            return reader[column] == DBNull.Value ? (byte?)null : Convert.ToByte(reader[column]);
        }

        public static byte ConvertByteBezNull(IDataRecord reader, string column)
        {
            return Convert.ToByte(reader[column]);
        }

        public static DateTime? ConvertDateTime(IDataRecord reader, string column)
        {
            return reader[column] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader[column]);
        }

        public static DateTime? ConvertDateTimeRow(DataRow reader, string column)
        {
            return reader[column] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader[column]);
        }
    }
}
