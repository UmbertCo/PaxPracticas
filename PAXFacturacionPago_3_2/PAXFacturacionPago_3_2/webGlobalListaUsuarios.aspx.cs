using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class webGlobalListaUsuarios : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        fnActualizar();
    }


    protected ArrayList fnLogin()
    {
        ArrayList aLista = new ArrayList();

        System.Collections.Generic.List<string> d = Application["UsersLoggedIn"] as System.Collections.Generic.List<string>;

        if (d != null)
        {
            for (int count = 0; count < d.Count;count++ )
            {
                aLista.Add(d[count].ToString());
            }
        }

        return aLista;
    }
    protected void btnActualziar_Click(object sender, EventArgs e)
    {
        fnActualizar();
    }

    private void fnActualizar()
    {
        ArrayList aLista = new ArrayList();
        aLista = fnLogin();

        lblTotalValue.Text = aLista.Count.ToString();
        lbxUsuarios.Items.Clear();
        foreach (var item in aLista)
        {
            
            lbxUsuarios.Items.Add(item.ToString());
        }
    }
}