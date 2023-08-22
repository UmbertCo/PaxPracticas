using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameWork.ExceptionHandling
{
    public abstract class ErrorCustomPage : System.Web.UI.Page
    {
        public abstract string Error { set; }
    }
}
