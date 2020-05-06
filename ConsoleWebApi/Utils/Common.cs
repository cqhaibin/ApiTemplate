using BAccurate.Models;
using ConsoleWebApi.Extends;
using Microsoft.Owin;
using System.Net.Http;

namespace ConsoleWebApi.Utils
{
    public static class Common
    {
        public static string GetToken(HttpRequestMessage request)
        {
            var _req = GetOwinConext(request).Request;
            string token = _req.Query["token"]; 
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }
            token = _req.Headers["token"];
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }
            if (_req.Cookies["token"] != null)
            {
                token = _req.Cookies["token"];
                if (!string.IsNullOrEmpty(token))
                {
                    return token;
                }
            }

            return string.Empty;
        }

        public static string GetClientIp(HttpRequestMessage request)
        {
            string uip = GetOwinConext(request).Request.RemoteIpAddress;
            return uip;
        }

        public static RequestClientInfo GetClientInfo(HttpRequestMessage request)
        {
            string agent = request.Headers.UserAgent.ToString();
            return GetClientInfo(request, agent);
        }
        public static RequestClientInfo GetClientInfo(HttpRequestMessage request, string cont)
        {
            string agent = cont;
            var uaParser = Parser.GetDefault();
            var client = uaParser.Parse(agent);
            RequestClientInfo clientInfo = new RequestClientInfo();
            clientInfo.SourceCont = agent;
            clientInfo.OS = client.OS.Family;
            clientInfo.Device = client.Device.Family;
            clientInfo.Agent = client.UserAgent.Family;
            clientInfo.IP = GetClientIp(request);
            return clientInfo;
        }

        public static void SetCookiesValue(HttpRequestMessage request, string key, string value, string domain = "")
        {
            var response = GetOwinConext(request).Response;
            if (string.IsNullOrEmpty(domain))
            {
                response.Cookies.Append(key, value);
            }
            else
            {
                response.Cookies.Append(key, value, new CookieOptions() { Domain = domain });
            }

        }

        public static IOwinContext GetOwinConext(HttpRequestMessage request)
        {
            var cxt = (IOwinContext)request.Properties["MS_OwinContext"];
            return cxt;
        }
    }
}