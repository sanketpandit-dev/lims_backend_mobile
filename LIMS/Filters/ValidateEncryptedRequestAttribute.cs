using BusinessAccessLayer.Log;
using DataAccessLayer.Log;
using DataObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LIMS.Filters
{
    public class ValidateEncryptedRequestAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.TryGetValue("request", out var requestObj) || requestObj is not EncryptedRequest request)
            {
                string message = "Invalid request format.";
                LoggerDAL.FnStoreErrorLog("LoginController", "Login", message, "", "", 0);
              
                context.Result = new BadRequestObjectResult(
                    ApiResponse<object>.FailureResponse(message)
                );
                return;
            }

            if (string.IsNullOrWhiteSpace(request.EncryptedAESKey) ||
                string.IsNullOrWhiteSpace(request.IV) ||
                string.IsNullOrWhiteSpace(request.EncryptedData))
            {

                string message = "Missing encryption parameters.";
                LoggerDAL.FnStoreErrorLog("LoginController", "Login", message, "", "", 0);

                context.Result = new BadRequestObjectResult(
                    ApiResponse<object>.FailureResponse(message)
                );
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
