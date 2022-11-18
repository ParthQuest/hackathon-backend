using HackathonAPI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonAPI.Helper
{
    public class MessageHelper
    {
        public const string UsernamePasswordIncorrect = "The email address or password is not correct.";
        public const string AccountInactive = "Your account is inactive.";
        public const string UserExists = "User is already exists.";
        public const string SaveError = "Error while saving record.";
        public const string DeleteError = "Error while delete record.";
        //public const string Invalidlink = "Invalid link.";


        public static ApiError SomethingWentWrong
        {
            get
            {
                return new ApiError { ErrorCode = "999", Error = "Something went wrong.", ErrorDetail = "Something went wrong." };
            }
        }
        public static ApiError Invalidlink
        {
            get
            {
                return new ApiError { ErrorCode = "1003", Error = "Invalid link.", ErrorDetail = "Invalid link." };
            }
        }
        public static ApiError LinkExpire
        {
            get
            {
                return new ApiError { ErrorCode = "1004", Error = "Link is Expired.", ErrorDetail = "Link is Expired." };
            }
        }

        public static ApiError InvalidCode
        {
            get
            {
                return new ApiError { ErrorCode = "1002", Error = "Invalid otp.", ErrorDetail = "Invalid otp." };
            }
        }
        public static ApiError CodeExpire
        {
            get
            {
                return new ApiError { ErrorCode = "1001", Error = "Otp expire.", ErrorDetail = "Otp expire." };
            }
        }
        public static ApiError CheckListProfile
        {
            get
            {
                return new ApiError { ErrorCode = "1005", Error = "Profile incomplete.", ErrorDetail = "Profile incomplete." };
            }
        }
        public static ApiError UnCannotEditSubmitChecklist
        {
            get
            {
                return new ApiError { ErrorCode = "1006", Error = "You cannot edit submitted checklist.", ErrorDetail = "You cannot edit submitted checklist." };
            }
        }
        public static ApiError FileUploadFailed
        {
            get
            {
                return new ApiError { ErrorCode = "1007", Error = "File upload failed.", ErrorDetail = "File upload failed." };
            }
        }
        public static ApiError GetFileFailed
        {
            get
            {
                return new ApiError { ErrorCode = "1008", Error = "Error while getting file.", ErrorDetail = "Error while getting file." };
            }
        }
        public static ApiError RemoveFileFailed
        {
            get
            {
                return new ApiError { ErrorCode = "1010", Error = "File remove failed.", ErrorDetail = "File remove failed." };
            }
        }
        public static ApiError NoDataFound
        {
            get
            {
                return new ApiError { ErrorCode = "121", Error = "No Data Found.", ErrorDetail = "No Data Found." };
            }
        }
    }
}
