using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameWork.ExceptionHandling
{
    public class ExceptionHandlerProvider
    {
        public static bool HandlerException(Exception exception, string PolicyName)
        {
            bool reThrow = false;
            reThrow = ExceptionPolicy.HandleException(exception, PolicyName);
            return reThrow;
        }
    }
}
