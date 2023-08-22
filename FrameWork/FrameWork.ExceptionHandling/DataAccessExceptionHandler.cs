using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameWork.ExceptionHandling
{
    public static class DataAccessExceptionHandler
    {
        public static void HandleException(Exception ex)
        {
            bool reThrow = false;

            if (ex.GetType().Equals(typeof(CustomException.CriticalException)))
            {
                reThrow = ExceptionPolicy.HandleException(ex, "Propogate Policy");
                if (reThrow)
                    throw ex;
            }
            //else if (ex.GetType().Equals(typeof(CustomException.DataAccesLayerException)))
            //{
            //    reThrow = ExceptionPolicy.HandleException(ex, "Data Access Policy");
            //    if (reThrow)
            //        throw ex;
            //}
            else if (ex.GetType().Equals(typeof(CustomException.DataAccesLayerException)))
            {
                reThrow = ExceptionPolicy.HandleException(ex, "DataAccessPolicy");
                if (reThrow)
                    throw ex;
            }
            else
            {
                reThrow = ExceptionPolicy.HandleException(ex, "Propogate Policy");
                if (reThrow)
                    throw ex;
            }
            
        }
    }
}
