using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


[ServiceContract(Namespace = "https://test.paxfacturacion.com.mx:454")]
public interface IwcfValidaCFDI
{
    [OperationContract]
    string fnValidaXML(string psComprobante, string sNombre, string sContraseña, string sVersion);


    [OperationContract]
    string fnValidaCFD(string psComprobante, string sNombre, string sContraseña, string sVersion);
}