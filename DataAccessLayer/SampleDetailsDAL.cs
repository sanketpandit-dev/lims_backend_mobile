using DataAccessLayer.Log;
using DataObject;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public class sampleDAL
    {
        public List<SampleDetailsDO> GetSampleDetails(int UserId)
        {
            List<SampleDetailsDO> listdata = new List<SampleDetailsDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>
                {
                    new MySqlParameter("p_user_id", UserId)
                };

                listdata = Getdataconvert.getdata<SampleDetailsDO>(
                    DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_get_sample_details"));
            }
            catch (Exception ex)
            {
                string message = "Error while returning data from stored procedure.";
                LoggerDAL.FnStoreErrorLog("GetSampleDetailsController", "GetSampleDetails", message, "", "", UserId);
            }
            return listdata;
        }
    }
}
