using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameWork.ExceptionHandling
{
    public static class BussinessExceptionHandler
    {
        public static bool HandleException(ref System.Exception ex)
        {
            bool rethrow = false;
            if (ex.GetType().Equals(typeof(CustomException.DataAccesLayerException)) || ex.GetType().Equals(typeof(CustomException.DataAccessLayerCustomException)))
            {
                rethrow = ExceptionPolicy.HandleException(ex, "PassThroughPolicy");
                ex = new CustomException.PassThroughException(ex.Message);
            }
            else if (ex is CustomException.BussinessLayerCustomException)
            {
                rethrow = ExceptionPolicy.HandleException(ex, "BussinessCustomPolicy");
            }
            else
            {
                rethrow = ExceptionPolicy.HandleException(ex, "BussinessPolicy");
            }
            if (rethrow)
            {
                throw ex;
            }
            return rethrow;
        }
    }
}
