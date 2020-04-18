using BAccurate.AppServices;
using BAccurate.Models.Role;
using BAccurate.Repository.Freesql;
using BAccurate.Repository.Freesql.Entities;
using SAM.Framework;
using SAM.Framework.Result;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAccurate.AppService.Implement
{
    public class RoleService : IRoleAppService
    {
        private IFMapper mapper;
        private BAccurateContext dbContext;
        public RoleService(BAccurateContext dbContext, IFMapper mapper)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public ResultDataInfo<RoleDetailInfo> Get(int roleId)
        {
            ResultDataInfo<RoleDetailInfo> result = new ResultDataInfo<RoleDetailInfo>();
            result.Data = new RoleDetailInfo();
            result.Data.Info = this.mapper.Map<RoleInfo>(this.dbContext.RoleRepository.Select.Where(m => m.Id == roleId).ToOne());

            var relEntities = this.dbContext.RoleResRepository.Select.Where(m => m.RoleId == roleId).ToList();
            var resourceEntites = this.dbContext.ResourceRepository.Select.ToList();
            List<ResourceInfoOfRole> resRoles = new List<ResourceInfoOfRole>();
            foreach(var rel in relEntities)
            {
                var entity = resourceEntites.FirstOrDefault(m => m.Id == rel.ResourceId);
                var resInfo = this.mapper.Map<ResourceInfoOfRole>(entity);
                resInfo.Status = rel.Status;
                resRoles.Add(resInfo);
            }
            result.Data.Resources = resRoles;

            return result;
        }

        public ResultListInfo<RoleInfo> GetAllRoles()
        {
            var entites = this.dbContext.RoleRepository.Select.ToList();
            var ls = this.mapper.Map<List<RoleInfo>>(entites);

            return new ResultListInfo<RoleInfo>()
            {
                List = ls
            };
        }

        public ResultInfo Remove(int id)
        {
            ResultInfo result = null;
            try
            {
                dbContext.Transaction(() =>
                {
                    this.dbContext.RoleRepository.Remove(m => m.Id == id);
                    this.dbContext.RoleResRepository.Remove(m => m.RoleId == id);
                    this.dbContext.SaveChanges();
                });
                result = new ResultInfo();

            }
            catch (Exception ex)
            {
                result = new ResultExceptionInfo()
                {
                    Exception = ex
                };
            }
            return result;
        }

        public ResultInfo Save(RoleParam param)
        {
            ResultInfo result = null;
            try
            {
                int roleId;
                var roleEntity = this.mapper.Map<RoleEntity>(param);
                this.dbContext.Transaction(() =>
                {
                    if (roleEntity.Id == 0)
                    {
                        //insert
                        this.dbContext.RoleRepository.Add(roleEntity);
                    }
                    else
                    {
                        //update
                        this.dbContext.RoleRepository.Update(roleEntity);
                    }
                    this.dbContext.SaveChanges();
                    roleId = roleEntity.Id;
                    if (param.Resources != null)
                    {
                        this.dbContext.RoleResRepository.Remove(m => m.RoleId == roleId);
                        foreach (var r in param.Resources)
                        {
                            this.dbContext.RoleResRepository.Add(new RoleResourceRelationEntity()
                            {
                                ResourceId = r.Id,
                                Status = r.Status,
                                RoleId = roleId
                            });
                        }
                    }
                    this.dbContext.SaveChanges();
                });
                result = new ResultInfo();
            }
            catch (Exception ex)
            {
                result = new ResultExceptionInfo()
                {
                    Exception = ex
                };
            }

            return result;
        }
    }
}
