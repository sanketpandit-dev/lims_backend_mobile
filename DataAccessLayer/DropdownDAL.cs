using DataAccessLayer.Log;
using DataObject;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DropdownDAL
    {
        string remark = "Error while returning data from stored procedure";

        public List<DropdownResponseDO> Getcountries(int UserId)
        {
            List<DropdownResponseDO> listdata = new List<DropdownResponseDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                listdata = Getdataconvert.getdata<DropdownResponseDO>(
                    DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_get_country"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("RoleDAL", "Getcountries", remark, ex.StackTrace, ex.Message, UserId);
            }
            return listdata;
        }


        public List<DropdownResponseDO> Getstates(DropdownDO DDL, int UserId)
        {
            List<DropdownResponseDO> listdata = new List<DropdownResponseDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                mysqlParamList.Add(DataClass.GetParameter("@p_country_id", DDL.CountryId));

                listdata = Getdataconvert.getdata<DropdownResponseDO>(
                    DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_get_states"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("RoleDAL", "Getstates", remark, ex.StackTrace, ex.Message, UserId);
            }
            return listdata;
        }

        public List<DropdownResponseDO> GetCities(DropdownDO DDL, int UserId)
        {
            List<DropdownResponseDO> listdata = new List<DropdownResponseDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                mysqlParamList.Add(DataClass.GetParameter("@p_state_id", DDL.StateId
                    ));

                listdata = Getdataconvert.getdata<DropdownResponseDO>(
                    DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_get_cities"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("RoleDAL", "GetCities", remark, ex.StackTrace, ex.Message, UserId);

            }
            return listdata;
        }

       
        public List<DropdownResponseDO> GetDistrictsByStateId(int stateId, int userId)
        {
            List<DropdownResponseDO> listdata = new List<DropdownResponseDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();

                mysqlParamList.Add(DataClass.GetParameter("@p_state_id", stateId));

                listdata = Getdataconvert.getdata<DropdownResponseDO>(
                    DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "get_districts_by_state_id"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("DropdownDAL", "GetDistrictsByStateId", remark, ex.StackTrace, ex.Message, userId);
            }
            return listdata;
        }

        public List<DropdownResponseDO> GetRegionsByDistrictId(DropdownDO DDL, int userId)
        {
            List<DropdownResponseDO> listData = new List<DropdownResponseDO>();
            try
            {
                getConvertedData dataConvert = new getConvertedData();
                List<MySqlParameter> mysqlParams = new List<MySqlParameter>();
                mysqlParams.Add(DataClass.GetParameter("@p_division_id", DDL.DivisionId));

                listData = dataConvert.getdata<DropdownResponseDO>(
                    DataClass.getreaderFromSPWithParm(mysqlParams, "limsmgt", "get_regions_by_district_id")
                );
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("DropdownDAL", "GetRegionsByDistrictId", remark, ex.StackTrace, ex.Message, userId);
            }
            return listData;
        }

        public List<DropdownResponseDO> GetDivisionsByRegionId(DropdownDO DDL, int userId)
        {
            List<DropdownResponseDO> listData = new List<DropdownResponseDO>();
            try
            {
                getConvertedData dataConvert = new getConvertedData();
                List<MySqlParameter> mysqlParams = new List<MySqlParameter>();
                mysqlParams.Add(DataClass.GetParameter("@p_district_id", DDL.DistrictId));

                listData = dataConvert.getdata<DropdownResponseDO>(
                    DataClass.getreaderFromSPWithParm(mysqlParams, "limsmgt", "get_divisions_by_region_id")
                );
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("DropdownDAL", "GetDivisionsByRegionId", remark, ex.StackTrace, ex.Message, userId);
            }
            return listData;
        }

        public List<SampleDropDownDO> GetNatureOfSample(int UserId)

        {

            List<SampleDropDownDO> listdata = new List<SampleDropDownDO>();

            try

            {

                getConvertedData Getdataconvert = new getConvertedData();

                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();

                listdata = Getdataconvert.getdata<SampleDropDownDO>(DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_get_nature_of_sample"));

            }

            catch (Exception ex)

            {

                LoggerDAL.FnStoreErrorLog("SampleReceiverDAL", "GetNatureOfSample", remark, ex.StackTrace, ex.Message, UserId);

            }

            return listdata;

        }

        public List<SampleDropDownDO> GetLabMaster(int UserId)
        {
            List<SampleDropDownDO> listdata = new List<SampleDropDownDO>();

            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();

                listdata = Getdataconvert.getdata<SampleDropDownDO>(
                    DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_get_lab_master")
                );
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("SampleReceiverDAL", "GetLabMaster", "Fetching Lab Master failed", ex.StackTrace, ex.Message, UserId);
            }

            return listdata;
        }


        public List<SampleDropDownDO> GetSealNumber(int requestId)
        {
            List<SampleDropDownDO> listdata = new List<SampleDropDownDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();

                mysqlParamList.Add(new MySqlParameter("@p_request_id", requestId));

                listdata = Getdataconvert.getdata<SampleDropDownDO>(
                    DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_get_seal_number"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("SampleReceiverDAL", "GetSealNumber", "", ex.StackTrace, ex.Message, requestId);
            }

            return listdata;
        }






    }
}
