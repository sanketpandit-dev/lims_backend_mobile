using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using System.IO;

using System.IO;

namespace DataAccessLayer
{
    internal class DataClass
    {
        private static string DBFixName = "limsmgt";

        private static readonly IConfigurationRoot configuration;
        private static readonly string LimsConnection;

        static DataClass()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();
            LimsConnection = configuration.GetConnectionString("LimsConnection");
        }


        private static MySqlConnection GetConnection()
        {
            return new MySqlConnection(LimsConnection);
        }

        public static MySqlDataReader getreaderFromSPWithParm(List<MySqlParameter> sqlParamList, string DBName, string SP_Name)
        {
            if (string.IsNullOrEmpty(DBName)) DBName = DBFixName;

            MySqlConnection conn = GetConnection();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(SP_Name, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                if (sqlParamList != null)
                {
                    foreach (var param in sqlParamList)
                        cmd.Parameters.Add(param);
                }

                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public static MySqlDataReader getreaderFromSP(string DBName, string SP_Name)
        {
            if (string.IsNullOrEmpty(DBName)) DBName = DBFixName;

            MySqlConnection conn = GetConnection();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(SP_Name, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public static MySqlDataReader getreaderFromSelect(string DBName, string Qry)
        {
            if (string.IsNullOrEmpty(DBName)) DBName = DBFixName;

            MySqlConnection conn = GetConnection();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(Qry, conn))
            {
                cmd.CommandType = CommandType.Text;
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public static MySqlParameter GetParameter(string parameterName, object value)
        {
            return new MySqlParameter(parameterName, value);
        }
        public static int ExecuteNonQuery(string SP_Name, string DBName, List<MySqlParameter> sqlParamList = null)

        {
            if (string.IsNullOrEmpty(DBName)) DBName = DBFixName;

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(SP_Name, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (sqlParamList != null)
                    {
                        foreach (var param in sqlParamList)
                            cmd.Parameters.Add(param);
                    }

                    return cmd.ExecuteNonQuery();
                }

            }
        }
    }

    public class getConvertedData
    {
        public List<T> getdata<T>(MySqlDataReader dr) where T : new()
        {
            List<T> Returndata = new List<T>();
            try
            {
                Type type = typeof(T);
                PropertyInfo[] props = type.GetProperties();

                var fieldNames = Enumerable.Range(0, dr.FieldCount).Select(i => dr.GetName(i).ToUpper()).ToArray();

                while (dr.Read())
                {
                    T obj = new T();
                    foreach (var pi in props)
                    {
                        if (fieldNames.Contains(pi.Name.ToUpper()))
                        {
                            object val = dr[pi.Name];
                            if (val != DBNull.Value)
                            {
                                if (pi.PropertyType == typeof(string))
                                    pi.SetValue(obj, val.ToString());
                                else
                                    pi.SetValue(obj, Convert.ChangeType(val, pi.PropertyType));
                            }
                        }
                    }
                    Returndata.Add(obj);
                }
            }
            finally
            {
                dr.Close();
            }
            return Returndata;
        }
    }

}
