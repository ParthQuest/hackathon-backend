using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonAPI.Entity.DMS
{
    public class SaveFileReqVM
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public List<string> Tags { get; set; }
    }
}
