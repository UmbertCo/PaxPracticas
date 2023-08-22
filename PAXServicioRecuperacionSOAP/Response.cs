using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PAXServicioRecuperacionSOAP
{
    public partial class Response : Form
    {
        public static string sContentLength, sCacheControl, sContentType, sDate, sServer, sVersion, sPowered, sMvc, sBody;

        public Response()
        {
            InitializeComponent();
            lblContentL.Text = sContentLength;
            lblCache.Text = sCacheControl;
            lblContentT.Text = sContentType;
            lblDate.Text = sDate;
            lblServer.Text = sServer;
            lblVersion.Text = sVersion;
            lblPowered.Text = sPowered;
            lblMvc.Text = sMvc;
            txtBody.Text = sBody;
        }

        public Response(string ContentLength, string CacheControl, string ContentType, string Date, string Server, string Version, string Powered, string Mvc, string Body)
        {
            InitializeComponent();

            sContentLength = ContentLength;
            sCacheControl = CacheControl;
            sContentType = ContentType;
            sDate = Date;
            sServer = Server;
            sVersion = Version;
            sPowered = Powered;
            sMvc = Mvc;
            sBody = Body;
        }
    }
}
