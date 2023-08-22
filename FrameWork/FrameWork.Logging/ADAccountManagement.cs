using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;

namespace FrameWork.Logging
{
    public class ADAccountManagement
    {
        private string _sDominio;
        private string _sOU;
        private string _sPasswordServicio;
        private string _sUsuarioServicio;

        private string sDominio
        {
            get { return _sDominio; }
            set { _sDominio = value; }
        }
       
        private string sUsuarioServicio
        {
            get { return _sUsuarioServicio; }
            set { _sUsuarioServicio = value; }
        }        

        private string sPasswordServicio
        {
            get { return _sPasswordServicio; }
            set { _sPasswordServicio = value; }
        }

        private string sOU
        {
            get { return _sOU; }
            set { _sOU = value; }
        }
        
        public ADAccountManagement(string psDominio, string psUsuarioServicio, string psPasswordServicio)
        {
            _sDominio = psDominio;
            _sUsuarioServicio = psUsuarioServicio;
            _sPasswordServicio = psPasswordServicio;
            _sOU = "OU=pax Users,OU=pax,DC=pax,DC=com";
        }

        public ADAccountManagement(string psDominio, string psUsuarioServicio, string psPasswordServicio, string psOU)
        {
            _sDominio = psDominio;
            _sUsuarioServicio = psUsuarioServicio;
            _sPasswordServicio = psPasswordServicio;
            _sOU = psOU;
        }

        public bool EsUsuarioBloqueado(string psUsuario)
        {
            UserPrincipal oUserPrincipal = fnObtenerUsuario(psUsuario);
            return oUserPrincipal.IsAccountLockedOut();
        }

        public bool EsUsuarioExpirado(string psUsuario)
        {
            UserPrincipal oUserPrincipal = fnObtenerUsuario(psUsuario);
            if (oUserPrincipal.AccountExpirationDate != null)
                return false;
            else
                return true;
        }

        public bool EsUsuarioExistente(string psUsuario)
        {
            if (fnObtenerUsuario(psUsuario) == null)
                return false;
            else
                return true;
        }

        public bool fnValidarCredenciales(string psUsuario, string psPassword)
        {
            PrincipalContext oPrincipalContext = fnObtenerContextoPrincipal();
            return oPrincipalContext.ValidateCredentials(psUsuario, psPassword);
        }

        private UserPrincipal fnObtenerUsuario(string psUsuario)
        {
            PrincipalContext oPrincipalContext = fnObtenerContextoPrincipal();
            UserPrincipal oUserPrincipal = UserPrincipal.FindByIdentity(oPrincipalContext, psUsuario);
            return oUserPrincipal;
        }

        private PrincipalContext fnObtenerContextoPrincipal()
        {
            PrincipalContext oPrincipalContext = new PrincipalContext(ContextType.Domain, sDominio,
                sOU, ContextOptions.SimpleBind, sUsuarioServicio, sPasswordServicio);
            return oPrincipalContext;
        }

        private PrincipalContext fnObtenerContextoPrincipal(string psOU)
        {
            PrincipalContext oPrincipalContext = new PrincipalContext(ContextType.Domain, sDominio, 
                psOU, ContextOptions.SimpleBind, sUsuarioServicio, sPasswordServicio);
            return oPrincipalContext;
        }
    }
}
