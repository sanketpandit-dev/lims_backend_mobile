using DataAccessLayer.Log;
using DataObject;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public class SampleDAL
    {
        private const string Remark = "Error While SP Execution.";

        public List<InsertSampleResponseDO> InsertSample(InsertSampleRequestDO request, int insertedBy, int sampleInsertedBy)
        {
            List<InsertSampleResponseDO> result = new List<InsertSampleResponseDO>();

            try
            {
                getConvertedData Getdataconvert = new getConvertedData();
                List<MySqlParameter> mysqlParamList = new List<MySqlParameter>();

                
                mysqlParamList.Add(DataClass.GetParameter("@p_sender_name", request.SenderName));
                mysqlParamList.Add(DataClass.GetParameter("@p_sender_designation", request.SenderDesignation));
                mysqlParamList.Add(DataClass.GetParameter("@p_do_number", request.DoNumber));
                mysqlParamList.Add(DataClass.GetParameter("@p_country_id", request.CountryId));

                mysqlParamList.Add(DataClass.GetParameter("@p_district_id", request.DistrictId));
                mysqlParamList.Add(DataClass.GetParameter("@p_region_id", request.RegionId));
                mysqlParamList.Add(DataClass.GetParameter("@p_division_id", request.DivisionId));
                mysqlParamList.Add(DataClass.GetParameter("@p_lab_mast_id", request.LabMastId));                  
                mysqlParamList.Add(DataClass.GetParameter("@p_sample_send_location", request.SampleSendLocation));
                mysqlParamList.Add(DataClass.GetParameter("@p_area", request.Area));

                mysqlParamList.Add(DataClass.GetParameter("@p_inserted_by", insertedBy));

                mysqlParamList.Add(DataClass.GetParameter("@p_document_name", request.documentName));
                mysqlParamList.Add(DataClass.GetParameter("@p_documents", request.documentBase64));
                mysqlParamList.Add(DataClass.GetParameter("@p_do_seal_numbers", request.doSealNumber));

                mysqlParamList.Add(DataClass.GetParameter("@p_sample_code_number", request.SampleCodeNumber));
                mysqlParamList.Add(DataClass.GetParameter("@p_collection_date", request.CollectionDate));
                mysqlParamList.Add(DataClass.GetParameter("@p_place_of_collection", request.PlaceOfCollection));
                mysqlParamList.Add(DataClass.GetParameter("@p_sample_name", request.SampleName));
                mysqlParamList.Add(DataClass.GetParameter("@p_quantity_of_sample", request.QuantityOfSample));
                mysqlParamList.Add(DataClass.GetParameter("@p_sample_id", request.SampleId));

                mysqlParamList.Add(DataClass.GetParameter("@p_preservative_added", request.PreservativeAdded ? 1 : 0));
                mysqlParamList.Add(DataClass.GetParameter("@p_preservative_name", request.PreservativeName));
                mysqlParamList.Add(DataClass.GetParameter("@p_quantity_of_preservative", request.QuantityOfPreservative));

                mysqlParamList.Add(DataClass.GetParameter("@p_witness_signature", request.WitnessSignature ? 1 : 0));
                mysqlParamList.Add(DataClass.GetParameter("@p_paper_slip_number", request.PaperSlipNumber));
                mysqlParamList.Add(DataClass.GetParameter("@p_signature_of_do", request.SignatureOfDo ? 1 : 0));
                mysqlParamList.Add(DataClass.GetParameter("@p_wrapper_code_number", request.WrapperCodeNumber));

                mysqlParamList.Add(DataClass.GetParameter("@p_seal_impression", request.SealImpression ? 1 : 0));
                mysqlParamList.Add(DataClass.GetParameter("@p_seal_number", request.SealNumber));

                mysqlParamList.Add(DataClass.GetParameter("@p_memo_form_vi", request.MemoFormVI ? 1 : 0));
                mysqlParamList.Add(DataClass.GetParameter("@p_wrapper_form_vi", request.WrapperFormVI ? 1 : 0));

                mysqlParamList.Add(DataClass.GetParameter("@p_latitude", request.Latitude));
                mysqlParamList.Add(DataClass.GetParameter("@p_longitude", request.Longitude));

                mysqlParamList.Add(DataClass.GetParameter("@p_state_id", request.StateId));
                mysqlParamList.Add(DataClass.GetParameter("@p_sample_inserted_by", sampleInsertedBy));

                result = Getdataconvert.getdata<InsertSampleResponseDO>(
                    DataClass.getreaderFromSPWithParm(mysqlParamList, "limsmgt", "insert_sample_details")
                );
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("SampleDAL", "InsertSample", Remark, ex.StackTrace, ex.Message, insertedBy);
            }

            return result;
        }
    }
}
