using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C5;
using System.Xml;
using System.IO;

namespace Canonicalizacion_Port
{
    class CanonicalizerPhysical : CanonicalizerBase
    {
         private TreeSet<XmlAttribute> result = new TreeSet<XmlAttribute>(COMPARE);


       /* public CanonicalizerPhysical(bool includeComments) 
        {
            CanonicalizerBase.includeComments = includeComments;
        }*/

        public byte[] engineCanonicalizeXPathNodeSet(System.Collections.Generic.HashSet<XmlNode> xpathNodeSet, String inclusiveNamespaces)
        {

            /** $todo$ well, should we throw UnsupportedOperationException ? */
            throw new Exception("c14n.Canonicalizer.UnsupportedOperation");
        }

        /**
         * Always throws a CanonicalizationException.
         *
         * @param rootNode
         * @param inclusiveNamespaces
         * @return none it always fails
         * @throws CanonicalizationException
         */
        public byte[] engineCanonicalizeSubTree(XmlNode rootNode, String inclusiveNamespaces)
        {

            /** $todo$ well, should we throw UnsupportedOperationException ? */
            throw new Exception("c14n.Canonicalizer.UnsupportedOperation");
        }

        /**
         * Always throws a CanonicalizationException.
         *
         * @param rootNode
         * @param inclusiveNamespaces
         * @return none it always fails
         * @throws CanonicalizationException
         */
        public byte[] engineCanonicalizeSubTree(XmlNode rootNode, String inclusiveNamespaces, bool propagateDefaultNamespace)
        {

            /** $todo$ well, should we throw UnsupportedOperationException ? */
            throw new Exception("c14n.Canonicalizer.UnsupportedOperation");
        }

        /**
         * Returns the Attr[]s to be output for the given element.
         * <br>
         * The code of this method is a copy of {@link #handleAttributes(Element,
         * NameSpaceSymbTable)},
         * whereas it takes into account that subtree-c14n is -- well -- subtree-based.
         * So if the element in question isRoot of c14n, it's parent is not in the
         * node set, as well as all other ancestors.
         *
         * @param element
         * @param ns
         * @return the Attr[]s to be output
         * @throws CanonicalizationException
         */

        protected IEnumerator<XmlAttribute> handleAttributesSubtree(XmlElement element, NameSpaceSymbTable ns)
        {
            if (!element.HasAttributes) 
            {
                return null; 
            }

            // result will contain all the attrs declared directly on that element
            TreeSet<XmlAttribute> result = this.result;       
            result.Clear();

            if (element.HasAttributes) {
                XmlNamedNodeMap attrs = element.Attributes;
                int attrsLength = attrs.Count;      

                for (int i = 0; i < attrsLength; i++) {
                    XmlAttribute attribute = (XmlAttribute) attrs.Item(i);
                    result.Add(attribute);
                }
            }

            return result.GetEnumerator();
        }

        /**
         * Returns the Attr[]s to be output for the given element.
         * 
         * @param element
         * @param ns
         * @return the Attr[]s to be output
         * @throws CanonicalizationException
         */
        protected IEnumerator<XmlAttribute> handleAttributes(XmlElement element, NameSpaceSymbTable ns) 
        {    

            /** $todo$ well, should we throw UnsupportedOperationException ? */
            throw new Exception("c14n.Canonicalizer.UnsupportedOperation");
        }

        /** @inheritDoc */
        public String engineGetURI() 
        {
            return Canonicalizer.ALGO_ID_C14N_PHYSICAL;
        }

        /** @inheritDoc */
        public bool engineGetIncludeComments() {
            return true;
        }


        protected void outputPItoWriter(XmlProcessingInstruction currentPI, StreamWriter writer, int position) 
        {
            // Processing Instructions before or after the document element are not treated specially
            base.outputPItoWriter(currentPI, writer, NODE_NOT_BEFORE_OR_AFTER_DOCUMENT_ELEMENT);
        }


        protected void outputCommentToWriter(XmlComment currentComment, StreamWriter writer, int position)
        {
            // Comments before or after the document element are not treated specially
            base.outputCommentToWriter(currentComment, writer, NODE_NOT_BEFORE_OR_AFTER_DOCUMENT_ELEMENT);
        }
    }
}
