using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObject
{
    public class sampleResponseDataDO
    {
        public List<SampleDetailsDO> SampleList { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public sampleResponseDataDO()
        {
            SampleList = new List<SampleDetailsDO>();
        }
    }
    public class SampleDetailsDO
    {
        public string serial_no { get; set; }
        public DateTime sample_sent_date { get; set; }
        public DateTime sample_resent_date { get; set; }
        public DateTime sample_re_requested_date { get; set; }
        public string lab_location { get; set; }
        public string status_name { get; set; }
        public int UserID { get; set; }
    }
}
