using HackathonAPI.Entity.DMS;
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

        public async Task<List<FilesResponseVM>> GetData(long? folderId)
        {
            var folderData = (await _dbContext.Query("folders")
                .Where(new { ParentId = folderId })
                .Select("Id", "FolderName as Name")
                .SelectRaw("GetParentPath(ParentId) as Path")
                .GetAsync<FilesResponseVM>()).ToList();

            var fileData = (await _dbContext.Query("files")
                .Where(new { FolderId = folderId })
                .Select("Id", "FileName as Name", "FileUrl")
                .SelectRaw("GetParentPath(FolderId) as Path")
                .GetAsync<FilesResponseVM>()).ToList();

            return folderData.Union(fileData).ToList();
        }
    }
}
