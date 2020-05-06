using Autofac;
using BAccurate.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ConsoleWebApi.Extends
{/// <summary>
 /// 资源认证
 /// </summary>
    public class AuthResourceTagAttribute : ActionFilterAttribute
    {
        private string resouceCode;
        public AuthResourceTagAttribute(string code = "")
        {
            this.resouceCode = code;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var actionAttrs = actionContext.ActionDescriptor.GetCustomAttributes<Attribute>();
            var controllerAttrs = actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<Attribute>();

            // 如果为允许匿名访问，则返回
            if (actionAttrs.Any(m => m is AllowAnonymousAttribute) || controllerAttrs.Any(m => m is AllowAnonymousAttribute))
            {
                return;
            }

            string code = this.resouceCode;
            if (string.IsNullOrEmpty(code))
            {
                code = $"{actionContext.ActionDescriptor.ControllerDescriptor.ControllerName}-{actionContext.ActionDescriptor.ActionName}";
            }

            IOnlineUserMgr authAggregate = Framework.IocContainer.IOCContainer.Resolve<IOnlineUserMgr>();
            string token = Utils.Common.GetToken(actionContext.Request);
            if (string.IsNullOrEmpty(token))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }

            var profile = authAggregate.Get(token);
            if (profile == null)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }

            if (!profile.Verify(code))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }
            base.OnActionExecuting(actionContext);
        }

    }
}