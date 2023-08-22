using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace PAXRecuperacionSOAPBateriaGT
{
    public class MyMessageInspector : IClientMessageInspector
    {
        public string LastRequestXML { get; private set; }
        public string LastRequestEncabezado { get; private set; }
        public string LastResponseXML { get; private set; }
        public string LastResponseEncabezado { get; private set; }

        string sContentLength, sCacheControl, sContentType, sDate, sServer, sVersion, sPowered, sMvc;

        clsRecuperacionSOAP recuperaSOAP = new clsRecuperacionSOAP();

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

            if (!string.IsNullOrEmpty(LastResponseXML))
            {
                recuperaSOAP.recuperarResponse(sContentLength, sCacheControl, sContentType, sDate, sServer, sVersion, sPowered, sMvc, LastResponseXML);
            }
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            LastRequestXML = request.ToString();

            string sRequest = string.Empty;
            if (!string.IsNullOrEmpty(LastRequestXML))
            {
                recuperaSOAP.recuperaRequest(LastRequestXML);
            }
            return request;
        }
    }
}
