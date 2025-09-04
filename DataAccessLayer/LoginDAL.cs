using DataAccessLayer.Log;
using DataObject;
using MySql.Data.MySqlClient;
using System;

using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace DataAccessLayer
{

    public class LoginDAL
    {
        string remark = "Error While SP Execution .";

        public List<ChangePassResponseDO> ChangePassword(ChangePassDO passDO, int userid)
        {
            List<ChangePassResponseDO> listdata = new List<ChangePassResponseDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                mysqlParamList.Add(DataClass.GetParameter("@p_Username", passDO.Username));
                mysqlParamList.Add(DataClass.GetParameter("@p_Password", passDO.Password));
                mysqlParamList.Add(DataClass.GetParameter("@p_NewPassword", passDO.NewPassword));
                listdata = Getdataconvert.getdata<ChangePassResponseDO>(DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_user_changepassword_arya"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("LoginDAL", "ChangePassword", remark, ex.StackTrace, ex.Message, userid);

            }
            return listdata;
        }


        public List<EmailVerificationResponseDO> EmailVerification(EmailVerificationDO emailDO)
        {
            List<EmailVerificationResponseDO> listdata = new List<EmailVerificationResponseDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                mysqlParamList.Add(DataClass.GetParameter("@p_User_Email", emailDO.UserMailId));
                //mysqlParamList.Add(DataClass.GetParameter("@p_Username", emailDO.Username));
                listdata = Getdataconvert.getdata<EmailVerificationResponseDO>(DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_verify_user_emailid"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("LoginDAL", "EmailVerification", remark, ex.StackTrace, ex.Message, 0);

            }
            return listdata;
        }

        public List<EmailVerificationResponseDO> EmailOTPVerification(EmailVerificationDO emailDO)
        {
            List<EmailVerificationResponseDO> listdata = new List<EmailVerificationResponseDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                mysqlParamList.Add(DataClass.GetParameter("@p_User_Email", emailDO.UserMailId));
                //mysqlParamList.Add(DataClass.GetParameter("@p_Username", emailDO.Username));
                listdata = Getdataconvert.getdata<EmailVerificationResponseDO>(DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_verify_user_emailid"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("LoginDAL", "EmailVerification", remark, ex.StackTrace, ex.Message, 0);

            }
            return listdata;
        }


        public List<EmailVerificationResponseDO> SaveOtp(VerificationDO EmailDO)
        {
            List<EmailVerificationResponseDO> listdata = new List<EmailVerificationResponseDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                mysqlParamList.Add(DataClass.GetParameter("@p_user_id", EmailDO.UserId));
                mysqlParamList.Add(DataClass.GetParameter("@p_email", EmailDO.Email));
                mysqlParamList.Add(DataClass.GetParameter("@p_verification_code", EmailDO.verificationCode));
                listdata = Getdataconvert.getdata<EmailVerificationResponseDO>(DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_insert_code"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("LoginDAL", "SaveOtp", remark, ex.StackTrace, ex.Message, 0);
            }
            return listdata;
        }


        public List<UserVerificationResponseDO> SaveFirebaseOtp(VerificationDO otpDO)
        {
            List<UserVerificationResponseDO> listdata = new List<UserVerificationResponseDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();

                mysqlParamList.Add(DataClass.GetParameter("@p_username", otpDO.Username));
                mysqlParamList.Add(DataClass.GetParameter("@p_verification_code", otpDO.verificationCode));
                mysqlParamList.Add(DataClass.GetParameter("@p_inserted_by", otpDO.UserId)); // audit column

                listdata = Getdataconvert.getdata<UserVerificationResponseDO>(
                    DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_insert_firebase_otp")
                );
            }
            catch (Exception ex)
            {
                string remark = "Error while saving Firebase OTP";
                LoggerDAL.FnStoreErrorLog("LoginDAL", "SaveFirebaseOtp", remark, ex.StackTrace, ex.Message, 0);
            }
            return listdata;
        }


        public List<ForgotPasswordResponseDO> ForgotPassword(ForgetPasswordDO forgotDO)
        {
            List<ForgotPasswordResponseDO> listdata = new List<ForgotPasswordResponseDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                mysqlParamList.Add(DataClass.GetParameter("@p_UserId", forgotDO.UserId));
                mysqlParamList.Add(DataClass.GetParameter("@p_NewPassword", forgotDO.Password));
                listdata = Getdataconvert.getdata<ForgotPasswordResponseDO>(DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_forgotpassword"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("LoginDAL", "ForgotPassword", remark, ex.StackTrace, ex.Message, 0);

            }
            return listdata;
        }

        public List<VerifyOtpResponseDO> VerifyOtp(VerificationDO OTP)
        {
            List<VerifyOtpResponseDO> listdata = new List<VerifyOtpResponseDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                mysqlParamList.Add(DataClass.GetParameter("@p_UserId", OTP.UserId));
                mysqlParamList.Add(DataClass.GetParameter("@p_email", OTP.Email));
                mysqlParamList.Add(DataClass.GetParameter("@p_verification_code", OTP.verificationCode));
                listdata = Getdataconvert.getdata<VerifyOtpResponseDO>(DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_verify_otp"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("LoginDAL", "VerifyOtp", remark, ex.StackTrace, ex.Message, 0);


            }
            return listdata;
        }
        public List<VerifyOtpResponseDO> VerifyLoginOtp(VerificationDO OTP)
        {
            List<VerifyOtpResponseDO> listdata = new List<VerifyOtpResponseDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                mysqlParamList.Add(DataClass.GetParameter("@p_UserId", OTP.UserId));
                mysqlParamList.Add(DataClass.GetParameter("@p_Username", OTP.Username));
                mysqlParamList.Add(DataClass.GetParameter("@p_verification_code", OTP.verificationCode));
                listdata = Getdataconvert.getdata<VerifyOtpResponseDO>(DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_verify_login_otp"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("LoginDAL", "VerifyOtp", remark, ex.StackTrace, ex.Message, 0);


            }
            return listdata;
        }




        public List<UserLoginData> LoginDetailsDAL(LoginDO loginParam)
        {
            List<UserLoginData> userDetails = new();
            try
            {

                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                mysqlParamList.Add(DataClass.GetParameter("@p_Username", loginParam.Username));
                mysqlParamList.Add(DataClass.GetParameter("@p_Password", loginParam.Password));

                var getConverted = new getConvertedData();
                userDetails = getConverted.getdata<UserLoginData>(
                    DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_user_authentication_bkp_arya"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("LoginDAL", "LoginDetailsDAL", remark, ex.StackTrace, ex.Message, 0);


            }
            return userDetails;
        }


        public List<GetEmailTemplateDO> GetEmailTemplate(GetEmailTemplatereuestDO emailDO)
        {
            List<GetEmailTemplateDO> listdata = new List<GetEmailTemplateDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                mysqlParamList.Add(DataClass.GetParameter("@p_OTP", emailDO.VerificationCode));
                mysqlParamList.Add(DataClass.GetParameter("@p_UserName", emailDO.Username));
                listdata = Getdataconvert.getdata<GetEmailTemplateDO>(DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_get_email_template"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("LoginDAL", "GetEmailTemplate", remark, ex.StackTrace, ex.Message, 0);

            }
            return listdata;
        }


        public List<GetEmailTemplateDO> GetLoginEmailTemplate(GetEmailTemplatereuestDO emailDO)
        {
            List<GetEmailTemplateDO> listdata = new List<GetEmailTemplateDO>();
            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();
                mysqlParamList.Add(DataClass.GetParameter("@p_OTP", emailDO.VerificationCode));
                mysqlParamList.Add(DataClass.GetParameter("@p_UserName", emailDO.Username));
                listdata = Getdataconvert.getdata<GetEmailTemplateDO>(DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "sp_login_email_tpl"));
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("LoginDAL", "GetEmailTemplate", remark, ex.StackTrace, ex.Message, 0);

            }
            return listdata;
        }

    }
}