using ConsoleWebApi.Utils;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConsoleWebApi.Controllers
{
    public class ValueController : ApiController
    {
        private ITestHub test;
        public ValueController()
        {
            this.test = new TestHub(null);
        }

        [HttpGet]
        [AllowAnonymous]
        public DateTime Index()
        {
            var cxt = Common.GetClientInfo(Request);
            return DateTime.Now;
        }

        [HttpGet]
        [AllowAnonymous]
        public bool send()
        {
            this.test.Send(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return true;
        }
    }
}
