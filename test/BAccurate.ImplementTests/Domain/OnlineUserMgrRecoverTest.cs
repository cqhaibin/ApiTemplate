using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAccurate.Domain;
using BAccurate.Implement.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BAccurate.Repository.Freesql.Auth;
using BAccurate.WebApi.Framework;
using AutoMapper;
using SAM.Framework;
using BAccurate.Repository.Freesql;
using System.IO;
using BAccurate.AppService.Implement;
using BAccurate.AppServices;
using BAccurate.Models.User;

namespace BAccurate.ImplementTests.Domain
{
    [TestClass]
    public class OnlineUserMgrRecoverTest
    {
        private MockAuthService mockAuthService;
        public OnlineUserMgrRecoverTest()
        {
            this.mockAuthService = new MockAuthService();
            this.Deletedb();
        }

        [TestMethod()]
        public void LoginTest()
        {
            var onlineMgr = this.CreateOnlineMgr();
            var userEnity = new UserEntity(1,
                this.mockAuthService.Mock_ReadAuthRepository(), new Models.RequestClientInfo()
                {

                }, onlineMgr);
            onlineMgr.Add(userEnity);
            Assert.AreEqual(1, onlineMgr.GetAll().Count);

            userEnity = new UserEntity(2,
                this.mockAuthService.Mock_ReadAuthRepository(), new Models.RequestClientInfo()
                {

                }, onlineMgr);
            onlineMgr.Add(userEnity);
            Assert.AreEqual(2, onlineMgr.GetAll().Count);

            onlineMgr.Remove(1);
            Assert.AreEqual(1, onlineMgr.GetAll().Count);

            onlineMgr.Load();
            Assert.AreEqual(1, onlineMgr.GetAll().Count);

        }

        [TestMethod]
        public void UserRoleTest()
        {
            IUserAppService userAppService = new UserService(this.BuildFreesql(), this.BuildMapper());
            UserParam param = new UserParam();
            param.UserName = "admin";
            param.RealName = "管理员";
            param.Roles = new List<int>() { 1, 2, 3, 5 };
            var result = userAppService.Save(param);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void RoleAndResTest()
        {
            var freesql = this.BuildFreesql();
            var mapper = this.BuildMapper();
            IResourceAppService resourceAppService = new ResourceService(freesql, mapper);
            IRoleAppService roleAppService = new RoleService(freesql, mapper);
            IRoleAndResDepend roleAndResDepend = new RoleAndResDependRepository(freesql, mapper);

            #region 添加资源和资源与角色关系
            resourceAppService.Save(new Models.Resource.ResourceInfo()
            {
                Id = "1",
                ResourceCode = "login",
                ResourceName = "登入",
                ResourceType = Enums.ResourceType.Menu,
                Url = "/api/Auth/PostLogin"
            });
            resourceAppService.Save(new Models.Resource.ResourceInfo()
            {
                Id = "2",
                ResourceCode = "loginOut",
                ResourceName = "登出",
                ResourceType = Enums.ResourceType.Menu,
                Url = "/api/Auth/LoginOut"
            });
            roleAppService.Save(new Models.Role.RoleParam()
            {
                RoleName="admin",
                RoleCode="admin",
                Resources = new List<Models.Role.ResourceOfRole>()
                {
                    new Models.Role.ResourceOfRole()
                    {
                        Status = 1,
                        Id = "2"
                    },new Models.Role.ResourceOfRole()
                    {
                        Status = 1,
                        Id = "1"
                    }
                } 
            });
            roleAppService.Save(new Models.Role.RoleParam()
            {
                RoleName = "guest",
                RoleCode = "guest",
                Resources = new List<Models.Role.ResourceOfRole>()
                {
                    new Models.Role.ResourceOfRole()
                    {
                        Status = 1,
                        Id = "2"
                    }
                }
            });
            #endregion

            var res = roleAndResDepend.GetAllResourceInfos();
            var roleAndRes = roleAndResDepend.GetAllRoleAndRes();
            Assert.IsNotNull(res);
            Assert.IsNotNull(roleAndRes);
        }

        private IOnlineUserMgr CreateOnlineMgr()
        {
            var cxt = this.BuildFreesql();
            var onlineMgr = new OnlineUserMgr(cxt.TokenRepository,
                this.mockAuthService.Mock_ReadAuthRepository(), this.mockAuthService.Mock_RoleAndResDepend());
            return onlineMgr;
        }
        private IFMapper BuildMapper()
        {
            var mapperCfg = DtoMapper.CreateConfig();
            IMapper mapper = new Mapper(mapperCfg);
            return new DtoMapper(mapper);
        }

        private BAccurateContext BuildFreesql()
        {
            string curDir = AppDomain.CurrentDomain.BaseDirectory + "/test.db";
            string conn = "Data Source=" + curDir + "; Pooling=true;Min Pool Size=3;Journal Mode=WAL";
            IFreeSql freeSql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.Sqlite, conn)
                .UseAutoSyncStructure(false)
                .Build(); 
            BAccurateContext.Fluent(freeSql);
            freeSql.CodeFirst.SyncStructure<BAccurate.Repository.Freesql.Entities.UserRoleRelationEntity>();
            freeSql.CodeFirst.SyncStructure<BAccurate.Repository.Freesql.Entities.UserEntity>();
            freeSql.CodeFirst.SyncStructure<BAccurate.Repository.Freesql.Entities.RoleEntity>();
            freeSql.CodeFirst.SyncStructure<BAccurate.Repository.Freesql.Entities.RoleResourceRelationEntity>();
            freeSql.CodeFirst.SyncStructure<BAccurate.Repository.Freesql.Entities.RoleEntity>();
            freeSql.CodeFirst.SyncStructure<BAccurate.Repository.Freesql.Entities.ResourceEntity>();
            return new BAccurateContext(freeSql, this.BuildMapper());
        }

        private void Deletedb()
        {
            string curDir = AppDomain.CurrentDomain.BaseDirectory + "/test.db";
            if (File.Exists(curDir))
            {
                File.Delete(curDir);
            }
        }
    }
}
