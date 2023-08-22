using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FrameWork.ExceptionHandling.CustomException
{
    [Serializable()]
    public class BussinessLayerException : BaseException, ISerializable
    {
        public BussinessLayerException() : base() 
        { 
        } 
        
        public BussinessLayerException(string message) : base(message) 
        { 
        } 
        
        public BussinessLayerException(string message, Exception exception) : base(message, exception) 
        { 
        }

        protected BussinessLayerException(SerializationInfo info, StreamingContext context) : base(info, context) 
        { 
        }
    }
}
