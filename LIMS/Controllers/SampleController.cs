using BusinessAccessLayer;
using BusinessAccessLayer.Log;
using DataAccessLayer.Log;
using DataObject;
using DataObject.Errorlog;
using LIMS.Common;
using LIMS.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LIMS.Controllers
{
    [Route("api/Sample")]
    public class SampleController : BaseApiController
    {
        [HttpPost("Insert")] 
        [ValidateToken]
        [ValidateEncryptedRequest]
        public IActionResult Insert([FromBody] EncryptedRequest request)
        {
            int userId = 0;
            int.TryParse(HttpContext.Items["UserId"].ToString(), out userId);

            try
            {
                DecryptedJsonDataWithKey decrypted = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                InsertSampleRequestDO payload = JsonConvert.DeserializeObject<InsertSampleRequestDO>(decrypted.DecryptedJsonData);

                if (payload == null || string.IsNullOrWhiteSpace(payload.SampleCodeNumber))
                {
                    LoggerDAL.FnStoreErrorLog("SampleController", "Insert", "Invalid sample data.", "", "", userId);
                    return BadRequest(ApiResponse<object>.FailureResponse("Invalid sample data."));
                }


                var bal = new SampleBAL();
                var result = bal.InsertSample(payload, userId);

                if (!result.Success)
                {
                    return HandleFailureResponse<string>(result.Message, result.StatusCode);
                }

                var encryptedResponse = Cryptohelper.EncryptResponse(result, decrypted.ReqAesKey, decrypted.Iv);



                return Ok(encryptedResponse);
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("SampleController", "Insert", "Insert sample failed", ex.StackTrace, ex.Message, userId);
                return StatusCode(500, ApiResponse<object>.FailureResponse("Insert sample failed"));
            }
        }
    }
}

