using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Web;
namespace SolucionPruebas.Servicios.ConfirmacionUsuario
{
    public class clsServiceUsernameTokenManager : UsernameTokenManager
    {
        public clsServiceUsernameTokenManager()
        { }

        public clsServiceUsernameTokenManager(XmlNodeList xnNodos)
            : base(xnNodos)
        { }

        protected override string AuthenticateToken(UsernameToken token)
        {
            string sClaveUsuario = token.Username;

            char[] ch = sClaveUsuario.ToCharArray();
            Array.Reverse(ch);
            return new string(ch);
        }  
    }
}
