using BusinessAccessLayer;
using DataAccessLayer.Log;
using DataObject;
using LIMS.Common;
using LIMS.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LIMS.Controllers
{
    [Route("api/SealRequest")]
    public class SealRequestController : Controller
    {
        [HttpPost("seal_request")]
        [ValidateToken]
        [ValidateEncryptedRequest]
        public IActionResult Insert([FromBody] EncryptedRequest request)
        {
            int userId = 0;
            int.TryParse(HttpContext.Items["UserId"]?.ToString(), out userId);

            try
            {
                DecryptedJsonDataWithKey decrypted = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                SealRequestDO payload = JsonConvert.DeserializeObject<SealRequestDO>(decrypted.DecryptedJsonData);

                if (payload == null || payload.UserId <= 0)
                {
                    LoggerDAL.FnStoreErrorLog("SealRequestController", "Insert", "Invalid request data.", "", "", userId);
                    return BadRequest(ApiResponse<object>.FailureResponse("Invalid request data."));
                }

                var bal = new sealRequestBAL();
                var result = bal.InsertSealRequest(payload, userId);

                if (!result.Success)
                {
                    return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message));
                }

                var encryptedResponse = Cryptohelper.EncryptResponse(result, decrypted.ReqAesKey, decrypted.Iv);
                return Ok(encryptedResponse);

                //return Ok(result);
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("SealRequestController", "Insert", "Insert seal request failed", ex.StackTrace, ex.Message, userId);
                return StatusCode(500, ApiResponse<object>.FailureResponse("Insert seal request failed"));
            }
        }

        [HttpPost("GetSealRequestDetails")]
        public IActionResult GetSealRequestDetails([FromBody] getSealRequestDO payload)
        {
            int userId = 0;
            try
            {
                if (payload == null || payload.UserId <= 0)
                {
                    return BadRequest(ApiResponse<object>.FailureResponse("Invalid UserId"));
                }

                sealRequestBAL bal = new sealRequestBAL();
                var result = bal.GetSealRequestData(payload);

                if (result == null || !result.Success || result.Data.Count == 0)
                {
                    return NotFound(ApiResponse<object>.FailureResponse(result?.Message ?? "No data found."));
                }

                // Return plain result for testing (no encryption)
                return Ok(result);
            }
            catch (Exception ex)
            {
                string message = "An error occurred while getting seal request details.";
                LoggerDAL.FnStoreErrorLog("SealRequestController", "GetSealRequestDetails", message, ex.StackTrace, ex.Message, userId);
                return StatusCode(500, ApiResponse<object>.FailureResponse(message));
            }
        }
    }
}
