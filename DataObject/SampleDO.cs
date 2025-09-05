using System;

namespace DataObject
{
    public class InsertSampleRequestDO
    {
        // sample_location_master inputs
        public string SenderName { get; set; }
        public string SenderDesignation { get; set; }
        public string DoNumber { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int RegionId { get; set; }
        public int DivisionId { get; set; }
        public string SampleSendLocation { get; set; }    
        public int LabMastId { get; set; }
        public string Area { get; set; }
     

        // sample_details inputs
        public string SampleCodeNumber { get; set; }
        public DateTime CollectionDate { get; set; }
        public string PlaceOfCollection { get; set; }
        public string SampleName { get; set; }
        public string QuantityOfSample { get; set; }
        public int SampleId { get; set; }
        public bool PreservativeAdded { get; set; }
        public string PreservativeName { get; set; }
        public string QuantityOfPreservative { get; set; }
        public bool WitnessSignature { get; set; }
        public string PaperSlipNumber { get; set; }
        public bool SignatureOfDo { get; set; }
        public string WrapperCodeNumber { get; set; }
        public bool SealImpression { get; set; }
        public string SealNumber { get; set; }
        public bool MemoFormVI { get; set; }
        public bool WrapperFormVI { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool SampleIsActive { get; set; }
        public int insertedBy { get; set; }
        public int sampleInsertedBy { get; set; }
        public int doSealNumber { get; set; }
        public string documentName { get; set; }
        public string documentBase64 { get; set; }  

    }


    public class InsertSampleResponseDO
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public string SerialNo { get; set; }


    }
}

