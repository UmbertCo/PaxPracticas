using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Canonicalizacion_Tests
{
    class C14nHelper
    {
        /**
         * Constructor C14nHelper
         *
         */
        private C14nHelper() 
        {
            // don't allow instantiation
        }

        /**
         * Method namespaceIsRelative
         *
         * @param namespace
         * @return true if the given namespace is relative. 
         */
        public static bool namespaceIsRelative(XmlAttribute namespaceE) 
        {
            return !namespaceIsAbsolute(namespaceE);
        }

        /**
         * Method namespaceIsRelative
         *
         * @param namespaceValue
         * @return true if the given namespace is relative.
         */
        public static bool namespaceIsRelative(String namespaceValue) 
        {
            return !namespaceIsAbsolute(namespaceValue);
        }

        /**
         * Method namespaceIsAbsolute
         *
         * @param namespace
         * @return true if the given namespace is absolute.
         */
        public static bool namespaceIsAbsolute(XmlAttribute namespaceE) 
        {
            return namespaceIsAbsolute(namespaceE.Value);
        }

        /**
         * Method namespaceIsAbsolute
         *
         * @param namespaceValue
         * @return true if the given namespace is absolute.
         */
        public static bool namespaceIsAbsolute(String namespaceValue) 
        {
            // assume empty namespaces are absolute
            if (namespaceValue.Length == 0) 
            {
                return true;
            }
            return namespaceValue.IndexOf(':') > 0;
        }

        /**
         * This method throws an exception if the Attribute value contains
         * a relative URI.
         *
         * @param attr
         * @throws CanonicalizationException
         */
        public static void assertNotRelativeNS(XmlAttribute attr)
        {
            if (attr == null) 
            {
                return;
            }
            //REVISAR attr.getNodeName ///
            String nodeAttrName = attr.Name;
            bool definesDefaultNS = nodeAttrName.Equals("xmlns");
            bool definesNonDefaultNS = nodeAttrName.StartsWith("xmlns:");

            if ((definesDefaultNS || definesNonDefaultNS) && namespaceIsRelative(attr)) 
            {
                String parentName = attr.OwnerElement.Name;
                String attrValue = attr.Value;
                Object[] exArgs = { parentName, nodeAttrName, attrValue };

                throw new Exception("c14n.Canonicalizer.RelativeNamespace " + exArgs);
            }
        }

        /**
         * This method throws a CanonicalizationException if the supplied Document
         * is not able to be traversed using a TreeWalker.
         *
         * @param document
         * @throws CanonicalizationException
         */
        public static void checkTraversability(XmlDocument document)
        {
            if (!document.Supports("Traversal", "2.0")) 
            {
                Object[] exArgs = { document.Implementation.ToString() };

                throw new Exception("c14n.Canonicalizer.TraversalNotSupported " + exArgs);
            }
        }

        /**
         * This method throws a CanonicalizationException if the supplied Element
         * contains any relative namespaces.
         *
         * @param ctxNode
         * @throws CanonicalizationException
         * @see C14nHelper#assertNotRelativeNS(Attr)
         */
        public static void checkForRelativeNamespace(XmlElement ctxNode)
        {
            if (ctxNode != null) 
            {
                XmlNamedNodeMap attributes = ctxNode.Attributes;

                for (int i = 0; i < attributes.Count; i++) 
                {
                    C14nHelper.assertNotRelativeNS((XmlAttribute) attributes.Item(i));
                }
            } 
            else 
            {
                throw new Exception("Called checkForRelativeNamespace() on null");
            }
        }
    }
}
