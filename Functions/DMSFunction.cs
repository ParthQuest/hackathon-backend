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
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "documentfolder/savefile")] HttpRequest req,
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
    }
}
