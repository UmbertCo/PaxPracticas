using Root.Reports;
using System;
using System.Drawing;

// Creation date: 25.07.2002
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

namespace ReportSamples {
  /// <summary>Image Sample</summary>
  class ImageSample : Report {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Starts the image sample.</summary>
    public  void _Main() {
      RT.ViewPDF(new ImageSample(), "ImageSample.pdf");
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates this report</summary>
    protected override void Create() {
      FontDef fd = new FontDef(this, "Arial");
      FontProp fp = new FontPropMM(fd, 2.1);
      FontProp fp_Title = new FontPropMM(fd, 18);
      fp_Title.bBold = true;
      BrushProp bp = new BrushProp(this, Color.LightGray);
      PenProp pp = new PenProp(this, 0.2, Color.FromArgb(235, 235, 235));

      new Page(this);
      Double rY = 40;
      page_Cur.AddCenteredMM(rY, new RepString(fp_Title, "Image Sample"));
      fp_Title.rSizeMM = 4;

      System.IO.Stream stream = GetType().Assembly.GetManifestResourceStream("ReportSamples.Image.jpg");

      page_Cur.AddMM(20, 90, new RepImageMM(stream, 40, Double.NaN));
      page_Cur.AddMM(20, 95, new RepString(fp, "W = 40mm, H = auto."));
      page_Cur.AddMM(67, 90, new RepImageMM(stream, 40, 20));
      page_Cur.AddMM(67, 95, new RepString(fp, "W = 40mm, H = 20mm"));
      page_Cur.AddMM(114, 90, new RepImageMM(stream, Double.NaN, 30));
      page_Cur.AddMM(114, 95, new RepString(fp, "W = auto., H = 30mm"));
      page_Cur.AddMM(161, 90, new RepImageMM(stream, 30, 30));
      page_Cur.AddMM(161, 95, new RepString(fp, "W = 30mm, H = 30mm"));
      rY +=  150;

      // adjust the size of a bounding rectangle
      RepRect dr = new RepRectMM(bp, 80, 60);
      page_Cur.AddMM(20, rY, dr);
      RepImage di  = new RepImageMM(stream, 70, Double.NaN);
      page_Cur.AddMM(25, rY - 5, di);
      dr.rHeightMM = di.rHeightMM + 10;

      // rotated image
      di = new RepImageMM(stream, 40, 30);
      di.RotateTransform(-15);
      page_Cur.AddMM(120, rY - 33, di);
      
      // rotated image with rectangle
      StaticContainer sc = new StaticContainer(RT.rMM(45), RT.rMM(35));
      page_Cur.AddMM(145, rY - 35, sc);
      sc.RotateTransform(15);
      sc.AddMM(0, 35, new RepRectMM(bp, 45, 35));
      sc.AddMM(1.25, 33.75, new RepLineMM(pp, 42.5, 0));
      sc.AddMM(1.25, 1.25, new RepLineMM(pp, 42.5, 0));
      sc.AddAlignedMM(22.5, RepObj.rAlignCenter, 17.5, RepObj.rAlignCenter, new RepImageMM(stream, 40, 30));
      rY += 30;

      // alignment sample
      page_Cur.AddMM(20, rY, new RepString(fp_Title, "Alignment"));
      rY += 18;
      Int32 rX = 100;
      Double rD = 20;
      bp.color = Color.DarkSalmon;
      page_Cur.AddMM(rX, rY + rD, new RepRectMM(bp, rD, rD));
      page_Cur.AddAlignedMM(rX, RepObj.rAlignRight, rY, RepObj.rAlignBottom, new RepImageMM(stream, 20, Double.NaN));
      page_Cur.AddAlignedMM(rX, RepObj.rAlignRight, rY + rD, RepObj.rAlignTop, new RepImageMM(stream, 20, Double.NaN));
      page_Cur.AddMM(rX + rD, rY, new RepImageMM(stream, 20, Double.NaN));  // default
      page_Cur.AddAlignedMM(rX + rD, RepObj.rAlignLeft, rY + rD, RepObj.rAlignTop, new RepImageMM(stream, 20, Double.NaN));
      page_Cur.AddAlignedMM(rX + rD / 2, RepObj.rAlignCenter, rY + rD / 2, RepObj.rAlignCenter, new RepImageMM(stream, 10, Double.NaN));
    }

  }
}
