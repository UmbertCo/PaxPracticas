using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Canonicalizacion_Port
{
    interface NodeFilter
    {
        /**
    * Tells if a node must be output in c14n.
    * @param n
    * @return 1 if the node should be output.
    * 		   0 if node must not be output, 
    * 		  -1 if the node and all it's child must not be output.
    * 			
    */
        int isNodeInclude(XmlNode n);

        /**
         * Tells if a node must be output in a c14n.
         * The caller must assured that this method is always call
         * in document order. The implementations can use this 
         * restriction to optimize the transformation.
         * @param n
         * @param level the relative level in the tree
         * @return 1 if the node should be output.
         * 		   0 if node must not be output, 
         * 		  -1 if the node and all it's child must not be output.
         */
        int isNodeIncludeDO(XmlNode n, int level);
    }
}
