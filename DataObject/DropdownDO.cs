using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObject
{
    public class DropdownDO
    {
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int DivisionId { get; set; }

    }

    public class getSealNumberDO
    {
        public int RequestId { get; set; }

    }
    public class DropdownWrapperResponseDO
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<DropdownResponseDO> Data { get; set; }

    }
    public class DropdownResponseDO
    {
        public int Id { get; set; }
        public string Name { get; set; }


    }
    public class sealNumberResponseDO
    {
        public string Seal_numbers { get; set; }


    }
    public class SampleDropDownResponseDO
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<SampleDropDownDO> Data { get; set; }
    }

    public class SampleDropDownDO
    {
         public int Id { get; set; }
        public string Name { get; set; }
    }

  
    

 

   

}
