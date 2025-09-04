using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObject.Errorlog
{
    public class ErrorLogDO
    {
        public string ControllerName { get; set; }
        public string FunctionName { get; set; }
        public string message { get; set; }
        public string StackTrace { get; set; }
        public string Error_Description { get; set; }
        public int UserId { get; set; }

    }
    public class RequestResponseLogDO
    {

        public string Action { get; set; }
        public string JsonData { get; set; }
        public string ApiUrl { get; set; }
        public int UserId { get; set; }
        public int NewId { get; set; }



    }
}
