using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAM.Framework;

namespace ConsoleWebApi
{
    public interface ITestHub : ITransient
    {
        void Send(string time);
    }

    public class TestHub : Hub, ITestHub
    {
        private readonly IHubContext hubContext;
        public TestHub(IHubContext hubContext)
        {
            if (this.hubContext == null)
            {
                this.hubContext = GlobalHost.ConnectionManager.GetHubContext<TestHub>();
            }
            else
            {
                this.hubContext = hubContext;
            }
        }
        public void Send(string time)
        {
            this.hubContext.Clients.All.sendMessage("这一个wsk全局消息" + time);
        }
    }
}
