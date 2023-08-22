using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OpenSSL_Lib
{
    //pendiendte de terminar esta clase
    class OpenSSLException: Exception
    {
        public enuEstatus eError { set; get; }

        String sOutputMsj;



        StreamReader srLectorOutput;

        StreamReader srLectorErr;

        //public OpenSSLException(String psMensaje)
        //    : base(psMensaje)
        //{
        //    eError = enuEstatus.OK;
        //}

        public OpenSSLException(enuEstatus peError, String psMensaje)
            : base(psMensaje)
        {
            eError = peError;
        
        }

        //public OpenSSLException(enuEstatus peError, String psMensaje, StreamReader psrOut,StreamReader psrErr)
        //    : this(peError, psMensaje)
        //{
        //    srLectorOutput = psrOut;

        //    srLectorErr = psrErr;
        
        //}

        

    }
}
