using BusinessAccessLayer.Log;
using DataAccessLayer;
using DataAccessLayer.Log;
using DataObject;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer
{
    public class LoginBAL
    {

        public ChangePassResponseDO ChangePassword(ChangePassDO passDO, int userid)
        {
            ChangePassResponseDO changePassResponse = new ChangePassResponseDO();

            if (passDO != null)
            {
                try
                {
                    if (passDO.Password == passDO.NewPassword)
                    {
                        changePassResponse.Success = false;
                        changePassResponse.Message = "New password must be different from current password.";
                        changePassResponse.StatusCode = 400;
                        LoggerDAL.FnStoreErrorLog("LoginBAL", "ChangePassword", "New password must be different from current password.", "", "", userid);

                        return changePassResponse;
                    }

                    LoginDAL loginparamDAL = new LoginDAL();
                    List<ChangePassResponseDO> listDO = loginparamDAL.ChangePassword(passDO, userid);

                    if (listDO.Count > 0)
                    {
                        changePassResponse = listDO[0];
                    }

                }
                catch (Exception ex)
                {
                    changePassResponse.Success = false;
                    changePassResponse.Message = "Internal error occurred.";

                    LoggerDAL.FnStoreErrorLog("LoginBAL", "ChangePassword",
                      "Internal error occurred.", ex.StackTrace, ex.Message, userid);

                }
            }
            else
            {
                changePassResponse.Success = false;
                changePassResponse.Message = "Invalid input.";
                changePassResponse.StatusCode = 400;
            }

            return changePassResponse;
        }


        public EmailVerificationResponseDO EmailVerification(EmailVerificationDO emailDO)
        {
            List<EmailVerificationResponseDO> VerificationResponse = new List<EmailVerificationResponseDO>();

            EmailVerificationResponseDO VerificationResult = new EmailVerificationResponseDO();

            try
            {
                LoginDAL loginparamDAL = new LoginDAL();
                VerificationResponse = loginparamDAL.EmailVerification(emailDO);
                if (VerificationResponse.Count > 0)
                {
                    VerificationResult = VerificationResponse[0];
                }

            }
            catch (Exception ex)
            {

                VerificationResult.Success = false;
                VerificationResult.Message = "Internal error occurred.";
                LoggerDAL.FnStoreErrorLog("LoginBAL", "EmailVerification",
                    "Internal error occurred.", ex.StackTrace, ex.Message, 0);



            }
            return VerificationResult;
        }


        public EmailVerificationResponseDO EmailOTPVerification(EmailVerificationDO emailDO)
        {
            List<EmailVerificationResponseDO> VerificationResponse = new List<EmailVerificationResponseDO>();

            EmailVerificationResponseDO VerificationResult = new EmailVerificationResponseDO();

            try
            {
                LoginDAL loginparamDAL = new LoginDAL();
                VerificationResponse = loginparamDAL.EmailOTPVerification(emailDO);
                if (VerificationResponse.Count > 0)
                {
                    VerificationResult = VerificationResponse[0];
                }

            }
            catch (Exception ex)
            {

                VerificationResult.Success = false;
                VerificationResult.Message = "Internal error occurred.";
                LoggerDAL.FnStoreErrorLog("LoginBAL", "EmailVerification",
                    "Internal error occurred.", ex.StackTrace, ex.Message, 0);



            }
            return VerificationResult;
        }
        public ForgotPasswordResponseDO ForgotPassword(ForgetPasswordDO forgotDO)
        {
            List<ForgotPasswordResponseDO> listDO = new List<ForgotPasswordResponseDO>();
            ForgotPasswordResponseDO forgotPasswordResponse = new ForgotPasswordResponseDO();

            if (forgotDO != null)
            {
                try
                {
                    LoginDAL loginparamDAL = new LoginDAL();
                    listDO = loginparamDAL.ForgotPassword(forgotDO);
                    if (listDO.Count > 0)
                    {
                        forgotPasswordResponse = listDO[0];

                    }
                }
                catch (Exception ex)
                {
                    forgotPasswordResponse.Success = false;
                    forgotPasswordResponse.Message = "Internal error occurred.";
                    LoggerDAL.FnStoreErrorLog("LoginBAL", "ForgotPassword",
                  "Internal error occurred.", ex.StackTrace, ex.Message, 0);

                }
            }

            return forgotPasswordResponse;
        }


        public UserLoginData UserDetails(LoginDO login)
        {

            List<UserLoginData> listDO = new List<UserLoginData>();
            UserLoginData result = new UserLoginData();
            //UserLoginData result = null;
            if (login != null)
            {
                try
                {
                    LoginDAL loginparamDAL = new LoginDAL();
                    listDO = loginparamDAL.LoginDetailsDAL(login);

                    if (listDO.Count > 0)
                    {

                        result = listDO[0];

                        if (result.Success)
                        {
                            result.Token = AuthTokenBAL.GenerateJwtToken(listDO);

                        }
                        else
                        {
                            result.Token = null;

                        }


                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "Data Not Found.";
                    }
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Message = "Exception occurred.";
                    LoggerDAL.FnStoreErrorLog("LoginBAL", "UserDetails", "Internal error occurred.", ex.StackTrace, ex.Message, 0);

                }
            }
            else
            {
                result.Success = false;
                result.Message = "Invalid input.";

            }

            return result;
        }

        public EmailVerificationResponseDO InsertVerificationcode(VerificationDO EmailDO)
        {
            List<EmailVerificationResponseDO> listDO = new List<EmailVerificationResponseDO>();

            EmailVerificationResponseDO VerificationResponse = new EmailVerificationResponseDO();

            try
            {
                LoginDAL loginparamDAL = new LoginDAL();
                listDO = loginparamDAL.SaveOtp(EmailDO);
                if (listDO.Count > 0)
                {
                    VerificationResponse = listDO[0];
                }
            }
            catch (Exception ex)
            {
                VerificationResponse.Success = false;
                VerificationResponse.Message = "Internal error occurred.";
                LoggerDAL.FnStoreErrorLog("LoginBAL", "InsertVerificationcode", "Internal error occurred.", ex.StackTrace, ex.Message, 0);


            }

            return VerificationResponse;
        }


        public List<UserVerificationResponseDO> SaveFirebaseOtp(VerificationDO otpDO)
        {
            List<UserVerificationResponseDO> listDO = new List<UserVerificationResponseDO>();

            try
            {
                LoginDAL loginparamDAL = new LoginDAL();
                listDO = loginparamDAL.SaveFirebaseOtp(otpDO);

                if (listDO == null || listDO.Count == 0)
                {
                    // no response from DB
                    listDO = new List<UserVerificationResponseDO>
            {
                new UserVerificationResponseDO
                {
                    Success = false,
                    ResponseMsg = "Failed to save OTP."
                }
            };
                }
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("LoginBAL", "SaveFirebaseOtp",
                    "Error while saving Firebase OTP.", ex.StackTrace, ex.Message, otpDO.UserId);

                listDO = new List<UserVerificationResponseDO>
        {
            new UserVerificationResponseDO
            {
                Success = false,
                ResponseMsg = "Internal error occurred while saving OTP."
            }
        };
            }

            return listDO;
        }

        public VerifyOtpResponseDO VerifyOtp(VerificationDO OTP)
        {
            List<VerifyOtpResponseDO> verifyOtpResponse = new List<VerifyOtpResponseDO>();
            VerifyOtpResponseDO verifyOtpResult = new VerifyOtpResponseDO();

            try
            {
                LoginDAL loginparamDAL = new LoginDAL();
                verifyOtpResponse = loginparamDAL.VerifyOtp(OTP);

                if (verifyOtpResponse != null && verifyOtpResponse.Count > 0)
                {
                    verifyOtpResult = verifyOtpResponse[0];
                }
                else
                {
                    verifyOtpResult.Success = false;
                    verifyOtpResult.Message = "Invalid OTP or user details.";
                    verifyOtpResult.StatusCode = 400;
                }
            }
            catch (Exception ex)
            {
                verifyOtpResult.Success = false;
                verifyOtpResult.Message = "Error occurred during OTP verification.";
                verifyOtpResult.StatusCode = 500;
                LoggerDAL.FnStoreErrorLog("LoginBAL", "VerifyOtp", "Error occurred during OTP verification.", ex.StackTrace, ex.Message, 0);

            }

            return verifyOtpResult;
        }
        public VerifyOtpResponseDO VerifyLoginOtp(VerificationDO OTP)
        {
            List<VerifyOtpResponseDO> verifyOtpResponse = new List<VerifyOtpResponseDO>();
            VerifyOtpResponseDO verifyOtpResult = new VerifyOtpResponseDO();

            try
            {
                LoginDAL loginparamDAL = new LoginDAL();
                verifyOtpResponse = loginparamDAL.VerifyLoginOtp(OTP);

                if (verifyOtpResponse != null && verifyOtpResponse.Count > 0)
                {
                    verifyOtpResult = verifyOtpResponse[0];
                }
                else
                {
                    verifyOtpResult.Success = false;
                    verifyOtpResult.Message = "Invalid OTP or user details.";
                    verifyOtpResult.StatusCode = 400;
                }
            }
            catch (Exception ex)
            {
                verifyOtpResult.Success = false;
                verifyOtpResult.Message = "Error occurred during OTP verification.";
                verifyOtpResult.StatusCode = 500;
                LoggerDAL.FnStoreErrorLog("LoginBAL", "VerifyOtp", "Error occurred during OTP verification.", ex.StackTrace, ex.Message, 0);

            }

            return verifyOtpResult;
        }

        public GetEmailTemplateDO GetEmailBody(GetEmailTemplatereuestDO emailDO)

        {

            List<GetEmailTemplateDO> VerificationResponse = new List<GetEmailTemplateDO>();

            GetEmailTemplateDO VerificationResult = new GetEmailTemplateDO();

            try

            {

                LoginDAL loginparamDAL = new LoginDAL();

                VerificationResponse = loginparamDAL.GetEmailTemplate(emailDO);

                if (VerificationResponse.Count > 0)

                {

                    VerificationResult = VerificationResponse[0];

                }

            }

            catch (Exception ex)

            {

                VerificationResult.Success = false;

                VerificationResult.Message = "Internal error occurred.";

                LoggerDAL.FnStoreErrorLog("LoginBAL", "EmailVerification",

                    "Internal error occurred.", ex.StackTrace, ex.Message, 0);

            }

            return VerificationResult;

        }

        public GetEmailTemplateDO GetLoginEmailBody(GetEmailTemplatereuestDO emailDO)

        {

            List<GetEmailTemplateDO> VerificationResponse = new List<GetEmailTemplateDO>();

            GetEmailTemplateDO VerificationResult = new GetEmailTemplateDO();

            try

            {

                LoginDAL loginparamDAL = new LoginDAL();

                VerificationResponse = loginparamDAL.GetLoginEmailTemplate(emailDO);

                if (VerificationResponse.Count > 0)

                {

                    VerificationResult = VerificationResponse[0];

                }

            }

            catch (Exception ex)

            {

                VerificationResult.Success = false;

                VerificationResult.Message = "Internal error occurred.";

                LoggerDAL.FnStoreErrorLog("LoginBAL", "EmailVerification",

                    "Internal error occurred.", ex.StackTrace, ex.Message, 0);

            }

            return VerificationResult;

        }
    }
}
