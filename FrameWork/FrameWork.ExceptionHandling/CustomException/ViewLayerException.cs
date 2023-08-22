using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FrameWork.ExceptionHandling.CustomException
{
    [Serializable()]
    class ViewLayerException : ApplicationException
    {
        public ViewLayerException() : base() 
        { 
        } 
        
        public ViewLayerException(string message) : base(message) 
        { 
        } 
        
        public ViewLayerException(string message, Exception exception) : base(message, exception) 
        { 
        }

        protected ViewLayerException(SerializationInfo info, StreamingContext context) : base(info, context) 
        { 
        } 
    }
}
