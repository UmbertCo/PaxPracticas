using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IwcfRecepcion" in both code and config file together.
[ServiceContract(Namespace = "https://test.paxfacturacion.com.mx:454")]
public interface IwcfRecepcion
{

    [OperationContract]
    string fnEnviarTXT(string psComprobante, string psTipoDocumento, int pnId_Estructura, string sNombre, string sContraseña, string sVersion, string sOrigen);

    [OperationContract]
    object[] fnEnviarTXTGeneralSeguros(string psComprobante, string psTipoDocumento, int pnId_Estructura, string sNombre, string sContraseña, string sVersion, string sOrigen);
}
