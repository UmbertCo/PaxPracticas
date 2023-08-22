using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Summary description for ThemeManager
/// </summary>
public class ThemeManager
{
    public static List<Theme> GetThemes()
    {
        DirectoryInfo dInfo = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("App_Themes"));
        DirectoryInfo[] dArrInfo = dInfo.GetDirectories();

        List<Theme> list = new List<Theme>();
        foreach (DirectoryInfo sDirectory in dArrInfo)
        {
            Theme temp = new Theme(sDirectory.Name);
            list.Add(temp);
        }
        return list;
    }
}