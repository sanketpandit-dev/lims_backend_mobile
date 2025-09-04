using System.Text.RegularExpressions;
using BusinessAccessLayer;
using BusinessAccessLayer.Log;
using DataAccessLayer;
using DataAccessLayer.Log;
using DataObject;
using DataObject.Errorlog;
using LIMS.Common;
using LIMS.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;

namespace LIMS.Controllers
{
    [Route("api/mobile")]

    public class LoginController : BaseApiController
    {
        RequestResponseLogDO requestLog = new RequestResponseLogDO();

        [HttpPost("Login")]
        [ValidateEncryptedRequest]  
        public IActionResult Login([FromBody] EncryptedRequest request)
        {
            try
            {
                DecryptedJsonDataWithKey result = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);

                LoginDO loginDO = JsonConvert.DeserializeObject<LoginDO>(result.DecryptedJsonData);

                if (loginDO == null ||
                    string.IsNullOrWhiteSpace(loginDO.Username) ||
                    string.IsNullOrWhiteSpace(loginDO.Password))
                {
                    string message = "Invalid login details. Username or password is missing.";
                    LoggerDAL.FnStoreErrorLog("LoginController", "Login", message, "", "", 0);
                    return Unauthorized(ApiResponse<object>.FailureResponse(message));
                }

                requestLog.Action = "SAVE_REQUEST";
                requestLog.JsonData = JsonConvert.SerializeObject(loginDO);
                requestLog.ApiUrl = this.Request.Path;
                requestLog.UserId = 0;

                int id = LoggerBAL.FnStoreResponseRequestLog(requestLog);

                var loginBAL = new LoginBAL();
                var loginResponse = loginBAL.UserDetails(loginDO);

                if (loginResponse != null && loginResponse.Success)
                {
                    var encryptedResponse = Cryptohelper.EncryptResponse(loginResponse, result.ReqAesKey, result.Iv);

                    Response.Cookies.Append("AuthToken", loginResponse.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    });

                    requestLog.Action = "SAVE_RESPONSE";
                    requestLog.JsonData = JsonConvert.SerializeObject(loginResponse);
                    requestLog.ApiUrl = this.Request.Path;
                    requestLog.UserId = loginResponse.UserId;
                    requestLog.NewId = id;
                    LoggerBAL.FnStoreResponseRequestLog(requestLog);

                    if (loginResponse.PassResetFlag == "1")
                    {
                        EmailVerificationDO emailDO = new EmailVerificationDO
                        {
                            UserMailId = loginResponse.Email,
                        };

                        EmailVerificationResponseDO emailEntity = loginBAL.EmailOTPVerification(emailDO);

                        if (emailEntity == null || string.IsNullOrEmpty(emailEntity.Email) || !emailEntity.Success || emailEntity.StatusCode == 404)
                        {
                            string message = emailEntity?.Message ?? "Email verification failed.";
                            LoggerDAL.FnStoreErrorLog("LoginController", "EmailVerification", message, "", "", 0);
                            return HandleFailureResponse<string>(message, emailEntity?.StatusCode ?? 400);
                        }

                        var rng = new Random();
                        string verificationCode = rng.Next(100000, 999999).ToString();
                        string subject = "Your Verification Code";

                        var emailTemplateReq = new GetEmailTemplatereuestDO
                        {
                            Username = emailEntity.Username,
                            VerificationCode = verificationCode
                        };

                        var emailTemplate = loginBAL.GetLoginEmailBody(emailTemplateReq);
                        if (emailTemplate.Success && emailTemplate.StatusCode == 200)
                        {
                            string plainTextBody = Regex.Replace(emailTemplate.EmailBody ?? string.Empty, "<.*?>", string.Empty);

                            Emailhelper.SendEmail(emailEntity.Email, subject, plainTextBody);
                        }
                        else
                        {
                            return HandleFailureResponse<string>("Failed to generate email template.", 500);
                        }

                        // Save OTP in Firebase
                        var VerificationDO = new VerificationDO
                        {
                            Success = true,
                            Message = "Email exists. Verification code sent.",
                            verificationCode = verificationCode,
                            Email = emailEntity.Email,
                            UserId = emailEntity.UserId,
                            Username = loginResponse.Username
                        };
                        loginBAL.SaveFirebaseOtp(VerificationDO);
                    }

                    return Ok(encryptedResponse); // ✅ Still returning encrypted response
                }
                else
                {
                    string message = loginResponse?.Message ?? "Login failed due to unknown error";
                    LoggerDAL.FnStoreErrorLog("LoginController", "Login", message, "", "", 0);

                    return HandleFailureResponse<string>(message, loginResponse?.StatusCode ?? 400);
                }
            }
            catch (Exception ex)
            {
                string message = "Login failed!";
                LoggerDAL.FnStoreErrorLog("LoginController", "Login", message, ex.StackTrace, ex.Message, 0);

                return StatusCode(500, ApiResponse<object>.FailureResponse(message));
            }
        }



        //[HttpPost("Login")]
        // [ValidateEncryptedRequest]  // 🔒 commented for plain JSON input
        //public IActionResult Login([FromBody] EncryptedRequest request)
        //{
        //    try
        //    {
        //        // 🔒 Decrypt request
        //        DecryptedJsonDataWithKey result = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);

        //        LoginDO loginDO = JsonConvert.DeserializeObject<LoginDO>(result.DecryptedJsonData);

        //        if (loginDO == null ||
        //            string.IsNullOrWhiteSpace(loginDO.Username) ||
        //            string.IsNullOrWhiteSpace(loginDO.Password))
        //        {
        //            string message = "Invalid login details. Username or password is missing.";
        //            LoggerDAL.FnStoreErrorLog("LoginController", "Login", message, "", "", 0);
        //            return Unauthorized(ApiResponse<object>.FailureResponse(message));
        //        }

        //        requestLog.Action = "SAVE_REQUEST";
        //        requestLog.JsonData = JsonConvert.SerializeObject(loginDO);
        //        requestLog.ApiUrl = this.Request.Path;
        //        requestLog.UserId = 0;

        //        int id = LoggerBAL.FnStoreResponseRequestLog(requestLog);

        //        var loginBAL = new LoginBAL();
        //        var loginResponse = loginBAL.UserDetails(loginDO);

        //        if (loginResponse.Success && loginResponse != null)
        //        {
        //            var encryptedResponse = Cryptohelper.EncryptResponse(loginResponse,result.ReqAesKey,result.Iv); 

        //            Response.Cookies.Append("AuthToken", loginResponse.Token, new CookieOptions
        //            {
        //                HttpOnly = true,
        //                Secure = true,
        //                SameSite = SameSiteMode.Strict,
        //                Expires = DateTimeOffset.UtcNow.AddHours(1)
        //            });

        //            // Log response
        //            requestLog.Action = "SAVE_RESPONSE";
        //            requestLog.JsonData = JsonConvert.SerializeObject(loginResponse);
        //            requestLog.ApiUrl = this.Request.Path;
        //            requestLog.UserId = loginResponse.UserId;
        //            requestLog.NewId = id;
        //            LoggerBAL.FnStoreResponseRequestLog(requestLog);

        //            if (loginResponse.PassResetFlag == "1")
        //            {
        //                EmailVerificationDO emailDO = new EmailVerificationDO
        //                {
        //                    UserMailId = loginResponse.Email,
        //                };

        //                EmailVerificationResponseDO emailEntity = loginBAL.EmailOTPVerification(emailDO);

        //                if (emailEntity == null || string.IsNullOrEmpty(emailEntity.Email) || !emailEntity.Success || emailEntity.StatusCode == 404)
        //                {
        //                    string message = emailEntity?.Message ?? "Email verification failed.";
        //                    LoggerDAL.FnStoreErrorLog("LoginController", "EmailVerification", message, "", "", 0);
        //                    return HandleFailureResponse<string>(message, emailEntity?.StatusCode ?? 400);
        //                }

        //                // Send OTP
        //                var rng = new Random();
        //                string verificationCode = rng.Next(100000, 999999).ToString();
        //                string subject = "Your Verification Code";
        //                var emailTemplateReq = new GetEmailTemplatereuestDO
        //                {
        //                    Username = emailEntity.Username,
        //                    VerificationCode = verificationCode
        //                };

        //                var emailTemplate = loginBAL.GetEmailBody(emailTemplateReq);
        //                if (emailTemplate.Success && emailTemplate.StatusCode == 200)
        //                {
        //                    Emailhelper.SendEmail(emailEntity.Email, subject, emailTemplate.EmailBody);
        //                }
        //                else
        //                {
        //                    return HandleFailureResponse<string>("Failed to generate email template.", 500);
        //                }

        //                var VerificationDO = new VerificationDO
        //                {
        //                    Success = true,
        //                    Message = "Email exists. Verification code sent.",
        //                    verificationCode = verificationCode,
        //                    Email = emailEntity.Email,
        //                    UserId = emailEntity.UserId,
        //                    Username = loginResponse.Username
        //                };
        //                loginBAL.SaveFirebaseOtp(VerificationDO);
        //            }



        //            return Ok(encryptedResponse); // Return plain JSON
        //        }
        //        else
        //        {
        //            string message = loginResponse?.Message ?? "Login failed due to unknown error";
        //            LoggerDAL.FnStoreErrorLog("LoginController", "Login", message, "", "", 0);

        //            return HandleFailureResponse<string>(message, loginResponse?.StatusCode ?? 400);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = "Login failed !";
        //        LoggerDAL.FnStoreErrorLog("LoginController", "Login", message, ex.StackTrace, ex.Message, 0);

        //        return StatusCode(500, ApiResponse<object>.FailureResponse(message));
        //    }
        //}



        [HttpPost("ChangePassword")]
        [ValidateToken]
        [ValidateEncryptedRequest]
        public IActionResult ChangePassword([FromBody] EncryptedRequest request)
        {
            try
            {
                int UserId = 0;
                int.TryParse(HttpContext.Items["UserId"].ToString(), out UserId);

                DecryptedJsonDataWithKey decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                ChangePassDO passDO = JsonConvert.DeserializeObject<ChangePassDO>(decryptedResult.DecryptedJsonData);

                if (passDO == null ||
                    string.IsNullOrWhiteSpace(passDO.Username) ||
                    string.IsNullOrWhiteSpace(passDO.NewPassword) ||
                    string.IsNullOrWhiteSpace(passDO.Password))
                {
                    LoggerDAL.FnStoreErrorLog("LoginController", "ChangePassword",
                        "Invalid user data. Please check all required fields.", "", "", UserId);

                    return BadRequest(ApiResponse<object>
                        .FailureResponse("Invalid user data. Please check all required fields."));
                }

                ChangePassResponseDO result = new LoginBAL().ChangePassword(passDO, UserId);

                if (!result.Success)
                {
                    LoggerDAL.FnStoreErrorLog("LoginController", "ChangePassword", result.Message, "", "", UserId);

                    return HandleFailureResponse<string>(result.Message, result.StatusCode);
                }

                return Ok(Cryptohelper.EncryptResponse(result, decryptedResult.ReqAesKey, decryptedResult.Iv));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("LoginController", "ChangePassword",
                    "Login failed due to exception", ex.StackTrace, ex.Message, 0);

                return StatusCode(500, ApiResponse<EncryptedResponse>
                    .FailureResponse("An error occurred during password change."));
            }
        }



        [HttpPost("ForgotPassword")]
        [ValidateEncryptedRequest]
        public IActionResult ForgotPassword([FromBody] EncryptedRequest request)
        {
            try
            {

                DecryptedJsonDataWithKey decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);

                ForgetPasswordDO forgotDO = JsonConvert.DeserializeObject<ForgetPasswordDO>(decryptedResult.DecryptedJsonData);

                if (forgotDO == null ||
                       string.IsNullOrWhiteSpace(forgotDO.Password) || forgotDO.UserId <= 0)
                {
                    string message = "Invalid data. Username or Email is required.";

                    LoggerDAL.FnStoreErrorLog("LoginController", "ForgotPassword",
                 message, "", "", forgotDO.UserId);

                    return BadRequest(
                        ApiResponse<object>
                        .FailureResponse(message)
                        );

                }

                var loginBAL = new LoginBAL();
                ForgotPasswordResponseDO result = loginBAL.ForgotPassword(forgotDO);

                if (!result.Success)
                {
                    LoggerDAL.FnStoreErrorLog("LoginController", "ForgotPassword",
                result.Message, "", "", forgotDO.UserId);
                    return HandleFailureResponse<string>(
                      result.Message, result.StatusCode
                       );
                }


                LoggerBAL.FnStoreResponseRequestLog(requestLog);

                return Ok(Cryptohelper.EncryptResponse(result, decryptedResult.ReqAesKey, decryptedResult.Iv));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("LoginController", "ForgotPassword",
         "Login failed due to exception", ex.StackTrace, ex.Message, 0);

                return BadRequest(
                    ApiResponse<object>
                    .FailureResponse("An error occurred during password recovery.")
                    );
            }
        }

        //[HttpPost("TestEncrypt")]
        //public IActionResult TestEncrypt()
        //{
        //    var login = new VerificationDO
        //    {

        //        User_Id = 6,
        //        Email = "shubhada.alphonsol@gmail.com",
        //        verificationCode = "197208"

        //    };

        //    string json = JsonConvert.SerializeObject(login);
        //    var (encryptedData, iv, aesKey) = Cryptohelper.AES_Encrypt(json);
        //    byte[] encryptedAESKey = Cryptohelper.RSA_Encrypt(aesKey, Cryptohelper.PublicKey);

        //    var request = new EncryptedRequest
        //    {
        //        EncryptedData = Convert.ToBase64String(encryptedData),
        //        IV = Convert.ToBase64String(iv),
        //        EncryptedAESKey = Convert.ToBase64String(encryptedAESKey)
        //    };

        //    return Ok(request);
        //}

        [HttpPost("EmailVerification")]
        [ValidateEncryptedRequest]

        public IActionResult EmailVerification([FromBody] EncryptedRequest request)
        {
            try
            {
                DecryptedJsonDataWithKey decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                EmailVerificationDO emailDO = JsonConvert.DeserializeObject<EmailVerificationDO>(decryptedResult.DecryptedJsonData);
                if (emailDO == null || string.IsNullOrWhiteSpace(emailDO.UserMailId))
                {
                    string message = "Invalid input. Email is required.";
                    LoggerDAL.FnStoreErrorLog("LoginController", "EmailVerification", message

                 , "", "", 0);

                    return BadRequest(
                        ApiResponse<object>
                        .FailureResponse(message)
                        );
                }
                LoginBAL loginBAL = new LoginBAL();
                EmailVerificationResponseDO emailEntity = loginBAL.EmailVerification(emailDO);
                if (emailEntity == null || string.IsNullOrEmpty(emailEntity.Email) || emailEntity.Success == false || emailEntity.StatusCode == 404)
                {
                    string message = emailEntity?.Message ?? "Email verification failed.";
                    LoggerDAL.FnStoreErrorLog("LoginController", "EmailVerification", message, "", "", 0);
                    return HandleFailureResponse<string>(message, emailEntity?.StatusCode ?? 400);
                }
                var rng = new Random();
                string verificationCode = rng.Next(100000, 999999).ToString();
                string subject = "Your Verification Code";
                var emailTemplateReq = new GetEmailTemplatereuestDO
                {
                    Username = emailEntity.Username,
                    VerificationCode = verificationCode

                };
                var emailTemplate = loginBAL.GetEmailBody(emailTemplateReq);
                if (emailTemplate.Success && emailTemplate.StatusCode == 200)
                {
                    Emailhelper.SendEmail(emailEntity.Email, subject, emailTemplate.EmailBody);
                }
                else
                {
                    return HandleFailureResponse<string>("Failed to generate email template.", 500);
                }
                var VerificationDO = new VerificationDO

                {
                    Success = true,
                    Message = "Email exists. Verification code sent.",
                    verificationCode = verificationCode,
                    Email = emailEntity.Email,
                    UserId = emailEntity.UserId

                };

                var Verifycode = loginBAL.InsertVerificationcode(VerificationDO);
                return Ok(Cryptohelper.EncryptResponse(VerificationDO, decryptedResult.ReqAesKey, decryptedResult.Iv));
            }

            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("LoginController", "EmailVerification",
                        "Login failed due to exception", ex.StackTrace, ex.Message, 0);
                return StatusCode(500, ApiResponse<object>.FailureResponse("An error occurred during email verification."));

            }

        }


        [HttpPost("VerifyOtp")]
        [ValidateEncryptedRequest]
        public IActionResult VerifyOtp([FromBody] EncryptedRequest request)
        {
            try
            {
                DecryptedJsonDataWithKey decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                VerificationDO OTP = JsonConvert.DeserializeObject<VerificationDO>(decryptedResult.DecryptedJsonData);

                if (OTP == null || string.IsNullOrWhiteSpace(OTP.verificationCode))
                {
                    string message = "Invalid input. Email is required.";
                    LoggerDAL.FnStoreErrorLog("LoginController", "ForgotPassword", message
                       , "", "", 0);

                    return BadRequest(
                        ApiResponse<object>
                        .FailureResponse("Invalid input. Email is required.")
                        );

                }

                var loginBAL = new LoginBAL();
                VerifyOtpResponseDO result = loginBAL.VerifyOtp(OTP);

                if (!result.Success)
                {
                    LoggerDAL.FnStoreErrorLog("LoginController", "ForgotPassword", result.Message
              , "", "", 0);

                    return BadRequest(
                       ApiResponse<object>
                       .FailureResponse(result.Message)
                       );
                }

                return Ok(Cryptohelper.EncryptResponse(result, decryptedResult.ReqAesKey, decryptedResult.Iv));

            }
            catch (Exception ex)
            {
                string message = "An error occurred during verifying user.";
                LoggerDAL.FnStoreErrorLog("LoginController", "ForgotPassword", message
              , "", "", 0);

                return BadRequest(
                    ApiResponse<object>
                    .FailureResponse(message)
                    );
            }
        }



        [HttpPost("VerifyLoginOtp")]
        [ValidateEncryptedRequest]
        public IActionResult VerifyLoginOtp([FromBody] EncryptedRequest request)
        {
            try
            {
                DecryptedJsonDataWithKey decryptedResult = Cryptohelper.DecryptRequest<DecryptedJsonDataWithKey>(request);
                VerificationDO OTP = JsonConvert.DeserializeObject<VerificationDO>(decryptedResult.DecryptedJsonData);

                if (OTP == null || string.IsNullOrWhiteSpace(OTP.verificationCode))
                {
                    string message = "Invalid input. Email is required.";
                    LoggerDAL.FnStoreErrorLog("LoginController", "ForgotPassword", message, "", "", 0);

                    return BadRequest(
                        ApiResponse<object>.FailureResponse("Invalid input. Email is required.")
                    );
                }

                var loginBAL = new LoginBAL();
                VerifyOtpResponseDO result = loginBAL.VerifyLoginOtp(OTP);

                if (!result.Success)
                {
                    LoggerDAL.FnStoreErrorLog("LoginController", "ForgotPassword", result.Message, "", "", 0);

                    return BadRequest(
                        ApiResponse<object>.FailureResponse(result.Message)
                    );
                }

                return Ok(Cryptohelper.EncryptResponse(result, decryptedResult.ReqAesKey, decryptedResult.Iv));

                // Return plain response
                //return Ok(result);
            }
            catch (Exception ex)
            {
                string message = "An error occurred during verifying user.";
                LoggerDAL.FnStoreErrorLog("LoginController", "ForgotPassword", message, "", "", 0);

                return BadRequest(
                    ApiResponse<object>.FailureResponse(message)
                );
            }
        }


    }
}
