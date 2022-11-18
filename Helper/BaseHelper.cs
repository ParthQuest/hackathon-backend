using HackathonAPI.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonAPI.Helper
{
    public class BaseHelper
    {
        #region Response Utility
        public IActionResult OkResponse(object data = null)
        {
            return new ObjectResult(new ApiResponse { ResponseStatus = "Success", ResponseData = data });
        }
        public IActionResult ExceptionErrorResponse(object data = null)
        {
            return new ObjectResult(new ApiResponse { ResponseStatus = "Failure", ErrorData = new ApiError { ErrorDetail = data } });
        }
        public IActionResult ErrorResponse(string error = "", object data = null)
        {
            return new ObjectResult(new ApiResponse { ResponseStatus = "Failure", Message = error, ErrorData = new ApiError { ErrorDetail = data, Error = error } });
        }
        public IActionResult ErrorWithResponse(ApiError error)
        {
            return new ObjectResult(new ApiResponse { ResponseStatus = "Failure", ErrorData = error });
        }
        public IActionResult InvalidRequest()
        {
            return new ObjectResult(new ApiResponse { ResponseStatus = "Failure", Message = "invalid request!", ErrorData = new ApiError { Error = "invalid request!", ErrorCode = "777" } });
        }
        #endregion
    }
}
