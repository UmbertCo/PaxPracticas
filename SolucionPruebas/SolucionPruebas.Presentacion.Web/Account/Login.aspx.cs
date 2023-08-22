using SolucionPruebas.Presentacion.Servicios;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;

                if (LoginUser.UserName.ToString().Equals(string.Empty) || LoginUser.Password.ToString().Equals(string.Empty))
                    return;


                List<string> alDomains = new List<string>();
                Forest currentForest = Forest.GetCurrentForest();
                DomainCollection myDomains = currentForest.Domains;

                foreach (Domain objDomain in myDomains)
                {
                    alDomains.Add(objDomain.Name);
                }


                PrincipalContext oPrincipalContext = new PrincipalContext(ContextType.Domain, "192.168.3.254",
                "OU=pax Users,OU=pax,DC=pax,DC=com", ContextOptions.SimpleBind, LoginUser.UserName.ToString(), LoginUser.Password.ToString());

                DirectoryEntry deBase = new DirectoryEntry("LDAP://192.168.3.254:389/dc=dom,dc=fr");
                OuInTheFormOf(deBase, "", "");
                GetADUsers();

                Entidades.Mensajes ENMensajes = new Entidades.Mensajes();

                SolucionPruebas.Presentacion.Servicios.SesionServicioLocal.SesionServiceClient SDSesion = ProxyLocator.ObtenerSesionServicioLocal();
                SDSesion.fnIniciarSesion(LoginUser.UserName.ToString(), LoginUser.Password.ToString(), ref ENMensajes);

            }
            catch (FaultException ex)
            {
                lblError.Text = ex.Message;
            }
            catch (Exception ex)
            {
                LoginUser.FailureText = ex.Message;
            }
        }

        public void GetADUsers()
        {
            try
            {
                string DomainPath = "LDAP://DC=pax.local";
                DirectoryEntry searchRoot = new DirectoryEntry(DomainPath);
                DirectorySearcher search = new DirectorySearcher(searchRoot);
                search.Filter = "(&(objectClass=user)(objectCategory=person))";
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("usergroup");
                search.PropertiesToLoad.Add("displayname");//first name
                SearchResult result;
                SearchResultCollection resultCol = search.FindAll();
                if (resultCol != null)
                {
                    for (int counter = 0; counter < resultCol.Count; counter++)
                    {
                        string UserNameEmailString = string.Empty;
                        result = resultCol[counter];
                        if (result.Properties.Contains("samaccountname") && result.Properties.Contains("mail") && result.Properties.Contains("displayname"))
                        {
                            //Users objSurveyUsers = new Users();
                            //objSurveyUsers.Email = (String)result.Properties["mail"][0] +
                            //  "^" + (String)result.Properties["displayname"][0];
                            //objSurveyUsers.UserName = (String)result.Properties["samaccountname"][0];
                            //objSurveyUsers.DisplayName = (String)result.Properties["displayname"][0];
                            //lstADUsers.Add(objSurveyUsers);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        static List<DirectoryEntry> OuInTheFormOf(DirectoryEntry deBase, string ou1, string ou2)
        {
            List<DirectoryEntry> deList = null;

            /* Directory Search
             */
            DirectorySearcher dsLookFor = new DirectorySearcher(deBase);
            dsLookFor.Filter = ou1;
            dsLookFor.SearchScope = SearchScope.Subtree;
            dsLookFor.PropertiesToLoad.Add("ou");

            SearchResultCollection srcOUs = dsLookFor.FindAll();

            if (srcOUs.Count != 0)
            {
                deList = new List<DirectoryEntry>();

                foreach (SearchResult srOU in srcOUs)
                {
                    DirectoryEntry deOU = srOU.GetDirectoryEntry();
                    if (deOU.Parent.Name.ToUpper() == ou2.ToUpper())
                        deList.Add(deOU);
                }
            }
            return deList;
        }
    }
}
