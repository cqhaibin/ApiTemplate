using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BAccurate.Domain;

namespace BAccurate.ImplementTests
{
    public class MockAuthService
    {
        public IReadAuthRepository Mock_ReadAuthRepository()
        {
            var mock = new Mock<IReadAuthRepository>();
            mock.Setup(f => f.GetUserInfo(1)).Returns(new Models.Auth.UserInfo()
            {
                Id = 1,
                RealName = "admin user",
                Roles = new int[] { 1, 2, 5 }
            });
            mock.Setup(f => f.GetUserInfo(2)).Returns(new Models.Auth.UserInfo()
            {
                Id = 2,
                RealName = "guest user",
                Roles = new int[] { 2 }
            });
            return mock.Object;
        }

        public ITokenRepository Mock_TokenRepository()
        {
            var mock = new Mock<ITokenRepository>();
            mock.Setup(f => f.GetAllTokens()).Returns(new List<TokenEntity>());
            return mock.Object;
        }

        public IRoleAndResDepend Mock_RoleAndResDepend()
        {
            var res = new List<Models.Auth.ResourceInfo>()
            {
                new Models.Auth.ResourceInfo()
                {
                  Id = 1,
                  ResourceCode="login",
                  ResourceName="登入",
                  ResourceType=1,
                  Url="/api/Auth/PostLogin"
                },new Models.Auth.ResourceInfo()
                {
                  Id = 2,
                  ResourceCode="loginOut",
                  ResourceName="登出",
                  ResourceType=1,
                  Url="/api/Auth/LoginOut"
                }
            };
            var roleAndRes = new List<Models.Auth.RoleAndResInfo>()
            {
                new Models.Auth.RoleAndResInfo()
                {
                    ResourceCode="login",
                    ResurceId=1,
                    RoleId=1
                },new Models.Auth.RoleAndResInfo()
                {
                    ResourceCode="loginOut",
                    ResurceId=2,
                    RoleId=1
                },new Models.Auth.RoleAndResInfo()
                {
                    ResourceCode="login",
                    ResurceId=1,
                    RoleId=2
                }
            };
            var mock = new Mock<IRoleAndResDepend>();
            mock.Setup(f => f.GetAllResourceInfos()).Returns(res);
            mock.Setup(f => f.GetAllRoleAndRes()).Returns(roleAndRes);
            return mock.Object;
        }
    }
}
