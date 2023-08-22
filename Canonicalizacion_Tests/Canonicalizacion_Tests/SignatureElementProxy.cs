using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Canonicalizacion_Tests
{
    class SignatureElementProxy
    {
        //CONTIENE METODOS DE XMLUTILS Y DE ELEMENTPROXY

        private XmlDocument wrappedDoc = null;
        private XmlElement wrappedElement = null;
        protected String baseURI = null;
        private static volatile String dsPrefix = "ds";

        protected SignatureElementProxy() {
        }
    
        /**
         * Constructor SignatureElementProxy
         *
         * @param doc
         */
        public SignatureElementProxy(XmlDocument doc) {
            if (doc == null) {
                throw new Exception("Document is null");
            }

            setDocument(doc);
            setElement(createElementInSignatureSpace(doc,this.getBaseLocalName()));
        }

        /**
         * Constructor SignatureElementProxy
         *
         * @param element
         * @param baseURI
         * @throws XMLSecurityException
         */
        public SignatureElementProxy(XmlElement element, String baseURI)
        {
            ElementProxy(element, baseURI);
        }


        public void ElementProxy(XmlElement element, String baseURI)
        {
            if (element == null) 
            {
                throw new Exception("ElementProxy.nullElement");
            }

            /*if (log.isDebugEnabled()) 
            {
                //TAGNAME
                log.debug("setElement(\"" + element.Name + "\", \"" + baseURI + "\")");
            }*/

            setElement(element);
            this.baseURI = baseURI;

            this.guaranteeThatElementInCorrectSpace();
        }

        /** @inheritDoc */
        public String getBaseNamespace() 
        {
            return Constants.SignatureSpecNS;
        }

        protected void setDocument(XmlDocument doc) 
        {
            wrappedDoc = doc;
        }

        protected void setElement(XmlElement elem) 
        {
            wrappedElement = elem;
        }

        /**
         * Get the local name of this element
         *
         * @return Constants._TAG_SIGNATURE
         */
        public abstract String getBaseLocalName();

        /**
         * Method guaranteeThatElementInCorrectSpace
         *
         * @throws XMLSecurityException
         */
        public void guaranteeThatElementInCorrectSpace()
        {
            String expectedLocalName = this.getBaseLocalName();
            String expectedNamespaceUri = this.getBaseNamespace();

            String actualLocalName = wrappedElement.LocalName;
            String actualNamespaceUri = wrappedElement.NamespaceURI;

            if(!expectedNamespaceUri.Equals(actualNamespaceUri) && !expectedLocalName.Equals(actualLocalName)) 
            {      
                Object[] exArgs = { actualNamespaceUri + ":" + actualLocalName, expectedNamespaceUri + ":" + expectedLocalName};
                throw new Exception("xml.WrongElement "+ exArgs);
            }
        }

        /**
         * Creates an Element in the XML Signature specification namespace.
         *
         * @param doc the factory Document
         * @param elementName the local name of the Element
         * @return the Element
         */
        public static XmlElement createElementInSignatureSpace(XmlDocument doc, String elementName)
        {
            if (doc == null)
            {
                throw new Exception("Document is null");
            }

            if (dsPrefix == null || dsPrefix.Length == 0)
            {
                return doc.CreateElement(elementName, Constants.SignatureSpecNS);
            }
            return doc.CreateElement(dsPrefix + ":" + elementName, Constants.SignatureSpecNS);
        }
    }
}
