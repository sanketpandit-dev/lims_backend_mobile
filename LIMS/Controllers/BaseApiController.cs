using DataObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LIMS.Controllers
{
    
    public class BaseApiController : ControllerBase

    {
        protected IActionResult HandleFailureResponse<T>(string message, int statusCode)
        {
            var response = new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
            };

            return statusCode switch
            {
                400 => BadRequest(response),
                401 => Unauthorized(response),
                403 => Forbid(),
                404 => NotFound(response),
              409=>  Conflict(response),
                _ => StatusCode(statusCode, response)
            };
        }


    }
}
