﻿using HackathonAPI.Entity.DMS;
using Microsoft.VisualBasic.FileIO;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HackathonAPI.Business
{
    public class DMSService : IDMSService
    {
        private readonly QueryFactory _dbContext;
        public DMSService(QueryFactory dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveFile(SaveFileReqVM model)
        {
            try
            {
                var categoryFolderId = await GetFolderId(model.CategoryName);
                var subCategoryFolderId = await GetFolderId(model.SubCategoryName, categoryFolderId);

                await _dbContext.Query("files").InsertGetIdAsync<long>(new
                {
                    FileName = model.FileName,
                    FileUrl = model.FileUrl,
                    FolderId = subCategoryFolderId,
                    Tags = string.Join(',', model.Tags)
                });
            }
            catch (Exception e)
            {

            }
        }

        public async Task<long> GetFolderId(string name, long? parentId = null)
        {
            long id = await _dbContext.Query("folders")
                .Where(new { FolderName = name, ParentId = parentId })
                .Select("Id").FirstOrDefaultAsync<long>();

            if (id == 0)
            {
                id = await _dbContext.Query("folders").InsertGetIdAsync<long>(new
                {
                    FolderName = name,
                    ParentId = parentId,
                });
            }
            return id;
        }

        public async Task<List<FilesResponseVM>> GetData(GetDataReqVM filter)
        {
            try
            {
                var folderData = (await _dbContext.Query("folders")
                    .Where(new { ParentId = filter.FolderId })
                    .WhereLike("FolderName", "%" + filter.Name + "%")
                    .Select("Id", "FolderName as Name")
                    .SelectRaw("GetParentPath(ParentId) as Path")
                    .GetAsync<FilesResponseVM>()).ToList();

                var fileData = (await _dbContext.Query("files")
                    .Where(new { FolderId = filter.FolderId })
                    .WhereLike("FileName", "%" + filter.Name + "%")
                    .Select("Id", "FileName as Name", "FileUrl")
                    .SelectRaw("GetParentPath(FolderId) as Path")
                    .GetAsync<FilesResponseVM>()).ToList();

                return folderData.Union(fileData).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<FileWithTagsVM>> GetFilesOnKeyword(string keyword)
        {
            var fileData = (await _dbContext.Query("files")
                .WhereLike("Tags", "%" + keyword + "%")
                .Select("Id", "FileName as Name", "FileUrl", "Tags")
                .SelectRaw("GetParentPath(FolderId) as Path")
                .GetAsync<FileWithTagsVM>()).ToList();

            return fileData;
        }

        public async Task<List<MenuVM>> GetLeftMenuData()
        {
            var folderData = (await _dbContext.Query("folders")
                    .Select("Id", "FolderName as Name", "ParentId")
                    .GetAsync<FolderResponseVM>()).ToList();

            var hierarchy = folderData
                     .Where(c => c.ParentId == null)
                     .Select(c => new MenuVM
                     {
                         Id = c.Id,
                         Name = c.Name,
                         ParentId = c.ParentId,
                         Items= GetChildren(folderData, c.Id)
                     }).ToList();

            return hierarchy;
        }

        public List<MenuVM> GetChildren(List<FolderResponseVM> data, long parentId)
        {
            return data
                    .Where(c => c.ParentId == parentId)
                    .Select(c => new MenuVM
                    {
                        Id = c.Id,
                        Name = c.Name,
                        ParentId = c.ParentId,
                        Items = GetChildren(data, c.Id)
                    })
                    .ToList();
        }
    }
}
