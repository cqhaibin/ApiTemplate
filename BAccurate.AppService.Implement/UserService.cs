using BAccurate.AppServices;
using BAccurate.Models.User;
using BAccurate.Repository.Freesql;
using BAccurate.Repository.Freesql.Entities;
using SAM.Framework;
using SAM.Framework.Result;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAccurate.AppService.Implement
{
    public class UserService : IUserAppService
    {
        private IFMapper mapper;
        private BAccurateContext dbContext;
        public UserService(BAccurateContext dbContext, IFMapper mapper)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public ResultDataInfo<UserDetailInfo> Get(int id)
        {
            UserDetailInfo info = new UserDetailInfo();
            var roleEntities = this.dbContext.RoleRepository.Select.ToList();
            var userEntity = this.dbContext.UserRepository.Select.Where(m => m.Id == id).ToOne();
            var userRoleRelsEntities = this.dbContext.UserRoleRepository.Select.Where(m => m.UserId == id).ToList();
            info.UserInfo = this.mapper.Map<UserInfo>(userEntity);

            foreach(var r in userRoleRelsEntities)
            {
                var role = roleEntities.FirstOrDefault(m => m.Id == r.RoleId);
                if (role != null)
                {
                    info.Roles.Add(this.mapper.Map<RoleInfoOfUser>(role));
                }
            }
            return new ResultDataInfo<UserDetailInfo>()
            {
                Data = info
            };
        }

        public ResultListInfo<UserInfo> GetAllUsers()
        {
            var entites = this.dbContext.UserRepository.Select.ToList();

            return new ResultListInfo<UserInfo>()
            {
                List = this.mapper.Map<IList<UserInfo>>(entites)
            };
        }

        public ResultInfo Remove(int id)
        {
            ResultInfo result = null;
            try
            {
                this.dbContext.Transaction(() =>
                {
                    this.dbContext.UserRepository.Remove(m => m.Id == id);
                    this.dbContext.UserRoleRepository.Remove(m => m.UserId == id);
                    this.dbContext.SaveChanges();
                });
                result = new ResultInfo();
            }
            catch(Exception ex)
            {
                result = new ResultExceptionInfo()
                {
                    Exception = ex
                };
            }
            return result;
        }

        public ResultInfo Save(UserParam param)
        {
            ResultInfo result = null;
            try
            {
                var entity = this.mapper.Map<UserEntity>(param);
                int userId;
                //user exists
                if (this.dbContext.UserRepository.Select.Where(m => m.UserName == entity.UserName).ToOne() != null)
                {
                    return new ResultInfo()
                    {
                        ResultCode = OcmStatusCode.USER_ALREADY_EXIST,
                        IsSuccess = false
                    };
                }
                this.dbContext.Transaction(() =>
                {
                    if (entity.Id == 0)
                    {
                        entity.Password = "123456";
                        //insert
                        this.dbContext.UserRepository.Add(entity);
                    }
                    else
                    {
                        var oldEntity = this.dbContext.UserRepository.Select.Where(m => m.Id == entity.Id).ToOne();
                        entity.UserName = oldEntity.UserName;
                        entity.Password = oldEntity.Password;
                        //update
                        this.dbContext.UserRepository.Update(entity);
                    }
                    this.dbContext.SaveChanges();
                    userId = entity.Id;
                    if (param.Roles != null)
                    {
                        this.dbContext.UserRoleRepository.Remove(m => m.UserId == userId); //根据userid删除与角色的关联
                        foreach (var role in param.Roles)
                        {
                            this.dbContext.UserRoleRepository.Add(new UserRoleRelationEntity()
                            {
                                RoleId = role,
                                UserId = userId
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
