using BusinessAccessLayer;
using DataAccessLayer.Log;
using DataObject;
using LIMS.Common;
using LIMS.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LIMS.Controllers
{
    [Route("api/mobile")]

    public class DropdownController : BaseApiController
    {
        DropdownBAL DDLBAL = new DropdownBAL();


        [HttpPost("GetCountries")]
        [ValidateToken]
        [ValidateEncryptedRequest]

        public IActionResult GetCountries([FromBody] EncryptedRequest request)
        {
            int UserId = 0;
            int.TryParse(HttpContext.Items["UserId"]?.ToString(), out UserId);

            try
            {

                DecryptedJsonDataWithKey decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
               // DropdownDO DDL = JsonConvert.DeserializeObject<DropdownDO>(decryptedResult.DecryptedJsonData);

                var result = DDLBAL.GetCountries(UserId);

                if (result == null)
                {
                    string message = "Country list fetch failed or returned no data.";
                    LoggerDAL.FnStoreErrorLog("DropdownController", "GetCountries", message, "", "", UserId);
                    return NotFound(ApiResponse<object>.FailureResponse(message));
                }

                 return Ok(Cryptohelper.EncryptResponse(result, decryptedResult.ReqAesKey, decryptedResult.Iv));
            }
            catch (Exception ex)
            {
                string message = "An error occurred during getting user list.";
                LoggerDAL.FnStoreErrorLog("DropdownController", "GetCountries", message, ex.Message, "", UserId);
                return StatusCode(500,
                       ApiResponse<object>.FailureResponse(message));
            }
        }


        [HttpPost("GetStates")]
        [ValidateToken]
        [ValidateEncryptedRequest]

        public IActionResult GetStates([FromBody] EncryptedRequest request)
        {
            int UserId = 0;
            int.TryParse(HttpContext.Items["UserId"]?.ToString(), out UserId);

            try
            {

                DecryptedJsonDataWithKey decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                DropdownDO DDL = JsonConvert.DeserializeObject<DropdownDO>(decryptedResult.DecryptedJsonData);

                var result = DDLBAL.GetStates(DDL,UserId);

                if (result == null)
                {
                    string message = "State list fetch failed or returned no data.";
                    LoggerDAL.FnStoreErrorLog("DropdownController", "GetStates", message, "", "", UserId);
                    return NotFound(ApiResponse<object>.FailureResponse(message));
                }

                 return Ok(Cryptohelper.EncryptResponse(result, decryptedResult.ReqAesKey, decryptedResult.Iv));
            }
            catch (Exception ex)
            {
                string message = "An error occurred during getting state list.";
                LoggerDAL.FnStoreErrorLog("DropdownController", "GetStates", message, ex.Message, "", UserId);
                return StatusCode(500,
                       ApiResponse<object>.FailureResponse(message));
            }
        }


        [HttpPost("GetCities")]
        [ValidateToken]
        [ValidateEncryptedRequest]

        public IActionResult GetCitites([FromBody] EncryptedRequest request)
        {
            int UserId = 0;
            int.TryParse(HttpContext.Items["UserId"]?.ToString(), out UserId);

            try
            {
                DecryptedJsonDataWithKey decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                DropdownDO DDL = JsonConvert.DeserializeObject<DropdownDO>(decryptedResult.DecryptedJsonData);

                var result = DDLBAL.GetCities(DDL,UserId);

                if (result == null)
                {
                    string message = "City list fetch failed or returned no data.";
                    LoggerDAL.FnStoreErrorLog("DropdownController", "GetCities", message, "", "", UserId);
                    return NotFound(ApiResponse<object>.FailureResponse(message));
                }

                 return Ok(Cryptohelper.EncryptResponse(result, decryptedResult.ReqAesKey, decryptedResult.Iv));
            }
            catch (Exception ex)
            {
                string message = "An error occurred during getting City list.";
                LoggerDAL.FnStoreErrorLog("DropdownController", "GetCities", message, ex.Message, "", UserId);
                return StatusCode(500,
                       ApiResponse<object>.FailureResponse(message));
            }
        }
        
        
        
        [HttpPost("GetNatureOfSample")]
        [ValidateToken]
        [ValidateEncryptedRequest]

        public IActionResult GetNatureOfSample([FromBody] EncryptedRequest request)
        {
            int UserId = 0;
            int.TryParse(HttpContext.Items["UserId"]?.ToString(), out UserId);

            try
            {

                DecryptedJsonDataWithKey decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                DropdownDO DDL = JsonConvert.DeserializeObject<DropdownDO>(decryptedResult.DecryptedJsonData);

                var result = DDLBAL.NatureOfSample(UserId);

                if (result == null)
                {
                    string message = "Nature of sample list fetch failed or returned no data.";
                    LoggerDAL.FnStoreErrorLog("SampleReceiverController", "GetNatureOfSample", message, "", "", UserId);
                    return NotFound(ApiResponse<object>.FailureResponse(message));
                }

                return Ok(Cryptohelper.EncryptResponse(result, decryptedResult.ReqAesKey, decryptedResult.Iv));

            }
            catch (Exception ex)
            {
                string message = "An error occurred during fetch Nature of sample.";
                LoggerDAL.FnStoreErrorLog("SampleReceiverController", "GetNatureOfSample", message, ex.StackTrace, ex.Message, UserId);
                return StatusCode(500,
                       ApiResponse<object>.FailureResponse(message));
            }
        }







        [HttpPost("GetDistrictsByStateId")]
        [ValidateToken]
        [ValidateEncryptedRequest]
        public IActionResult GetDistrictsByStateId([FromBody] EncryptedRequest request)
        {
            int userId = 0;
            int.TryParse(HttpContext.Items["UserId"]?.ToString(), out userId);

            try
            {
                DecryptedJsonDataWithKey decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                DropdownDO ddlRequest = JsonConvert.DeserializeObject<DropdownDO>(decryptedResult.DecryptedJsonData);

                var result = DDLBAL.GetDistrictsByStateId(ddlRequest, userId);

                if (result == null || result.Data.Count == 0)
                {
                    string message = "No districts found for the given state.";
                    LoggerDAL.FnStoreErrorLog("DropdownController", "GetDistrictsByStateId", message, "", "", userId);
                    return NotFound(ApiResponse<object>.FailureResponse(message));
                }

                 return Ok(Cryptohelper.EncryptResponse(result, decryptedResult.ReqAesKey, decryptedResult.Iv));
              
            }
            catch (Exception ex)
            {
                string message = "Error fetching districts.";
                LoggerDAL.FnStoreErrorLog("DropdownController", "GetDistrictsByStateId", message, ex.StackTrace, ex.Message, userId);
                return StatusCode(500, ApiResponse<object>.FailureResponse(message));
            }
        }


        [HttpPost("GetRegionsByDistrictId")]
        [ValidateToken]
        [ValidateEncryptedRequest]
        public IActionResult GetRegionsByDistrictId([FromBody] EncryptedRequest request)
        {
            int userId = 0;
           int.TryParse(HttpContext.Items["UserId"]?.ToString(), out userId);

            try
            {
                DecryptedJsonDataWithKey decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                DropdownDO ddlRequest = JsonConvert.DeserializeObject<DropdownDO>(decryptedResult.DecryptedJsonData);

                var result = DDLBAL.GetRegionsByDistrictId(ddlRequest, userId);

                if (result == null || result.Data.Count == 0)
                {
                    string message = "No regions found for the given district.";
                    LoggerDAL.FnStoreErrorLog("DropdownController", "GetRegionsByDistrictId", message, "", "", userId);
                    return NotFound(ApiResponse<object>.FailureResponse(message));
                }

                 return Ok(Cryptohelper.EncryptResponse(result, decryptedResult.ReqAesKey, decryptedResult.Iv));
            }
            catch (Exception ex)
            {
                string message = "Error fetching regions.";
                LoggerDAL.FnStoreErrorLog("DropdownController", "GetRegionsByDistrictId", message, ex.StackTrace, ex.Message, userId);
                return StatusCode(500, ApiResponse<object>.FailureResponse(message));
            }
        }


        [HttpPost("GetDivisionsByRegionId")]
        [ValidateToken]
        [ValidateEncryptedRequest]
        public IActionResult GetDivisionsByRegionId([FromBody] EncryptedRequest request)
        {
            int userId = 0;
            int.TryParse(HttpContext.Items["UserId"]?.ToString(), out userId);

            try
            {
                DecryptedJsonDataWithKey decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                DropdownDO ddlRequest = JsonConvert.DeserializeObject<DropdownDO>(decryptedResult.DecryptedJsonData);

                var result = DDLBAL.GetDivisionsByRegionId(ddlRequest, userId);

                if (result == null || result.Data.Count == 0)
                {
                    string message = "No divisions found for the given region.";
                    LoggerDAL.FnStoreErrorLog("DropdownController", "GetDivisionsByRegionId", message, "", "", userId);
                    return NotFound(ApiResponse<object>.FailureResponse(message));
                }

                 return Ok(Cryptohelper.EncryptResponse(result, decryptedResult.ReqAesKey, decryptedResult.Iv));
            }
            catch (Exception ex)
            {
                string message = "Error fetching divisions.";
                LoggerDAL.FnStoreErrorLog("DropdownController", "GetDivisionsByRegionId", message, ex.StackTrace, ex.Message, userId);
                return StatusCode(500, ApiResponse<object>.FailureResponse(message));
            }
        }

        [HttpPost("GetLabMaster")]
        [ValidateToken]
        [ValidateEncryptedRequest]
        public IActionResult GetLabMaster([FromBody] EncryptedRequest request)
        {
            int UserId = 0;
            int.TryParse(HttpContext.Items["UserId"]?.ToString(), out UserId);

            try
            {
                DecryptedJsonDataWithKey decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                DropdownDO DDL = JsonConvert.DeserializeObject<DropdownDO>(decryptedResult.DecryptedJsonData);

                var result = DDLBAL.LabMaster(UserId);

                if (result == null)
                {
                    string message = "Lab master list fetch failed or returned no data.";
                    LoggerDAL.FnStoreErrorLog("SampleReceiverController", "GetLabMaster", message, "", "", UserId);
                    return NotFound(ApiResponse<object>.FailureResponse(message));
                }

                return Ok(Cryptohelper.EncryptResponse(result, decryptedResult.ReqAesKey, decryptedResult.Iv));
            }
            catch (Exception ex)
            {
                string message = "An error occurred during fetch Lab master.";
                LoggerDAL.FnStoreErrorLog("SampleReceiverController", "GetLabMaster", message, ex.StackTrace, ex.Message, UserId);
                return StatusCode(500, ApiResponse<object>.FailureResponse(message));
            }
        }



        [HttpPost("GetSealNumber")]
        [ValidateToken]
        [ValidateEncryptedRequest]
        public IActionResult GetSealNumber([FromBody] EncryptedRequest request)
        {
            int requestId = 0;
            try
            {
                DecryptedJsonDataWithKey decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                getSealNumberDO DDL = JsonConvert.DeserializeObject<getSealNumberDO>(decryptedResult.DecryptedJsonData);

                requestId = DDL.RequestId;

                var result = DDLBAL.GetSealNumber(requestId);

                if (result == null || !result.Success || result.Data == null || result.Data.Count == 0)
                {
                    string message = "Seal number fetch failed or returned no data.";
                    LoggerDAL.FnStoreErrorLog("SampleReceiverController", "GetSealNumber", message, "", "", requestId);
                    return NotFound(ApiResponse<object>.FailureResponse(message));
                }

                return Ok(Cryptohelper.EncryptResponse(result, decryptedResult.ReqAesKey, decryptedResult.Iv));
                //return Ok(result);
            }
            catch (Exception ex)
            {
                string message = "An error occurred during fetch Seal Number.";
                LoggerDAL.FnStoreErrorLog("SampleReceiverController", "GetSealNumber", message, ex.StackTrace, ex.Message, requestId);
                return StatusCode(500,
                       ApiResponse<object>.FailureResponse(message));
            }
        }






    }
}
