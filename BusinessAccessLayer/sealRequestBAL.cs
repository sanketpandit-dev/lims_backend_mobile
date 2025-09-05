using DataAccessLayer;
using DataAccessLayer.Log;
using DataObject;
using System;
using System.Collections.Generic;

namespace BusinessAccessLayer
{
    public class sealRequestBAL
    {
        private readonly sealRequestDAL _dal = new sealRequestDAL();

        public ApiResponse<SealRequestDO> InsertSealRequest(SealRequestDO request, int userId)
        {
            try
            {
                List<SealRequestDO> response = _dal.InsertSealRequest(request, userId);

                if (response.Count > 0)
                {
                    return new ApiResponse<SealRequestDO>
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = "Seal request inserted successfully.",
                    };
                }
                else
                {
                    return new ApiResponse<SealRequestDO>
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = "Failed to insert seal request.",
                    };
                }
            }
            catch (Exception ex)
            {
                string message = "Error while returning data from Business Layer";
                LoggerDAL.FnStoreErrorLog("sealRequestBAL", "InsertSealRequest", message, ex.StackTrace, ex.Message, userId);

                return new ApiResponse<SealRequestDO>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Internal error occurred.",
                };
            }
        }

        public ApiResponse<UpdateSealRequestDO> UpdateSealRequest(UpdateSealRequestDO request, int userId)
        {
            try
            {
                List<UpdateSealRequestDO> response = _dal.UpdateSealRequest(request, userId);

                if (response.Count > 0)
                {
                    return new ApiResponse<UpdateSealRequestDO>
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = "Seal request inserted successfully.",
                    };
                }
                else
                {
                    return new ApiResponse<UpdateSealRequestDO>
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = "Failed to insert seal request.",
                    };
                }
            }
            catch (Exception ex)
            {
                string message = "Error while returning data from Business Layer";
                LoggerDAL.FnStoreErrorLog("sealRequestBAL", "InsertSealRequest", message, ex.StackTrace, ex.Message, userId);

                return new ApiResponse<UpdateSealRequestDO>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Internal error occurred.",
                };
            }
        }

        public SealRequestResponseWrapperDO GetSealRequestData(getSealRequestDO request)
        {
            SealRequestResponseWrapperDO response = new SealRequestResponseWrapperDO();

            try
            {
                sealRequestDAL sampleDal = new sealRequestDAL();
                List<getSealRequestResponseDO> listDO = sampleDal.GetSealRequestDetails(request.UserId);

                if (listDO != null && listDO.Count > 0)
                {
                    response.Data = listDO;
                    response.Success = true;
                    response.Message = "Seal request data fetched successfully.";
                    response.StatusCode = 200;
                }
                else
                {
                    response.Data = new List<getSealRequestResponseDO>();
                    response.Success = false;
                    response.Message = "No seal request details found.";
                    response.StatusCode = 404;
                }
            }
            catch (Exception ex)
            {
                response.Data = new List<getSealRequestResponseDO>();
                response.Success = false;
                response.Message = "Error occurred while fetching seal request data.";
                response.StatusCode = 500;
                LoggerDAL.FnStoreErrorLog("SealRequestBAL", "GetSealRequestData", response.Message, ex.StackTrace, ex.Message, request.UserId);
            }

            return response;
        }

    }
}
