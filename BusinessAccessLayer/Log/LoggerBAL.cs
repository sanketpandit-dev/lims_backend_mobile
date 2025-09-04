using DataAccessLayer.Log;
using DataObject.Errorlog;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Log
{
    public static class LoggerBAL
    {

        public static int FnStoreResponseRequestLog(RequestResponseLogDO logData)

        {

            try

            {

                if (logData != null)

                {

                    return LoggerDAL.SaveRequestResponselog(logData);

                }

                else

                {

                    LoggerDAL.FnStoreErrorLog("LogBAL", "FnStoreResponseRequestLog", "Input logData is null", "", "Invalid input", 0);

                    return 0;

                }

            }

            catch (Exception ex)

            {

                LoggerDAL.FnStoreErrorLog("LogBAL", "FnStoreResponseRequestLog", "Exception occurred", ex.StackTrace, ex.Message, 0);

                return 0;

            }

        }
    }
}
