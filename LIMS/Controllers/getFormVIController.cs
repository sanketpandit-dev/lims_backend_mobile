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
    [ApiController]
    [Route("api/mobile")]
    public class GetFormVIController : ControllerBase
    {
        private readonly SampleDDLBAL SampleDDLBAL = new SampleDDLBAL();

        [HttpPost("GetForm6Data")]
        [ValidateToken]
        [ValidateEncryptedRequest]
        public IActionResult GetForm6Data([FromBody] EncryptedRequest request)
        {
            int UserId = 0;
            int.TryParse(HttpContext.Items["UserId"]?.ToString(), out UserId);

            try
            {
                var decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                GetForm6DetailsDO FSO = JsonConvert.DeserializeObject<GetForm6DetailsDO>(decryptedResult.DecryptedJsonData);

                if (FSO == null)
                {
                    string message = "Invalid data. Please check required fields.";
                    LoggerDAL.FnStoreErrorLog("GetFormVIController", "GetForm6Data", message, "", "", UserId);
                    return BadRequest(ApiResponse<object>.FailureResponse(message));
                }

                var result = SampleDDLBAL.GetForm6(FSO, UserId);

                if (result == null || result.Success == false)
                {
                    LoggerDAL.FnStoreErrorLog("GetFormVIController", "GetForm6Data", result?.Message ?? "Error", "", "", UserId);
                    return NotFound(ApiResponse<object>.FailureResponse(result?.Message ?? "Error"));
                }

                return Ok(Cryptohelper.EncryptResponse(result, decryptedResult.ReqAesKey, decryptedResult.Iv));
                //return Ok(result);
            }
            catch (Exception ex)
            {
                string message = "An error occurred while getting Form6 data.";
                LoggerDAL.FnStoreErrorLog("GetFormVIController", "GetForm6Data", message, ex.StackTrace, ex.Message, UserId);
                return StatusCode(500, ApiResponse<object>.FailureResponse(message));
            }
        }
    }
}
