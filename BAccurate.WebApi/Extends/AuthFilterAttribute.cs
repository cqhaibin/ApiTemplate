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

namespace BAccurate.WebApi.Extends
{
    /// <summary>
    /// 做基础的权限验证，没有对资料进行验证
    /// </summary>
    public class AuthFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var actionAttrs = actionContext.ActionDescriptor.GetCustomAttributes<Attribute>();
            var controllerAttrs = actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<Attribute>();

            // 如果为允许匿名访问，则返回
            if (actionAttrs.Any(m => m is AllowAnonymousAttribute || m is AuthResourceTagAttribute) || controllerAttrs.Any(m => m is AllowAnonymousAttribute))
            {
                return;
            }

            IOnlineUserMgr authAggregate = Framework.IocContainer.IOCContainer.Resolve<IOnlineUserMgr>();
            string token = Utils.Common.GetToken();
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

            if (!profile.Verify())
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }

            base.OnActionExecuting(actionContext);
        }
    }
}