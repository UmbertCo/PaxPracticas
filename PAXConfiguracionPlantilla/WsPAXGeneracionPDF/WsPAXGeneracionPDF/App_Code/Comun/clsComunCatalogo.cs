using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

/// <summary>
/// Summary description for clsComunCatalogo
/// </summary>
public class clsComunCatalogo
{

	public clsComunCatalogo()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Limpia los valores de todos los controles de un formulario
    /// </summary>
    /// <param name="pCtrlFormulario">El control contenedor de un formulario el cual debe ser un control web de servidor</param>
    public static void fnLimpiarFormulario(Control pCtrlFormulario)
    {
        try
        {
            foreach (TextBox txt in pCtrlFormulario.Controls.OfType<TextBox>())
            {
                txt.Text = string.Empty;
            }
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Carga los datos de una fila del GridView a un formulario con controles correspondientes
    /// </summary>
    /// <param name="pgvrFila">La fila con los datos</param>
    /// <param name="pCtrlFormulario">El formulario a llenar</param>
    public static void fnAsignarValorFila(GridViewRow pgvrFila, Control pCtrlFormulario)
    {
        foreach (TextBox txt in pCtrlFormulario.Controls.OfType<TextBox>())
        {
            try
            {
                txt.Text = ((Label)pgvrFila.FindControl(txt.ID.Replace("txt", "lbl"))).Text;
            }
            catch { }
        }

        foreach (DropDownList ddl in pCtrlFormulario.Controls.OfType<DropDownList>())
        {
            try
            {
                ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByText(((Label)pgvrFila.FindControl(ddl.ID.Replace("ddl", "lbl"))).Text));
            }
            catch
            {
                //Sin acción
            }
        }
    }
}