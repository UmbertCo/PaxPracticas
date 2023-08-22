using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FrameWork.ExceptionHandling.CustomException
{
    [Serializable()]
    public class BussinessLayerCustomException : BaseException, ISerializable
    {
        public BussinessLayerCustomException() : base() 
        { 
        } 
        
        public BussinessLayerCustomException(string message) : base(message) 
        { 
        } 
        
        public BussinessLayerCustomException(string message, Exception exception) : base(message, exception) 
        { 
        }

        protected BussinessLayerCustomException(SerializationInfo info, StreamingContext context) : base(info, context) 
        { 
        }
    }
}
