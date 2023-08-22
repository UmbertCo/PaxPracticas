using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Canonicalizacion_Port
{
    class AttrCompare : IComparer<XmlAttribute>
    {
        private const long serialVersionUID = -7113259629930576230L;
        private const int ATTR0_BEFORE_ATTR1 = -1;
        private const int ATTR1_BEFORE_ATTR0 = 1;
        private const String XMLNS = Constants.NamespaceSpecNS;

        /**
         * Compares two attributes based on the C14n specification.
         *
         * <UL>
         * <LI>Namespace nodes have a lesser document order position than 
         *   attribute nodes.
         * <LI> An element's namespace nodes are sorted lexicographically by
         *   local name (the default namespace node, if one exists, has no
         *   local name and is therefore lexicographically least).
         * <LI> An element's attribute nodes are sorted lexicographically with
         *   namespace URI as the primary key and local name as the secondary
         *   key (an empty namespace URI is lexicographically least).
         * </UL>
         *
         * @param attr0
         * @param attr1
         * @return returns a negative integer, zero, or a positive integer as 
         *   obj0 is less than, equal to, or greater than obj1
         *
         */
        public int Compare(XmlAttribute x, XmlAttribute y)
        {
            String namespaceURI0 = x.NamespaceURI;
            String namespaceURI1 = y.NamespaceURI;

            bool isNamespaceAttr0 = XMLNS.Equals(namespaceURI0);
            bool isNamespaceAttr1 = XMLNS.Equals(namespaceURI1);

            if (isNamespaceAttr0)
            {
                if (isNamespaceAttr1)
                {
                    // both are namespaces
                    String localname0 = x.LocalName;
                    String localname1 = y.LocalName;

                    if ("xmlns".Equals(localname0))
                    {
                        localname0 = "";
                    }

                    if ("xmlns".Equals(localname1))
                    {
                        localname1 = "";
                    }

                    return localname0.CompareTo(localname1);
                }
                // attr0 is a namespace, attr1 is not
                return ATTR0_BEFORE_ATTR1;
            }
            else if (isNamespaceAttr1)
            {
                // attr1 is a namespace, attr0 is not
                return ATTR1_BEFORE_ATTR0;
            }

            // none is a namespace
            if (namespaceURI0 == null)
            {
                if (namespaceURI1 == null)
                {
                    String name0 = x.Name;
                    String name1 = y.Name;
                    return name0.CompareTo(name1);
                }
                return ATTR0_BEFORE_ATTR1;
            }
            else if (namespaceURI1 == null)
            {
                return ATTR1_BEFORE_ATTR0;
            }

            int a = namespaceURI0.CompareTo(namespaceURI1);
            if (a != 0)
            {
                return a;
            }

            return x.LocalName.CompareTo(y.LocalName);
        }

        int IComparer<XmlAttribute>.Compare(XmlAttribute x, XmlAttribute y)
        {
            throw new NotImplementedException();
        }
    }
}
