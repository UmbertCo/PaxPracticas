using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        bool fnLeerCER(string NombreArchivo, out string Inicio, out string Final, out string Serie, out string Numero);

        [OperationContract]
        string fnCertificado(string ArchivoCER);

        [OperationContract]
        string fnCrearSello(string sXMLsinSello, string sKey, string sClavePrivada, string sArchivoExtXML);


    }
}
    
