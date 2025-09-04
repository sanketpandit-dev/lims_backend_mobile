using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataObject
{
   
    // Rohit R 19-07-2025 Custom Api Response
        public class ApiResponse<T>
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            [JsonIgnore]
            public int StatusCode { get; set; }

        
            

            
            public static ApiResponse<T> SuccessResponse(T data, string message = "")
            {
                return new ApiResponse<T>
                {
                    Success = true,
                    Message = message,
     
                   
                };
            }
        public static ApiResponse<T> SuccessResponse(string? Token, T data, string message)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
              
               
            };
        }

        public static ApiResponse<T> FailureResponse(string message)
            {
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = message,
             
                };
            }
        }
    
}
