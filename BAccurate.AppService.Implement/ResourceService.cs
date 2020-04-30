using BAccurate.AppServices;
using SAM.Framework;
using SAM.Framework.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using BAccurate.Repository.Freesql;
using BAccurate.Models.Resource;
using BAccurate.Repository.Freesql.Entities;

namespace BAccurate.AppService.Implement
{
    public class ResourceService : IResourceAppService
    {
        private IFMapper mapper;
        private BAccurateContext dbContext;
        public ResourceService(BAccurateContext dbContext, IFMapper mapper)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public ResultListInfo<ResourceInfo> GetAllList()
        {
            var entities = this.dbContext.ResourceRepository.Select.OrderBy(m => m.OrderNum).ToList();
            return new ResultListInfo<ResourceInfo>()
            {
                List = this.mapper.Map<List<ResourceInfo>>(entities)
            };
        }

        public ResultInfo Remove(string id)
        {
            this.dbContext.ResourceRepository.Remove(m => m.Id == id);
            this.dbContext.SaveChanges();
            return new ResultInfo();
        }

        public ResultDataInfo<string> Save(ResourceInfo info)
        {
            var entity = this.mapper.Map<ResourceEntity>(info);
            if (string.IsNullOrEmpty(info.Id) || this.dbContext.ResourceRepository.Select.Where(m => m.Id == info.Id).ToOne() == null)
            {
                if (string.IsNullOrEmpty(info.Id))
                {
                    entity.Id = Guid.NewGuid().ToString("N");
                }
                this.dbContext.ResourceRepository.Add(entity);
            }
            else
            {
                this.dbContext.ResourceRepository.Update(entity);
            }
            this.dbContext.SaveChanges();
            return new ResultDataInfo<string>()
            {
                Data = entity.Id
            };
        }
    }
}
