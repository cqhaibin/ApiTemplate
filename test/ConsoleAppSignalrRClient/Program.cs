using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace ConsoleAppSignalrRClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var hubConnection = new HubConnection("http://127.0.0.1:9909/wsc");
            IHubProxy hubProxy = hubConnection.CreateHubProxy("TestHub");
            hubProxy.On("sendMessage", (mgs) =>
            {
                Console.WriteLine(mgs);
            });
            hubConnection.Start().Wait();
            Console.ReadKey();
        }
    }
}
