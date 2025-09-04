using DataObject.Errorlog;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Log
{
    public static class LoggerDAL
    {

        //public static List<ErrorLogDO> StoreErrorLog(ErrorLogDO errorlog)
        //{

        //    List<ErrorLogDO> listdata = new List<ErrorLogDO>();
        //    try
        //    {
        //        getConvertedData Getdataconvert = new getConvertedData();
        //        List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
        //        mysqlParamList.Add(DataClass.GetParameter("@p_module", ""));
        //        mysqlParamList.Add(DataClass.GetParameter("@p_function_name", errorlog.FunctionName));
        //        mysqlParamList.Add(DataClass.GetParameter("@p_remark", errorlog.message));
        //        mysqlParamList.Add(DataClass.GetParameter("@p_stacktrace", errorlog.StackTrace));
        //        mysqlParamList.Add(DataClass.GetParameter("@p_error_description", errorlog.Error_Description));
        //        mysqlParamList.Add(DataClass.GetParameter("@p_inserted_by", errorlog.UserId));
        //        listdata = Getdataconvert.getdata<ErrorLogDO>(DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_store_error_log"));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return listdata;
        //}

        public static int SaveRequestResponselog(RequestResponseLogDO logdata)
        {
            int newId = 0;

            try
            {
                if (logdata.Action == "SAVE_REQUEST")
                {
                    logdata.NewId = 0;
                }


                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                mysqlParamList.Add(DataClass.GetParameter("@p_action", logdata.Action));
                mysqlParamList.Add(DataClass.GetParameter("@p_json", logdata.JsonData));
                mysqlParamList.Add(DataClass.GetParameter("@p_api_url", logdata.ApiUrl));
                mysqlParamList.Add(DataClass.GetParameter("@p_userid", logdata.UserId));
                mysqlParamList.Add(DataClass.GetParameter("@p_id", logdata.NewId));
                
                if (logdata.Action == "SAVE_REQUEST")
                {
                    using (MySqlDataReader reader = DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_log_request_response"))
                    {
                        if (reader != null && reader.Read())
                        {
                            newId = Convert.ToInt32(reader["new_id"]);
                        }
                    }
                }
                else if (logdata.Action == "SAVE_RESPONSE")
                {
                    using (MySqlDataReader reader = DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_log_request_response"))
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

            return newId;
        }

        public static  List<ErrorLogDO> FnStoreErrorLog(string controller_name, string function_name, string message, string stackTrace, string errorDescription, int user_id)
        {
            List<ErrorLogDO> listdata = new List<ErrorLogDO>();

            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                mysqlParamList.Add(DataClass.GetParameter("@p_module", controller_name));
                mysqlParamList.Add(DataClass.GetParameter("@p_function_name", function_name));
                mysqlParamList.Add(DataClass.GetParameter("@p_remark", message));
                mysqlParamList.Add(DataClass.GetParameter("@p_stacktrace", stackTrace));
                mysqlParamList.Add(DataClass.GetParameter("@p_error_description", errorDescription));
                mysqlParamList.Add(DataClass.GetParameter("@p_inserted_by", user_id));
                listdata = Getdataconvert.getdata<ErrorLogDO>(DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_store_error_log"));
                return listdata;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}

