using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebServidores
{
    public partial class _Comprar : System.Web.UI.Page
    {
        Double precio = 0, precio_Economy = 99.99, precio_Premium = 299.99, precio_ultimate = 699.99;
        Double precio_ftp = 50, precio_posicionamiento = 599, precio_kit=99;
        //variable para verificar si ha agregado por lo menos una vez
        Boolean a_agregado = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Contents["precio"] != null)
            {
                precio = Double.Parse(Session.Contents["precio"].ToString());
            }
            if (Request.QueryString["paquete"] != null)
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["paquete"] == "economico")
                    {
                        txtPaquete.Text = "ECONOMY";
                        precio = precio_Economy;
                        txtPrecio.Text = "$" + precio_Economy;
                        txtMeses.Text = "1 Mes";
                        txtDescripcion.Text = "1 Sitio web \nAncho de banda ilimitado \nEspacio en disco de 100GB \n10 Bases de datos \n100 direcciones de correo";
                        txtTotal.Text = "$" + precio;
                    }
                    if (Request.QueryString["paquete"] == "premium")
                    {
                        precio = precio_Premium;
                        txtPaquete.Text = "PREMIUM";
                        txtPrecio.Text = "$" + precio_Premium;
                        txtMeses.Text = "1 Mes";
                        txtDescripcion.Text = "Sitios Web ilimitados \nAncho de banda ilimitado \nEspacio en disco ilimitado \n25 Bases de datos \n500 direcciones de correo";
                        txtTotal.Text = "$" + precio;
                    }
                    if (Request.QueryString["paquete"] == "ultimate")
                    {
                        precio = precio_ultimate;
                        txtPaquete.Text = "ULTIMATE";
                        txtPrecio.Text = "$" + precio_ultimate;
                        txtMeses.Text = "1 Mes";
                        txtDescripcion.Text = "Sitios Web ilimitados \nAncho de banda ilimitado \nEspacio en disco ilimitado \nBases de datos ilimitadas\nCuentas de correo ilimitadas \nDNS Premium \nCertificado SSL";
                        txtTotal.Text = "$" + precio;
                    }
                }
            }
            else {
                Response.Redirect("Contratar.aspx");
            }
        }

        protected void maximizar(object sender, ImageClickEventArgs e)
        {
            panelMin.Visible = false;
            btnMax.Visible = false;
            panelMax.Visible = true;
            btnMin.Visible = true;
        }

        protected void minimizar(object sender, ImageClickEventArgs e)
        {
            panelMax.Visible = false;
            btnMin.Visible = false;
            panelMin.Visible = true;
            btnMax.Visible = true;
        }

        protected void sumarFTP(object sender, EventArgs e)
        {
            a_agregado = true;
            if (chkServidor.Checked == true)
            {
                precio += precio_ftp;
            }
            else
            {
                if (a_agregado == true)
                {
                    precio-= precio_ftp;
                }
            }
            txtTotal.Text = "$" + precio;
            Session.Contents["precio"] = precio;
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            Session.Contents["precio"] = precio;
        }

        protected void sumarPosicionamiento(object sender, EventArgs e)
        {
            a_agregado = true;
            if (chkPosicionamiento.Checked == true)
            {
                precio += precio_posicionamiento;
            }
            else
            {
                if (a_agregado == true)
                {
                    precio -= precio_posicionamiento;
                }
            }
            txtTotal.Text = "$" + precio;
            Session.Contents["precio"] = precio;
        }

        protected void sumarKit(object sender, EventArgs e)
        {
            a_agregado = true;
            if (chkKit.Checked == true)
            {
                precio += precio_kit;
            }
            else
            {
                if (a_agregado == true)
                {
                    precio -= precio_kit;
                }
            }
            txtTotal.Text = "$" + precio;
            Session.Contents["precio"] = precio;
        }
    }
}