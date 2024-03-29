﻿using HackathonAPI.Entity.DMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonAPI.Business
{
    public interface IDMSService
    {
        Task SaveFile(SaveFileReqVM model);
        Task<List<FilesResponseVM>> GetData(GetDataReqVM filter);
        Task<List<FileWithTagsVM>> GetFilesOnKeyword(string keyword);
        Task<List<MenuVM>> GetLeftMenuData();
    }
}
