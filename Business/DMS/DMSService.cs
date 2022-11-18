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
    }
}
