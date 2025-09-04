using BusinessAccessLayer;
using DataAccessLayer.Log;
using DataObject;
using LIMS.Common;
using LIMS.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace LIMS.Controllers
{
    [Route("api/mobile")]
    public class GetSampleDetailsController : Controller
    {
        [HttpPost("get_samples")]
        [ValidateToken]
        [ValidateEncryptedRequest]
        public IActionResult GetUserWiseSampleData([FromBody] EncryptedRequest request)
        {
            int userId = 0;
            int.TryParse(HttpContext.Items["UserId"]?.ToString(), out userId);

            try
            {
                
                DecryptedJsonDataWithKey decrypted = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
 
                SampleDetailsDO payload = JsonConvert.DeserializeObject<SampleDetailsDO>(decrypted.DecryptedJsonData);

               
                if (payload == null || payload.UserID <= 0)
                {
                    string message = "Invalid sample data. Please provide a valid UserId.";
                    LoggerDAL.FnStoreErrorLog("GetSampleDetailsController", "GetUserWiseSampleData", message, "", "", userId);
                    return BadRequest(ApiResponse<object>.FailureResponse(message));
                }
 
                GetSampleBAL bal = new GetSampleBAL();
                var result = bal.GetSampleData(payload, payload.UserID);
                 
                if (result == null || !result.Success)
                {
                    LoggerDAL.FnStoreErrorLog("GetSampleDetailsController", "GetUserWiseSampleData", result?.Message ?? "No data", "", "", payload.UserID);
                    return NotFound(ApiResponse<object>.FailureResponse(result?.Message ?? "No data found."));
                }

               
                var encryptedResponse = Cryptohelper.EncryptResponse(result, decrypted.ReqAesKey, decrypted.Iv);

                return Ok(encryptedResponse);
            }
            catch (Exception ex)
            {
                string message = "An error occurred while getting sample data.";
                LoggerDAL.FnStoreErrorLog("GetSampleDetailsController", "GetUserWiseSampleData", message, ex.StackTrace, ex.Message, userId);
                return StatusCode(500, ApiResponse<object>.FailureResponse(message));
            }
        }
    }
}
