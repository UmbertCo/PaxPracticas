using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace SolucionPruebas.Presentacion.ConsoleApplication.Diagramas
{
    [Export(typeof(ICommandExtension))]
    [ClassDesignerExtension]
    [UseCaseDesignerExtension]
    [SequenceDesignerExtension]
    [ComponentDesignerExtension]
    [ActivityDesignerExtension]
    class Program
    {
        [Import]
        static IDiagramContext Context { get; set; }

        public static void Main(string[] args)
        {
            Execute();
        }

        public static void Execute()
        {
            // Get the diagram of the underlying implementation.
            //Diagram dslDiagram = Context.CurrentDiagram.GetObject<Diagram>();
            //if (dslDiagram != null)
            //{
            //    string imageFileName = string.Empty;
            //    imageFileName = "DiagramaPrueba";
            //    if (!string.IsNullOrEmpty(imageFileName))
            //    {
            //        Bitmap bitmap = dslDiagram.CreateBitmap(
            //         dslDiagram.NestedChildShapes,
            //         Diagram.CreateBitmapPreference.FavorClarityOverSmallSize);
            //        bitmap.Save(imageFileName, GetImageType(imageFileName));
            //    }
            //}
        }

        /// <summary>
        /// Return the appropriate image type for a file extension.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static ImageFormat GetImageType(string fileName)
        {
            string extension = System.IO.Path.GetExtension(fileName).ToLowerInvariant();
            ImageFormat result = ImageFormat.Bmp;
            switch (extension)
            {
                case ".jpg":
                    result = ImageFormat.Jpeg;
                    break;
                case ".emf":
                    result = ImageFormat.Emf;
                    break;
                case ".png":
                    result = ImageFormat.Png;
                    break;
            }
            return result;
        }
    }
}
