using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Root.Reports;
using System.Drawing;


namespace clsPlantillaUnionProgreso
{
    class clsEstadoCuenta
    {

        #region Propiedades
        Report PDF;

            #region Propiedades Pagina
        Page pPaginaActual;

        double nPosy;

        int nPagina;

        double nAnchoPagina;

        double nAltoPagina;

        double nMargenPagina;
        
        #endregion        

        

        #endregion



        private void init() 
        {

            Formatter fFormato = new PdfFormatter();

            PDF = new Report(fFormato);

            pPaginaActual = new Page(PDF);

            nPosy = 0;

            nPagina = 1;

            nAltoPagina = RT.rPointFromMM(279.4);

            nAnchoPagina = RT.rPointFromMM(215.9);

            nMargenPagina = RT.rPointFromMM(0);
        }



        private StaticContainer fnCrearPanelEstado()
        {
            

            StaticContainer panelEstado = new StaticContainer(nAnchoPagina - nMargenPagina * 8, RT.rPointFromMM(altoEncabezado));

            FontProp fPropRojoObscuro = new FontProp(fuenteNormal, 10);
            fPropRojoObscuro.color = Color.DarkRed;
            fPropRojoObscuro.bBold = true;

            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 250;
            double pfAlto = 38;

            panelEstado.Add(pfPosX, pfPosY, new RepLine(penGruesa, pfAncho, 0));
            panelEstado.Add(pfPosX, pfAlto, new RepLine(penGruesa, pfAncho, 0));

            fPropNormal.bBold = true;
            panelEstado.Add(5, -3, new RepString(fPropRojoObscuro, "Estado de Cuenta"));
            panelEstado.Add(5, 10, new RepString(fPropNormal, "Periodo"));
            panelEstado.Add(5, 22, new RepString(fPropNormal, "Fecha Expedición"));
            panelEstado.Add(5, 34, new RepString(fPropNormal, "Página"));
            fPropChica.bBold = false;

            return panelEstado;
        }
    }
}
