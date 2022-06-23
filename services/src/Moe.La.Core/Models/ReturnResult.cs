using Moe.La.Core.Enums;
using System.Collections.Generic;

namespace Moe.La.Core.Models
{
    /// <summary>
    /// Acts as a response result for the service layer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReturnResult<T>
    {
        public ReturnResult()
        {
        }

        public ReturnResult(bool isSuccess, HttpStatuses statusCode, T data) : this()
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Data = data;
        }

        public ReturnResult(bool isSuccess, HttpStatuses statusCode, List<string> errorList) : this()
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            ErrorList = errorList;
        }

        /// <summary>
        /// Determines wether or not the operation is success.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Represent a HTTP status code.
        /// </summary>
        public HttpStatuses StatusCode { get; set; }

        /// <summary>
        /// Holds the returning data result of type <see cref="T"/>.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// The error list.
        /// </summary>
        public List<string> ErrorList { get; set; }
    }
}
