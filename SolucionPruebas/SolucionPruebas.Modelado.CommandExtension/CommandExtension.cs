using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using Microsoft.VisualStudio.Uml.Classes;

namespace SolucionPruebas.Modelado.CommandExtension
{
    // Custom context menu command extension
    // See http://msdn.microsoft.com/en-us/library/ee329481(VS.100).aspx
    [Export(typeof(ICommandExtension))]
    [ClassDesignerExtension]
    [UseCaseDesignerExtension]
    [SequenceDesignerExtension]
    [ComponentDesignerExtension]
    [ActivityDesignerExtension]
    class CommandExtension : ICommandExtension
    {
        [Import]
        IDiagramContext context { get; set; }

        public void Execute(IMenuCommand command)
        {
            // TODO: Add the logic for your command extension here

            // The following example creates a new class in the model store
            // and displays it on the current diagram.
            IClassDiagram diagram = context.CurrentDiagram as IClassDiagram;
            IModelStore store = diagram.ModelStore;
            IPackage rootPackage = store.Root;
            IClass newClass = rootPackage.CreateClass();
            newClass.Name = "SolucionPruebas.Modelado.CommandExtension";
            diagram.Display(newClass);
        }

        public void QueryStatus(IMenuCommand command)
        {
            command.Enabled = context.CurrentDiagram != null
                    && context.CurrentDiagram.ChildShapes.Count() > 0;
        }

        public string Text
        {
            get { return "Save To Image..."; }
        }

        /// <summary>
        /// Ask the user for the path of an image file.
        /// </summary>
        /// <returns>image file path, or null</returns>
        private string FileNameFromUser()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "image.bmp";
            dialog.Filter = "Bitmap ( *.bmp )|*.bmp|JPEG File ( *.jpg )|*.jpg|Enhanced Metafile (*.emf )|*.emf|Portable Network Graphic ( *.png )|*.png";
            dialog.FilterIndex = 1;
            dialog.Title = "Save Diagram to Image";
            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
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
