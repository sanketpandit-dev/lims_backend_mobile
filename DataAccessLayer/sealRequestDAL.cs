using DataAccessLayer.Log;
using DataObject;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public class sealRequestDAL
    {
        private const string Remark = "Error While SP Execution.";

        public List<SealRequestDO> InsertSealRequest(SealRequestDO request, int insertedBy)
        {
            List<SealRequestDO> result = new List<SealRequestDO>();

            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();

                mysqlParamList.Add(DataClass.GetParameter("@p_user_id", request.UserId));
                mysqlParamList.Add(DataClass.GetParameter("@p_requested_date", request.RequestedDate));
                mysqlParamList.Add(DataClass.GetParameter("@p_seal_number_count", request.SealNumbers));

                result = Getdataconvert.getdata<SealRequestDO>(
                    DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "SP_request_seal")
                );
            }   
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("sealRequestDAL", "InsertSealRequest", Remark, ex.StackTrace, ex.Message, insertedBy);
            }

            return result;
        }

        public List<getSealRequestResponseDO> GetSealRequestDetails(int userId)
        {
            List<getSealRequestResponseDO> listdata = new List<getSealRequestResponseDO>();

            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>
        {
            new MySqlParameter("p_user_id", userId)
        };

                listdata = Getdataconvert.getdata<getSealRequestResponseDO>(
                    DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_seal_request_details"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("SealRequestDAL", "GetSealRequestDetails",
                    "Error while fetching seal request details", ex.StackTrace, ex.Message, userId);
            }

            return listdata;
        }


    }
}
