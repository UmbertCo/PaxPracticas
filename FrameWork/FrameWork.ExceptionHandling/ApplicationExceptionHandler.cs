using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Assemblies;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace FrameWork.ExceptionHandling
{
    [ConfigurationElementType(typeof(CustomHandlerData))]
    public class ApplicationExceptionHandler : IExceptionHandler
    {
        private const string UNEXPECTED_ERROR = "Unexepected error";

        public ApplicationExceptionHandler(NameValueCollection ignore)
        {

        }

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            try
            {
                if (exception.GetType().Equals(typeof(CustomException.CriticalException)))
                {
                    if (HttpContext.Current.Items.Contains("ERROR_MSG"))
                    {
                        HttpContext.Current.Items.Remove("ERROR_MSG");
                    }

                    HttpContext.Current.Items.Add("ERROR_MSG", exception.Message);
                    HttpContext.Current.Server.Transfer("~/CriticalException.aspx", false);
                }
                else if (exception.GetType().Equals(typeof(CustomException.PresentationLayerException)))
                {
                    if ((ErrorCustomPage)HttpContext.Current.Handler != null)
                    {
                        ((ErrorCustomPage)HttpContext.Current.Handler).Error = "Presentation Layer Exception: " + exception.InnerException.Message;
                    }
                }
                else if (exception.GetType().Equals(typeof(CustomException.ViewLayerException)))
                {
                    if ((ErrorCustomPage)HttpContext.Current.Handler != null)
                    {
                        ((ErrorCustomPage)HttpContext.Current.Handler).Error = "View Layer Exception: " + exception.Message;
                    }
                }
                else if (exception.GetType().Equals(typeof(CustomException.DataAccesLayerException)))
                {
                    if ((ErrorCustomPage)HttpContext.Current.Handler != null)
                    {
                        ((ErrorCustomPage)HttpContext.Current.Handler).Error = "DataAccess Layer Exception: " + exception.Message;
                    }
                }
                else
                {
                    if ((ErrorCustomPage)HttpContext.Current.Handler != null)
                    {
                        HttpContext.Current.Server.Transfer("~/CriticalException.aspx", false);
                    }   
                }
            }
            catch(System.Threading.ThreadAbortException ex)
            {
                
            }
            catch (Exception ex)
            {
                
            }
            return exception;
        }
        
    }
}
