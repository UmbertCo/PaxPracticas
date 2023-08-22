using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Canonicalizacion_Port
{
    abstract class CanonicalizerSpi
    {
         /** Reset the writer after a c14n */
        internal protected bool reset = false;
        internal protected bool secureValidation;
    
        /**
         * Method canonicalize
         *
         * @param inputBytes
         * @return the c14n bytes. 
         *
         * @throws CanonicalizationException
         * @throws java.io.IOException
         * @throws javax.xml.parsers.ParserConfigurationException
         * @throws org.xml.sax.SAXException
         */
        public byte[] engineCanonicalize(byte[] inputBytes)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try {
                
                string xml = Encoding.UTF8.GetString(inputBytes);
                xmlDoc.LoadXml(xml);
                XmlDocumentType XDType = xmlDoc.DocumentType;
                xmlDoc.RemoveChild(XDType);

            } finally {

            }
            return this.engineCanonicalizeSubTree(xmlDoc);
        }

        /**
         * Method engineCanonicalizeXPathNodeSet
         *
         * @param xpathNodeSet
         * @return the c14n bytes
         * @throws CanonicalizationException
         */
        public byte[] engineCanonicalizeXPathNodeSet(XmlNodeList xpathNodeSet)
        {
            HashSet<XmlNode> Set = new HashSet<XmlNode>();
            
            foreach (XmlNode Node in xpathNodeSet)
	        {
		        Set.Add(Node);
	        }
            return this.engineCanonicalizeXPathNodeSet(Set);
        }

        /**
         * Method engineCanonicalizeXPathNodeSet
         *
         * @param xpathNodeSet
         * @param inclusiveNamespaces
         * @return the c14n bytes
         * @throws CanonicalizationException
         */
        public byte[] engineCanonicalizeXPathNodeSet(XmlNodeList xpathNodeSet, String inclusiveNamespaces)
        {
            HashSet<XmlNode> Set = new HashSet<XmlNode>();
            
            foreach (XmlNode Node in xpathNodeSet)
	        {
		        Set.Add(Node);
	        }
            return this.engineCanonicalizeXPathNodeSet(Set, inclusiveNamespaces);
        }

        /** 
         * Returns the URI of this engine.
         * @return the URI
         */
        public abstract String engineGetURI();

        /**
         * Returns true if comments are included
         * @return true if comments are included
         */
        public abstract bool engineGetIncludeComments();

        /**
         * C14n a nodeset
         *
         * @param xpathNodeSet
         * @return the c14n bytes
         * @throws CanonicalizationException
         */
        public abstract byte[] engineCanonicalizeXPathNodeSet(HashSet<XmlNode> xpathNodeSet);

        /**
         * C14n a nodeset
         *
         * @param xpathNodeSet
         * @param inclusiveNamespaces
         * @return the c14n bytes
         * @throws CanonicalizationException
         */
        public abstract byte[] engineCanonicalizeXPathNodeSet(HashSet<XmlNode> xpathNodeSet, String inclusiveNamespaces);

        /**
         * C14n a node tree.
         *
         * @param rootNode
         * @return the c14n bytes
         * @throws CanonicalizationException
         */
        public abstract byte[] engineCanonicalizeSubTree(XmlNode rootNode);

        /**
         * C14n a node tree.
         *
         * @param rootNode
         * @param inclusiveNamespaces
         * @return the c14n bytes
         * @throws CanonicalizationException
         */
        public abstract byte[] engineCanonicalizeSubTree(XmlNode rootNode, String inclusiveNamespaces);

        /**
         * C14n a node tree.
         *
         * @param rootNode
         * @param inclusiveNamespaces
         * @param propagateDefaultNamespace If true the default namespace will be propagated to the c14n-ized root element
         * @return the c14n bytes
         * @throws CanonicalizationException
         */
        public abstract byte[] engineCanonicalizeSubTree(
                XmlNode rootNode, String inclusiveNamespaces, bool propagateDefaultNamespace);

        /**
         * Sets the writer where the canonicalization ends. ByteArrayOutputStream if 
         * none is set.
         * @param os
         */
        //public abstract void setWriter(byte[] byOut);
        public abstract void setWriter(StreamWriter strwOS);

        public bool isSecureValidation() {
            return secureValidation;
        }

        public void setSecureValidation(bool secureValidation) {
            this.secureValidation = secureValidation;
        }

    }
}
