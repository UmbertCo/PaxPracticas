using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FrameWork.ExceptionHandling.CustomException
{
    [Serializable()]
    public class PresentationLayerException : ApplicationException
    {
        public PresentationLayerException() : base() 
        { 
        } 
        
        public PresentationLayerException(string message) : base(message) 
        { 
        } 
        
        public PresentationLayerException(string message, Exception exception) : base(message, exception) 
        { 
        }

        protected PresentationLayerException(SerializationInfo info, StreamingContext context) : base(info, context) 
        { 
        } 
    }
}
