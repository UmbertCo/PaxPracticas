using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FrameWork.ExceptionHandling.CustomException
{
    [Serializable()]
    public class CriticalException : ApplicationException
    {
        public CriticalException() : base() 
        { 
        } 
        
        public CriticalException(string message) : base(message) 
        { 
        } 
        
        public CriticalException(string message, Exception exception) : base(message, exception) 
        { 
        }

        protected CriticalException(SerializationInfo info, StreamingContext context) : base(info, context) 
        { 
        } 
    }
}
