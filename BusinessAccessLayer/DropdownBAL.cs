using DataAccessLayer;
using DataAccessLayer.Log;
using DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer
{
    public class DropdownBAL
    {
        DropdownDAL ddlDAL = new DropdownDAL();

        public DropdownWrapperResponseDO GetCountries(int userId)
        {
            var response = new DropdownWrapperResponseDO();
            try
            {
                DropdownDAL ddlDAL = new DropdownDAL();
                var dataList = ddlDAL.Getcountries(userId);

                response.Data = dataList;
                response.Success = true;
                response.StatusCode = 200;
                response.Message = "Countries fetched successfully";
            }
            catch (Exception ex)
            {
                response.Data = new List<DropdownResponseDO>();
                response.Success = false;
                response.StatusCode = 500;
                response.Message = "Error while returning data from Business Layer";

                LoggerDAL.FnStoreErrorLog("DropdownController", "GetCountries", response.Message, ex.StackTrace, ex.Message, userId);
            }

            return response;
        }


        public DropdownWrapperResponseDO GetStates(DropdownDO DDL, int userId)
        {
            var response = new DropdownWrapperResponseDO();
            try
            {
                DropdownDAL ddlDAL = new DropdownDAL();
                var dataList = ddlDAL.Getstates(DDL,userId);

                response.Data = dataList;
                response.Success = true;
                response.StatusCode = 200;
                response.Message = "States fetched successfully";
            }
            catch (Exception ex)
            {
                response.Data = new List<DropdownResponseDO>();
                response.Success = false;
                response.StatusCode = 500;
                response.Message = "Error while returning data from Business Layer";

                LoggerDAL.FnStoreErrorLog("DropdownController", "GetStates", response.Message, ex.StackTrace, ex.Message, userId);
            }

            return response;
        }


        public DropdownWrapperResponseDO GetCities(DropdownDO DDL, int userId)
        {
            var response = new DropdownWrapperResponseDO();
            try
            {
                DropdownDAL ddlDAL = new DropdownDAL();
                var dataList = ddlDAL.GetCities(DDL,userId);

                response.Data = dataList;
                response.Success = true;
                response.StatusCode = 200;
                response.Message = "Cities fetched successfully";
            }
            catch (Exception ex)
            {
                response.Data = new List<DropdownResponseDO>();
                response.Success = false;
                response.StatusCode = 500;
                response.Message = "Error while returning data from Business Layer";

                LoggerDAL.FnStoreErrorLog("DropdownController", "GetCities", response.Message, ex.StackTrace, ex.Message, userId);
            }

            return response;
        }

             
        public DropdownWrapperResponseDO GetDistrictsByStateId(DropdownDO DDL, int userId)
        {
            var response = new DropdownWrapperResponseDO();
            try
            {
                DropdownDAL ddlDAL = new DropdownDAL();
                var dataList = ddlDAL.GetDistrictsByStateId(DDL.StateId, userId);

                response.Data = dataList;
                response.Success = true;
                response.StatusCode = 200;
                response.Message = "Districts fetched successfully";
            }
            catch (Exception ex)
            {
                response.Data = new List<DropdownResponseDO>();
                response.Success = false;
                response.StatusCode = 500;
                response.Message = "Error while returning districts from Business Layer";

                LoggerDAL.FnStoreErrorLog("DropdownBAL", "GetDistrictsByStateId", response.Message, ex.StackTrace, ex.Message, userId);
            }
            return response;
        }

        public DropdownWrapperResponseDO GetRegionsByDistrictId(DropdownDO DDL, int userId)
        {
            var response = new DropdownWrapperResponseDO();
            try
            {
                DropdownDAL ddlDAL = new DropdownDAL();
                var dataList = ddlDAL.GetRegionsByDistrictId(DDL, userId);

                response.Data = dataList;
                response.Success = true;
                response.StatusCode = 200;
                response.Message = "Regions fetched successfully";
            }
            catch (Exception ex)
            {
                response.Data = new List<DropdownResponseDO>();
                response.Success = false;
                response.StatusCode = 500;
                response.Message = "Error while fetching regions";

                LoggerDAL.FnStoreErrorLog("DropdownBAL", "GetRegionsByDistrictId", response.Message, ex.StackTrace, ex.Message, userId);
            }
            return response;
        }

        public DropdownWrapperResponseDO GetDivisionsByRegionId(DropdownDO DDL, int userId)
        {
            var response = new DropdownWrapperResponseDO();
            try
            {
                DropdownDAL ddlDAL = new DropdownDAL();
                var dataList = ddlDAL.GetDivisionsByRegionId(DDL, userId);

                response.Data = dataList;
                response.Success = true;
                response.StatusCode = 200;
                response.Message = "Divisions fetched successfully";
            }
            catch (Exception ex)
            {
                response.Data = new List<DropdownResponseDO>();
                response.Success = false;
                response.StatusCode = 500;
                response.Message = "Error while fetching divisions";

                LoggerDAL.FnStoreErrorLog("DropdownBAL", "GetDivisionsByRegionId", response.Message, ex.StackTrace, ex.Message, userId);
            }
            return response;
        }


        public SampleDropDownResponseDO NatureOfSample(int userId)
        {
            SampleDropDownResponseDO response = new SampleDropDownResponseDO();
            try
            {
                List<SampleDropDownDO> datalist = new List<SampleDropDownDO>();
                datalist = ddlDAL.GetNatureOfSample(userId);
                if (datalist != null || datalist.Count > 0)
                {
                    response.Data = datalist;
                    response.Success = true;
                    response.StatusCode = 200;
                    response.Message = "Nature of sample fetched successfully";
                }
                else
                {

                    response.Data = new List<SampleDropDownDO>();
                    response.Success = false;
                    response.StatusCode = 404;
                    response.Message = "Nature of sample  not fetched successfully";
                }
            }
            catch (Exception ex)
            {

                LoggerDAL.FnStoreErrorLog("SampleDropDownBAL", "NatureOfSample", response.Message, ex.StackTrace, ex.Message, userId);
            }

            return response;
        }


        public SampleDropDownResponseDO LabMaster(int userId)
        {
            SampleDropDownResponseDO response = new SampleDropDownResponseDO();
            try
            {
                List<SampleDropDownDO> datalist = new List<SampleDropDownDO>();
                datalist = ddlDAL.GetLabMaster(userId);  

                if (datalist != null && datalist.Count > 0)
                {
                    response.Data = datalist;
                    response.Success = true;
                    response.StatusCode = 200;
                    response.Message = "Lab master fetched successfully";
                }
                else
                {
                    response.Data = new List<SampleDropDownDO>();
                    response.Success = false;
                    response.StatusCode = 404;
                    response.Message = "Lab master not fetched successfully";
                }
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("SampleDropDownBAL", "LabMaster", response.Message, ex.StackTrace, ex.Message, userId);
            }

            return response;
        }







        public SampleDropDownResponseDO GetSealNumber(int requestId)
        {
            SampleDropDownResponseDO response = new SampleDropDownResponseDO();
            try
            {
                List<SampleDropDownDO> datalist = ddlDAL.GetSealNumber(requestId);

                if (datalist != null && datalist.Count > 0) // ✅ fixed condition
                {
                    response.Data = datalist;
                    response.Success = true;
                    response.StatusCode = 200;
                    response.Message = "Seal number fetched successfully";
                }
                else
                {
                    response.Data = new List<SampleDropDownDO>();
                    response.Success = false;
                    response.StatusCode = 404;
                    response.Message = "Seal number not fetched successfully";
                }
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("SampleDropDownBAL", "GetSealNumber", response.Message, ex.StackTrace, ex.Message, requestId);
            }

            return response;
        }





    }
}
