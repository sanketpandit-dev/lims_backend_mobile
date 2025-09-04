using DataAccessLayer;
using DataAccessLayer.Log;
using DataObject;
using System;
using System.Collections.Generic;

namespace BusinessAccessLayer
{
    public class SampleDDLBAL
    {
        private readonly SampleReceiverDAL ddl = new SampleReceiverDAL();

        public Form6DetailsResponseDO GetForm6(GetForm6DetailsDO FSO, int UserId)
        {
            Form6DetailsResponseDO response = new Form6DetailsResponseDO();

            try
            {
                List<Form6DetailsDO> listDO = ddl.GetFSODetails(FSO, UserId);

                if (listDO != null && listDO.Count > 0)
                {
                    response.Form6Details = listDO;
                    response.Success = true;
                    response.Message = "Form6 data fetched successfully.";
                    response.StatusCode = 200;
                }
                else
                {
                    response.Form6Details = new List<Form6DetailsDO>();
                    response.Success = false;
                    response.Message = "No data found.";
                    response.StatusCode = 404;
                }
            }
            catch (Exception ex)
            {
                string remark = "Exception in BAL while fetching Form6";
                LoggerDAL.FnStoreErrorLog("SampleDDLBAL", "GetForm6", remark, ex.StackTrace, ex.Message, UserId);

                response.Form6Details = new List<Form6DetailsDO>();
                response.Success = false;
                response.Message = "An error occurred while fetching Form6.";
                response.StatusCode = 500;
            }

            return response;
        }
    }
}
