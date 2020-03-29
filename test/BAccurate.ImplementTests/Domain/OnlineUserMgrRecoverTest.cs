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

namespace BAccurate.ImplementTests.Domain
{
    [TestClass]
    public class OnlineUserMgrRecoverTest
    {
        private MockAuthService mockAuthService;
        public OnlineUserMgrRecoverTest()
        {
            this.mockAuthService = new MockAuthService();
        }

        [TestMethod()]
        public void LoginTest()
        {
            this.Deletedb();
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
            string conn = "Data Source=" + curDir + "; Pooling=true;Min Pool Size=1";
            IFreeSql freeSql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.Sqlite, conn)
                .UseAutoSyncStructure(true)
                .Build();
            BAccurateContext.Fluent(freeSql);
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
