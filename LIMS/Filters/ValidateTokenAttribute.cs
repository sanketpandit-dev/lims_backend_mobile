using BusinessAccessLayer;
using DataObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LIMS.Filters
{
    public class ValidateTokenAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            var tokenValidation = AuthTokenBAL.CheckValidToken(authHeader);

            if (string.IsNullOrWhiteSpace(authHeader) || tokenValidation[0] != "true")
            {
                context.Result = new UnauthorizedObjectResult(
                    ApiResponse<object>.FailureResponse("Unauthorized: " + (tokenValidation[1] ?? "Missing token"))
                );
                return;
            }
            var userId = AuthTokenBAL.GetUserIdFromToken(authHeader);
           
          

            if (userId == null)
            {
                context.Result = new UnauthorizedObjectResult(
                    ApiResponse<object>.FailureResponse("Unauthorized: " + (tokenValidation[1] ?? "Missing token"))
                ); 
                    return;
            }


            context.HttpContext.Items["UserId"] = userId;
            base.OnActionExecuting(context);
        }
    }
}
