using ConsoleWebApi.Extends;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;

namespace ConsoleWebApi
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<StartUp>("http://*:9909"))
            {
                Console.WriteLine("start 9909...");
                Console.ReadKey();
            }
        }
    }
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (Directory.Exists(rootPath))
            {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileSystem = new PhysicalFileSystem(rootPath),
                    RequestPath = new PathString("/root")
                });
            }

            var httpConfiguratin = new HttpConfiguration();

            JsonSerializerSettings setting = new JsonSerializerSettings(); 
            //日期类型默认格式化处理
            setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss.ffff";
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // 将长整型数据转为字符串
            // 否则id=1202054749357081610 会变成id = 1.2020547493570816E+18
            setting.Converters.Add(new LongToString());
            //空值处理
            setting.NullValueHandling = NullValueHandling.Ignore; 
            httpConfiguratin.Formatters.JsonFormatter.SerializerSettings = setting;



            httpConfiguratin.Routes.MapHttpRoute("default", "api/{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional });
            httpConfiguratin.Filters.Add(new AuthFilterAttribute());
            Framework.IocContainer.Builder(httpConfiguratin);
            app.UseAutofacWebApi(httpConfiguratin);
            app.UseWebApi(httpConfiguratin);

            //signalr 
            app.Map("/wsc", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                map.RunSignalR(new HubConfiguration());
            });
        }
    }
}
