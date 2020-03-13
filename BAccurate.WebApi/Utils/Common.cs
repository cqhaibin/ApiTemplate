using BAccurate.Models;
using BAccurate.WebApi.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAccurate.WebApi.Utils
{
    public static class Common
    {
        public static string GetToken()
        {
            string token = HttpContext.Current.Request["token"];
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }
            token = HttpContext.Current.Request.Headers["token"];
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }
            if (HttpContext.Current.Request.Cookies["token"] != null)
            {
                token = HttpContext.Current.Request.Cookies["token"].Value;
                if (!string.IsNullOrEmpty(token))
                {
                    return token;
                }
            }

            return string.Empty;
        }

        public static string GetClientIp()
        {
            if (HttpContext.Current.Request == null) return "";

            string uip = "";
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                uip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                uip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return uip;
        }

        public static RequestClientInfo GetClientInfo()
        {
            string agent = HttpContext.Current.Request.Headers["User-Agent"].ToString();
            return GetClientInfo(agent);
        }
        public static RequestClientInfo GetClientInfo(string cont)
        {
            string agent = cont;
            var uaParser = Parser.GetDefault();
            var client = uaParser.Parse(agent);
            RequestClientInfo clientInfo = new RequestClientInfo();
            clientInfo.SourceCont = agent;
            clientInfo.OS = client.OS.Family;
            clientInfo.Device = client.Device.Family;
            clientInfo.Agent = client.UserAgent.Family;
            clientInfo.IP = GetClientIp();
            return clientInfo;
        }

        public static void SetCookiesValue(string key, string value, string domain = "")
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = value;
            cookie.HttpOnly = true;
            if (!string.IsNullOrEmpty(domain) && domain.Length > 0)
            {
                cookie.Domain = domain;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}