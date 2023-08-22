using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using OpenSSL_Lib.Properties;

namespace OpenSSL_Lib
{

    public enum enuMetodoDigestion 
    {
    
        SHA1,
        SHA256
    
    }

    public enum DocTipo 
    {
    
        KEY,
        
        CER,
       
        PFX
        
    }

    public enum enuEstatus
    {
        PERMISOS,
        PASS,
        CER_LLAVE_INC,
        OK,
        VALIDACIONERR,
        ERROPENSSL,
        DESCONOCIDO
        
    
    }

}