using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TwoModalPopUpExtenderUpdatePanel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Handles the Click event of the btnOpenFirstDialogBox control.handels event for the first DialogBox
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnOpenFirstDialogBox_Click(object sender, EventArgs e)
    {
        mpeFirstDialogBox.Show();//opens the first dialogbox
    }

    /// <summary>
    /// Handles the Click event of the btnOpenSecondDialogBox control.handels event for the second DialogBox
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnOpenSecondDialogBox_Click(object sender, EventArgs e)
    {
        mpeFirstDialogBox.Show();//opens the first dialogbox
        mpeSecondDialogBox.Show();//opens the second dialogbox

    }
}