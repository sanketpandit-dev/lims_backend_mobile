using DataObject;
using LIMS.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LIMS.Filters
{
    public class DecryptEncryptedRequestAttribute : ActionFilterAttribute
    {
        private readonly Type _targetType;
        private readonly string _outputParameterName;

        public DecryptEncryptedRequestAttribute(Type targetType, string outputParameterName = "decryptedData")
        {
            _targetType = targetType;
            _outputParameterName = outputParameterName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.TryGetValue("request", out var requestObj) || requestObj is not EncryptedRequest request)
            {
                context.Result = new BadRequestObjectResult(
                    ApiResponse<object>.FailureResponse("Invalid request format.")
                );
                return;
            }

            try
            {
                var method = typeof(Cryptohelper)
                    .GetMethod("DecryptRequest")
                    ?.MakeGenericMethod(_targetType);

                if (method == null)
                {
                    context.Result = new BadRequestObjectResult(
                        ApiResponse<object>.FailureResponse("Decryption method not found.")
                    );
                    return;
                }

                var decrypted = method.Invoke(null, new object[] { request });
                context.ActionArguments[_outputParameterName] = decrypted;
            }
            catch (Exception ex)
            {
                context.Result = new BadRequestObjectResult(
                    ApiResponse<object>.FailureResponse("Decryption failed: " + ex.Message)
                );
            }
        }
    }
}
