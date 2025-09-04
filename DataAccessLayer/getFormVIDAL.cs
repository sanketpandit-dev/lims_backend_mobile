using DataAccessLayer.Log;
using DataObject;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public class SampleReceiverDAL
    {
        public List<Form6DetailsDO> GetFSODetails(GetForm6DetailsDO FSO, int UserId)
        {
            List<Form6DetailsDO> listdata = new List<Form6DetailsDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();

                mysqlParamList.Add(DataClass.GetParameter("@p_serial_no", FSO.SerialNo));

                listdata = Getdataconvert.getdata<Form6DetailsDO>(
                    DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_get_formVI")
                );
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("SampleReceiverDAL", "GetFSODetails", "Error fetching Form6 data", ex.StackTrace, ex.Message, UserId);
            }
            return listdata;
        }
    }
}
