using System;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace PAX.ServerModules
{
    public class CustomServerHeaderModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += this.OnPreSendRequestHeaders;
        }

        public void Dispose()
        { }

        void OnPreSendRequestHeaders(object sender, EventArgs e)
        {

            HttpApplication app = sender as HttpApplication;
            if (null != app && null != app.Request && !app.Request.IsLocal &&
                null != app.Context && null != app.Context.Response)
            {

                NameValueCollection headers = app.Context.Response.Headers;
                if (null != headers)
                {
                    headers.Remove("Server"); 
                    headers.Remove("X-AspNet-Version"); 
                    headers.Remove("X-AspNetMvc-Version"); 
                    headers.Remove("X-Powered-By");
                }
            }
        }
    }
}