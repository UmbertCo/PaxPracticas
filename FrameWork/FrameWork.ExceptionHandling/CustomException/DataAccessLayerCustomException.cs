using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FrameWork.ExceptionHandling.CustomException
{
    [Serializable()]
    public class DataAccessLayerCustomException : BaseException, ISerializable
    {
        public DataAccessLayerCustomException() : base()
        { 
            //Add Implementation
        }

        public DataAccessLayerCustomException(string message) : base(message)
        { 
            //Add Implementation
        }

        public DataAccessLayerCustomException(string message, System.Exception inner) : base(message, inner)
        { 
            //Add Implementation
        }

        protected DataAccessLayerCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        { 
             //Add Implementation
        }
    }
}
