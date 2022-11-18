using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonAPI.Helper
{
    public static class ModelCast
    {
        public static async Task<T> Request<T>(Stream body)
        {
            try
            {
                string requestBody = await new StreamReader(body).ReadToEndAsync();
                return JsonConvert.DeserializeObject<T>(requestBody);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
