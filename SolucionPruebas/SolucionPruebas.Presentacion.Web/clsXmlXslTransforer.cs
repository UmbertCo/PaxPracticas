using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


/// <summary>
/// Summary description for XmlXslTransforer.
/// </summary>
public class clsXmlXslTransforer
{
    private readonly string XslFile = @"D:\Forms Dinamicos\Plantilla.xslt";
    private readonly string XmlFile = @"D:\Forms Dinamicos\Plantilla.xml";

	public System.Web.UI.WebControls.Panel ControlHolder;

    public clsXmlXslTransforer()
	{
		//
		// TODO: Add constructor logic here
		//
	}

	public void TransformFields(string pageId)
	{
		/**
			* Transform aspx page with fields from xml file
			* Get transform result as a string
			* Parse controls into a parent control holder
			*/

		XmlDocument xdoc = new XmlDocument();
		xdoc.Load(XmlFile);

		// load xslt to do transformation
		XslTransform xsl = new XslTransform();
		xsl.Load(XslFile);

		// load xslt arguments to load specific page from xml file
		// this can be used if you have multiple pages in your xml file and you loading them one at a time
		XsltArgumentList xslarg = new XsltArgumentList();
		xslarg.AddParam("pageid", "", pageId);

		// get transformed results
		StringWriter sw = new StringWriter();
		xsl.Transform(xdoc, xslarg, sw);
		string result = sw.ToString().Replace("xmlns:asp=\"remove\"","").Replace("&lt;","<").Replace("&gt;",">");

		// free up the memory of objects that are not used anymore
		sw.Close();

		// parse the controls and add it to the page
		Control ctrl = ControlHolder.Page.ParseControl(result);
		ControlHolder.Controls.Add(ctrl);	
	}
}

