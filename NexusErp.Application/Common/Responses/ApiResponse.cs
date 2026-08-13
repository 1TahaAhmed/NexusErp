using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Common.Responses
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public IDictionary<string, string>? Errors { get; set; }

        public static ApiResponse<T> Success(T data, string message = "Operation completed successfully.")
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };

        }

        public static ApiResponse<T> Failure(string message, IDictionary<string, string>? errors = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors
            };
        }
    }
}
