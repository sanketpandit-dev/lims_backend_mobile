using DataAccessLayer;
using DataAccessLayer.Log;
using DataObject;
using System;
using System.Collections.Generic;

namespace BusinessAccessLayer
{
    public class GetSampleBAL
    {
        public sampleResponseDataDO GetSampleData(SampleDetailsDO sample, int UserId)
        {
            sampleResponseDataDO response = new sampleResponseDataDO();

            try
            {
                sampleDAL sampleDal = new sampleDAL();
                List<SampleDetailsDO> listDO = sampleDal.GetSampleDetails(UserId);

                if (listDO != null && listDO.Count > 0)
                {
                    response.SampleList = listDO;
                    response.Success = true;
                    response.Message = "Sample data fetched successfully.";
                    response.StatusCode = 200;
                }
                else
                {
                    response.SampleList = new List<SampleDetailsDO>();
                    response.Success = false;
                    response.Message = "No sample details found for the given User ID.";
                    response.StatusCode = 404;
                }
            }
            catch (Exception ex)
            {
                response.SampleList = new List<SampleDetailsDO>();
                response.Success = false;
                response.Message = "Error occurred while fetching sample data.";
                response.StatusCode = 500;
                LoggerDAL.FnStoreErrorLog("GetSampleDetailsController", "GetSampleData", response.Message, "", "", UserId);
            }

            return response;
        }
    }
}
