using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FrameWork.ExceptionHandling.CustomException
{
    [Serializable()]
    public class DataAccesLayerException : BaseException, ISerializable
    {
        public DataAccesLayerException() : base() 
        { 
        } 
        
        public DataAccesLayerException(string message) : base(message) 
        {
           
        } 
        
        public DataAccesLayerException(string message, Exception exception) : base(message, exception) 
        {
            
        }

        protected DataAccesLayerException(SerializationInfo info, StreamingContext context) : base(info, context) 
        { 
        } 
    }
}
