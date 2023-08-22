using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FrameWork.ExceptionHandling.CustomException
{
    [Serializable()]
    public class ServiceLayerException : ApplicationException
    {
        public ServiceLayerException() : base() 
        { 
        } 
        
        public ServiceLayerException(string message) : base(message) 
        { 
        } 
        
        public ServiceLayerException(string message, Exception exception) : base(message, exception) 
        { 
        }

        protected ServiceLayerException(SerializationInfo info, StreamingContext context) : base(info, context) 
        { 
        } 
    }
}
