using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZXing;
using System.Drawing;

namespace TestjQueryWebcamPlugin
{
    public partial class WebCamCapture : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Linkbutton1_Click(object sender, EventArgs e)
        {
            // to how Pop and calling another Page in Popup.
            string url = "/WebCam/Captureimage.aspx";
            string s = "window.open('" + url + "', 'popup_window', 'width=900,height=460,left=100,top=100,resizable=no');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }

        protected void Linkbutton2_Click(object sender, EventArgs e)
        {
            // create a barcode reader instance
            IBarcodeReader reader = new BarcodeReader();
            // load a bitmap
            //var barcodeBitmap = (Bitmap)System.Drawing.Image.LoadFrom("C:\\sample-barcode-image.png");

            DateTime nm = DateTime.Now;
            string date = nm.ToString("yyyyMMdd");

            var barcodeBitmap = (Bitmap)System.Drawing.Image.FromFile(Server.MapPath("~/Userimages/") + date + ".jpg");

            //Session["capturedImageURL"] = Server.MapPath("~/Userimages/") + date + ".png";  
            // detect and decode the barcode inside the bitmap
            var result = reader.Decode(barcodeBitmap);
            // do something with the result
            if (result != null)
            {
                txtDecoderType.Text = result.BarcodeFormat.ToString();
                txtDecoderContent.Text = result.Text;
            }
        }
    }
}