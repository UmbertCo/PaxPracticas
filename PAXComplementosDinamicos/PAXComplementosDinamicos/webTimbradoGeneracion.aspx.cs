using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webTimbradoGeneracion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page objp = new Page();

        if (objp.Session["Complemento"] != null)
        {
            objp.Session["Complemento"] = null;
        }
    }
}