using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataObject
{
    public class LoginDO
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
    public class EncryptedRequest
    {
        public string EncryptedData { get; set; }
        public string IV { get; set; }
        public string EncryptedAESKey { get; set; }
        
    }

    public class EncryptedResponse
    {
        public string EncryptedData { get; set; }
        public string IV { get; set; }
        public string EncryptedAESKey { get; set; }
    }


    public class LoginData
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string user_fullname { get; set; }
        public string user_email { get; set; }
        public int user_role_id { get; set; }
        public string PassResetFlag { get; set; }
    }

    public class ChangePassDO
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string NewPassword { get; set; }

        public string token { get; set; }
    }


    public class ChangePassResponseDO
    {

        public int UserId { get; set; }
        public string Username { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }


    public class EmailVerificationDO
    {
        public string UserMailId { get; set; }
    }

    public class VerificationDO
    {
        public int UserId { get; set; }
        public string Email { get; set; }

        public string Username { get; set; }

        [JsonIgnore]
        public string verificationCode { get; set; }
        public string ResponseMsg { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

    }
    public class OtpVerificationRequestDO
    {
        public int UserId { get; set; }              // User ID
        public string Username { get; set; }         // Optional username
        public string VerificationCode { get; set; } // OTP code to verify
    }
    public class UserVerificationResponseDO
    {
        public int OtpId { get; set; }
        public string Username { get; set; }
        public string ResponseMsg { get; set; }
        public bool Success { get; set; }
    }


    public class EmailVerificationResponseDO
    {
        public int UserId { get; set; }

        public string Username { get; set; }
        [JsonIgnore]
        public string VerificationCode { get; set; }
        public string Email { get; set; }

        public LoginData UserData { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

    }

    public class GetEmailTemplateDO

    {

        public bool Success { get; set; }

        public string Message { get; set; }

        public int StatusCode { get; set; }

        public string EmailBody { get; set; }

    }

    public class GetEmailTemplatereuestDO

    {

        public string Username { get; set; }

        public string VerificationCode { get; set; }

    }
    public class ForgetPasswordDO
    {
        public string Password { get; set; }
        public int UserId { get; set; }
    }


    public class VerifyOtpResponseDO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }

    public class ForgotPasswordResponseDO
    {
        public LoginData UserData { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

    }

    // 17-08-2025 Rohit R
    public class DecryptedJsonDataWithKey
    {
        public string DecryptedJsonData { get; set; }
        public byte[] ReqAesKey { get; set; }
        public byte[] Iv { get; set; }
    }

    // Rohit R 18-07-2025
    public class UserLoginData
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
        public string PassResetFlag { get; set; }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }

    public class loginResponseDO
    {

        public UserLoginData UserData { get; set; }

        public string token { get; set; }
    }

    public class TokenAppUserInfo
    {

        public int UserId { get; set; }
        public string Username { get; set; }

    }


    public class LogoutDO
    {

        public int User_Id { get; set; }

        public string token { get; set; }
    }

}
