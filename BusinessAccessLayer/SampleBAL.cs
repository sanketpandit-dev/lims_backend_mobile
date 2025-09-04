using DataAccessLayer;
using DataAccessLayer.Log;
using DataObject;
using System;
using System.Collections.Generic;

namespace BusinessAccessLayer
{
    public class SampleBAL
    {
        private readonly SampleDAL _dal = new SampleDAL();





        public InsertSampleResponseDO InsertSample(InsertSampleRequestDO request, int userId)
        {
            List<InsertSampleResponseDO> userResponse = new List<InsertSampleResponseDO>();

            InsertSampleResponseDO userResult = new InsertSampleResponseDO();
            try
            {
                userResponse = _dal.InsertSample(request, userId, userId);
                if (userResponse.Count > 0)
                {
                    userResult = userResponse[0];
                }

            }
            catch (Exception ex)
            {
                string message = "Error while returning data from Buisness Layer";
                LoggerDAL.FnStoreErrorLog("SampleBAL", "InsertSample", message, "", "", userId);
            }
            return userResult;
        }




    }
}

