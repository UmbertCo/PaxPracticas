using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebServidores
{
    public partial class _Contacto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Limpiar(object sender, ImageClickEventArgs e)
        {
            txtNombre.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            txtTitulo.Text = "";
            txtComentario.Text = "";
        }

        protected void Enviar(object sender, ImageClickEventArgs e)
        {
            Response.Write("<script>alert('Gracias por contactarnos " + txtNombre.Text + ". Responderemos a la brevedad posible.')</script>");
            Response.Write("<script language='javascript'>" + Environment.NewLine + "setTimeout(function(){ window.location.replace('../Default.aspx') },1000);</script>");
        }
    }
}