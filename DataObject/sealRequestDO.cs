using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObject
{
    public class SealRequestDO
    {
        public int UserId { get; set; }

        public string RequestedDate { get; set; }
        public int SealNumbers { get; set; }
        
    }

    public class UpdateSealRequestDO
    {

        public int UserId { get; set; }
        public int p_request_id { get; set; }
        public string RequestedDate { get; set; }
        public int SealNumbers { get; set; }

    }
    public class sealNumberDO
    {
        public int UserId { get; set; }

        public string Seal_numbers { get; set; } // maps seal_number column


    }

    public class getSealRequestDO
    {
        public int UserId { get; set; }
    }

    public class getSealRequestResponseDO
    {
        public string Seal_Number { get; set; }

        public int Request_id { get; set; }
        public int Count { get; set; }
        public string status { get; set; }
        public string seal_request_date { get; set; }
        public string seal_send_date { get; set; }
    }

    public class SealRequestResponseWrapperDO
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<getSealRequestResponseDO> Data { get; set; }
    }

}
