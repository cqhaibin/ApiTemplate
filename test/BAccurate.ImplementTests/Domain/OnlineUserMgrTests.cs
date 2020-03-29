using Microsoft.VisualStudio.TestTools.UnitTesting;
using BAccurate.Implement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAccurate.Domain;
using BAccurate.ImplementTests;

namespace BAccurate.Implement.Domain.Tests
{
    [TestClass()]
    public class OnlineUserMgrTests
    {
        private MockAuthService mockAuthService;
        public OnlineUserMgrTests()
        {
            this.mockAuthService = new MockAuthService();
        }

        [TestMethod()]
        public void AdminUserEntityTest()
        {
            int userId = 1;
            var onlineMgr = this.CreateOnlineMgr();
            var userEnity = new UserEntity(userId,
                this.mockAuthService.Mock_ReadAuthRepository(), new Models.RequestClientInfo()
                {

                }, onlineMgr);
            Assert.AreEqual(true, userEnity.Verify());
            Assert.AreEqual(true, userEnity.Verify("login"));
            Assert.AreEqual(true, userEnity.Verify("loginOut"));
        }

        [TestMethod()]
        public void GuestUserEntityTest()
        {
            int userId = 2;
            var onlineMgr = this.CreateOnlineMgr();
            var userEnity = new UserEntity(userId,
                this.mockAuthService.Mock_ReadAuthRepository(), new Models.RequestClientInfo()
                {

                }, onlineMgr);
            
            Assert.AreEqual(true, userEnity.Verify());
            Assert.AreEqual(true, userEnity.Verify("login"));
            Assert.AreEqual(false, userEnity.Verify("loginOut"));
        }

        private IOnlineUserMgr CreateOnlineMgr()
        {
            var onlineMgr = new OnlineUserMgr(this.mockAuthService.Mock_TokenRepository(),
                this.mockAuthService.Mock_ReadAuthRepository(), this.mockAuthService.Mock_RoleAndResDepend());
            return onlineMgr;
        }
    }
}