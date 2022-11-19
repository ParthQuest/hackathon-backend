using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HackathonAPI.Helper;
using HackathonAPI.Business;
using HackathonAPI.Entity.DMS;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace HackathonAPI.Functions
{
    public class DMSFunction : BaseHelper
    {
        private readonly IDMSService _dmsService;
        public DMSFunction(IDMSService dmsService)
        {
            _dmsService = dmsService;
        }

        [FunctionName(nameof(SaveFile))]
        public async Task<IActionResult> SaveFile(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "dms/savefile")] HttpRequest req,
        ILogger log)
        {
            try
            {
                var request = await ModelCast.Request<SaveFileReqVM>(req.Body);
                await _dmsService.SaveFile(request);
                return OkResponse();
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                return ErrorWithResponse(MessageHelper.GetFileFailed);
            }
        }

        [FunctionName(nameof(GetData))]
        public async Task<IActionResult> GetData(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "dms/get")] HttpRequest req,
        ILogger log)
        {
            try
            {
                var request = await ModelCast.Request<GetDataReqVM>(req.Body);
                var data = await _dmsService.GetData(request);
                if (data.Count > 0)
                {
                    return OkResponse(data);
                }
                else
                {
                    return ErrorWithResponse(MessageHelper.NoDataFound);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                return ErrorWithResponse(MessageHelper.GetFileFailed);
            }
        }

        [FunctionName(nameof(GetFilesOnKeyword))]
        public async Task<IActionResult> GetFilesOnKeyword(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "dms/search")] HttpRequest req,
        ILogger log)
        {
            try
            {
                var request = await ModelCast.Request<GetDataReqVM>(req.Body);
                var data = await _dmsService.GetFilesOnKeyword(request.Name);
                if (data.Count > 0)
                {
                    return OkResponse(data);
                }
                else
                {
                    return ErrorWithResponse(MessageHelper.NoDataFound);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                return ErrorWithResponse(MessageHelper.GetFileFailed);
            }
        }

        [FunctionName(nameof(GetLeftMenuData))]
        public async Task<IActionResult> GetLeftMenuData(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "dms/getmenu")] HttpRequest req,
        ILogger log)
        {
            try
            {
                var data = await _dmsService.GetLeftMenuData();
                if (data.Count > 0)
                {
                    return OkResponse(data);
                }
                else
                {
                    return ErrorWithResponse(MessageHelper.NoDataFound);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                return ErrorWithResponse(MessageHelper.GetFileFailed);
            }
        }
    }
}
