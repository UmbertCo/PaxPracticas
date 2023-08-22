using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

namespace ProbarServicio
{
    public class MyMessageInspector : IClientMessageInspector
    {
        public string LastRequestXML { get; private set; }
        public string LastRequestEncabezado { get; private set; }
        public string LastResponseXML { get; private set; }
        public string LastResponseEncabezado { get; private set; }

        string sContentLength, sCacheControl, sContentType, sDate, sServer, sVersion, sPowered, sMvc;

        void IClientMessageInspector.AfterReceiveReply(ref Message reply, Object correlationState)
        {
            HttpResponseMessageProperty prop =
                reply.Properties[HttpResponseMessageProperty.Name.ToString()] as HttpResponseMessageProperty;


            #region leyendoHeaderResponse

            for (int i = 0; i < prop.Headers.Count; i++)
            {
                if (prop.Headers.AllKeys[i].ToString().Contains("Content-Length"))
                {
                    try
                    {
                        sContentLength = prop.Headers["Content-Length"].ToString();
                        //Logger.Escribir(prop.Headers.AllKeys[i].ToString() + ": " + sContentLength);
                    }
                    catch { }
                }

                if (prop.Headers.AllKeys[i].ToString().Contains("Cache-Control"))
                {
                    try
                    {
                        sCacheControl = prop.Headers["Cache-Control"].ToString();
                        //Logger.Escribir(prop.Headers.AllKeys[i].ToString() + ": " + sCacheControl);
                    }
                    catch { }
                }

                if (prop.Headers.AllKeys[i].ToString().Contains("Content-Type"))
                {
                    try
                    {
                        sContentType = prop.Headers["Content-Type"].ToString();
                        //Logger.Escribir(prop.Headers.AllKeys[i].ToString() + ": " + sContentType);
                    }
                    catch { }
                }

                if (prop.Headers.AllKeys[i].ToString().Contains("Date"))
                {
                    try
                    {
                        sDate = prop.Headers["Date"].ToString();
                        //Logger.Escribir(prop.Headers.AllKeys[i].ToString() + ": " + sDate);
                    }
                    catch { }
                }

                if (prop.Headers.AllKeys[i].ToString().Contains("Server"))
                {
                    try
                    {
                        sServer = prop.Headers["Server"].ToString();
                        //Logger.Escribir(prop.Headers.AllKeys[i].ToString() + ": " + sServer);
                    }
                    catch { }
                }

                if (prop.Headers.AllKeys[i].ToString().Contains("X-AspNet-Version"))
                {
                    try
                    {
                        sVersion = prop.Headers["X-AspNet-Version"].ToString();
                        //Logger.Escribir(prop.Headers.AllKeys[i].ToString() + ": " + sVersion);
                    }
                    catch { }
                }

                if (prop.Headers.AllKeys[i].ToString().Contains("X-Powered-By"))
                {
                    try
                    {
                        sPowered = prop.Headers["X-Powered-By"].ToString();
                        //Logger.Escribir(prop.Headers.AllKeys[i].ToString() + ": " + sPowered);
                    }
                    catch { }
                }

                if (prop.Headers.AllKeys[i].ToString().Contains("X-AspNetMvc-Version"))
                {
                    try
                    {
                        sMvc = prop.Headers["X-AspNetMvc-Version"].ToString();
                        //Logger.Escribir(prop.Headers.AllKeys[i].ToString() + ": " + sPowered);
                    }
                    catch { }
                }
            }

            #endregion

            LastResponseXML = reply.ToString();

            //LastRequestEncabezado = encabezado.ToString();
            if (!string.IsNullOrEmpty(LastResponseXML))
            {
                Response vResponse = new Response(sContentLength, sCacheControl, sContentType, sDate, sServer, sVersion, sPowered, sMvc, LastResponseXML);
                //Logger.Escribir(LastResponseXML);
                //Logger.Escribir("");
            }
        }


        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, /*System.ServiceModel.Channels.AddressHeaderCollection encabezado*/ System.ServiceModel.IClientChannel channel)
        {

            //HttpRequestMessageProperty pro = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
            HttpRequestMessageProperty prop = request.Properties[HttpRequestMessageProperty.Name.ToString()] as HttpRequestMessageProperty;
            //pro.Headers[HttpRequestHeader.Expect] = "holi";

            //try { Logger.Escribir("Expect: " + prop.Headers[HttpRequestHeader.Expect].ToString()); }
            //catch { }

            //try { Logger.Escribir(prop.Method + " " + Program.direccionServicio); }
            //catch { }

            //try { Logger.Escribir("SOAPAction: " + ": " + request.Headers.Action); }
            //catch { }

            ////VsDebuggerCausalityData
            //try { Logger.Escribir(prop.Headers.ToString()); }
            //catch { }

            //try { Logger.Escribir("Host: " + Program.hostServicio+":"+Program.portServicio); }
            //catch { }

            LastRequestXML = request.ToString();

            string sRequest = string.Empty;
            //string[] request_lenght;

            //LastRequestEncabezado = encabezado.ToString();
            if (!string.IsNullOrEmpty(LastRequestXML))
            {
                Request vResponse = new Request(LastRequestXML);

                //sRequest = LastRequestXML.Replace("<s:Body>", "@<s:Body>");
                //request_lenght = sRequest.Split('@');

                //var utf8 = Encoding.UTF8;
                //byte[] utfBytes = utf8.GetBytes(request_lenght[1]);

                //Logger.Escribir("Content-Length: " + (utfBytes.Length - 2));

                Logger.Escribir("");

                Logger.Escribir(LastRequestXML);
                Logger.Escribir("");
            }
            return request;
        }
    }
}
