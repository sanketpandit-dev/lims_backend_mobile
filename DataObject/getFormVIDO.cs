using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObject
{
    public class GetForm6DetailsDO
    {
        public string SerialNo { get; set; }
    }

    // API Response wrapper
    public class Form6DetailsResponseDO
    {
        public bool Success { get; set; }        // true = success, false = error
        public string Message { get; set; }      // success/error message
        public int StatusCode { get; set; }      
        public List<Form6DetailsDO> Form6Details { get; set; } // actual data list
    }

    // Actual Form VI data
    public class Form6DetailsDO
    {
        public int Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public string SerialNo { get; set; }
        public string SampleCodeNumber { get; set; }
        public string CollectionDate { get; set; }
        public string PlaceOfCollection { get; set; }
        public string SampleName { get; set; }
        public string QuantityOfSample { get; set; }
        public string NatureOfSample { get; set; }
        public string PreservativeAdded { get; set; }
        public string PreservativeName { get; set; }
        public string QuantityOfPreservative { get; set; }
        public string WitnessSignature { get; set; }
        public string PaperSlipNumber { get; set; }
        public string SignatureOfDO { get; set; }
        public string WrapperCodeNumber { get; set; }
        public string SealImpression { get; set; }
        public string SealNumber { get; set; }
        public string MemoFormVI { get; set; }
        public string WrapperFormVI { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Status { get; set; }
        public string SenderName { get; set; }
        public string SenderDesignation { get; set; }
        public string DONumber { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
    }

}
