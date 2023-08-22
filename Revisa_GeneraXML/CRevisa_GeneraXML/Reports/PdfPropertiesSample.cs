using Root.Reports;
using System;

// Creation date: 08.08.2002
// Checked: 12.12.2002
// Author: Otto Mayer, mot@root.ch
// Version 1.01.00

// copyright (C) 2002 root-software ag  -  Bürglen Switzerland  -  www.root.ch; Otto Mayer, Stefan Spirig, Roger Gartenmann
// This library is free software; you can redistribute it and/or modirY it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Samples.Reports {
  /// <summary>PDF Properties Sample</summary>
  class PdfPropertiesSample : Report {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Starts the PDF Properties sample.</summary>
    public  void _Main() {
      PdfFormatter pf = new PdfFormatter();
      pf.sTitle = "PDF Sample";
      pf.sAuthor = "Otto Mayer, mot@root.ch";
      pf.sSubject = "Sample of some PDF features";
      pf.sKeywords = "Sample PDF RSF";
      pf.sCreator = "RSF Sample Application";
      pf.dt_CreationDate = new DateTime(2002, 8, 15, 0,0,0,0);
      pf.pageLayout = PageLayout.TwoColumnLeft;
      pf.bHideToolBar = true;
      pf.bHideMenubar = false;
      pf.bHideWindowUI = true;
      pf.bFitWindow = true;
      pf.bCenterWindow = true;
      pf.bDisplayDocTitle = true;

      RT.ViewPDF(new PdfPropertiesSample(pf), "PdfPropertiesSample.pdf");
    }

    //----------------------------------------------------------------------------------------------------x
    public PdfPropertiesSample(Root.Reports.Formatter formatter) : base(formatter) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates this report</summary>
    protected override void Create() {
      FontDef fd = new FontDef(this, "Helvetica");
      FontProp fp = new FontPropMM(fd, 4);
      FontProp fp_Title = new FontPropMM(fd, 11);
      fp_Title.bBold = true;

      Page page = new Page(this);
      page.AddCenteredMM(40, new RepString(fp_Title, "PDF Properties Sample"));
      fp_Title.rSizeMM = 8;
      page.AddCenteredMM(100, new RepString(fp_Title, "First Page"));
      page.AddCenteredMM(120, new RepString(fp, "Choose <Document Properties, Summary> from the"));
      page.AddCenteredMM(126, new RepString(fp, "File menu to display the document properties"));

      page = new Page(this);
      page.AddCenteredMM(100, new RepString(fp_Title, "Second Page"));
    }

  }
}
