using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAccurate.AppServices;
using BAccurate.Models.Test;
using SAM.Framework.Result;

namespace BAccurate.WebApi.Controllers
{
    public class TestController : BaseController
    {
        private ITestService testService;
        public TestController(ITestService testService)
        {
            this.testService = testService;
        }

        [HttpGet]
        public ResultInfo Save()
        {
            return this.testService.Save(new TestInfo()
            {
                Name = "Test"
            });
        }

        [HttpGet]
        public ResultListInfo<TestInfo> GetAll()
        {
            return this.testService.GetAll(null);
        }
    }
}
