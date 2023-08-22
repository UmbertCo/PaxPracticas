using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Canonicalizacion_Port
{
    class Canonicalizer
    {
        /** The output encoding of canonicalized data */
        public  const String ENCODING = "UTF8";

        /**
         * XPath Expression for selecting every node and continuous comments joined 
         * in only one node 
         */
        public  const String XPATH_C14N_WITH_COMMENTS_SINGLE_NODE = 
            "(.//. | .//@* | .//namespace::*)";

        /**
         * The URL defined in XML-SEC Rec for inclusive c14n <b>without</b> comments.
         */
        public  const String ALGO_ID_C14N_OMIT_COMMENTS = 
            "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
        /**
         * The URL defined in XML-SEC Rec for inclusive c14n <b>with</b> comments.
         */
        public  const String ALGO_ID_C14N_WITH_COMMENTS = 
            ALGO_ID_C14N_OMIT_COMMENTS + "#WithComments";
        /**
         * The URL defined in XML-SEC Rec for exclusive c14n <b>without</b> comments.
         */
        public  const String ALGO_ID_C14N_EXCL_OMIT_COMMENTS = 
            "http://www.w3.org/2001/10/xml-exc-c14n#";
        /**
         * The URL defined in XML-SEC Rec for exclusive c14n <b>with</b> comments.
         */
        public  const String ALGO_ID_C14N_EXCL_WITH_COMMENTS = 
            ALGO_ID_C14N_EXCL_OMIT_COMMENTS + "WithComments";
        /**
         * The URI for inclusive c14n 1.1 <b>without</b> comments.
         */
        public  const String ALGO_ID_C14N11_OMIT_COMMENTS = 
            "http://www.w3.org/2006/12/xml-c14n11";
        /**
         * The URI for inclusive c14n 1.1 <b>with</b> comments.
         */
        public  const String ALGO_ID_C14N11_WITH_COMMENTS = 
            ALGO_ID_C14N11_OMIT_COMMENTS + "#WithComments";
        /**
         * Non-standard algorithm to serialize the physical representation for XML Encryption
         */
        public  const String ALGO_ID_C14N_PHYSICAL = 
            "http://santuario.apache.org/c14n/physical";

       /* private  Map<String, Class<? extends CanonicalizerSpi>> canonicalizerHash = 
            new ConcurrentHashMap<String, Class<? extends CanonicalizerSpi>>(); */

        private  Dictionary<String, CanonicalizerSpi> canonicalizerHash = 
            new Dictionary<String, CanonicalizerSpi>();
    
        private CanonicalizerSpi canonicalizerSpi;
        private bool secureValidation;

        /**
         * Constructor Canonicalizer
         *
         * @param algorithmURI
         * @throws InvalidCanonicalizerException
         */
        private Canonicalizer(String algorithmURI)
        {
            try 
            {

                /*Class<? extends CanonicalizerSpi> implementingClass = 
                    canonicalizerHash.get(algorithmURI);*/

                CanonicalizerSpi implementingClass = 
                    canonicalizerHash[algorithmURI];

                //canonicalizerSpi = implementingClass.newInstance();
                canonicalizerSpi = implementingClass;
                canonicalizerSpi.reset = true;
            } 
            catch (Exception ex) 
            {
                Object[] exArgs = { algorithmURI };
                throw new Exception("signature.Canonicalizer.UnknownCanonicalizer " + ex);
            }
        }
    
        /**
         * Method getInstance
         *
         * @param algorithmURI
         * @return a Canonicalizer instance ready for the job
         * @throws InvalidCanonicalizerException
         */
        public static Canonicalizer getInstance(String algorithmURI)
        {
            return new Canonicalizer(algorithmURI);
        }


        /**
         * This method tries to canonicalize the given bytes. It's possible to even
         * canonicalize non-wellformed sequences if they are well-formed after being
         * wrapped with a <CODE>&gt;a&lt;...&gt;/a&lt;</CODE>.
         *
         * @param inputBytes
         * @return the result of the canonicalization.
         * @throws CanonicalizationException
         * @throws java.io.IOException
         * @throws javax.xml.parsers.ParserConfigurationException
         * @throws org.xml.sax.SAXException
         */
        public byte[] canonicalize(byte[] inputBytes)
        {
            /*
             * for some of the test vectors from the specification,
             * there has to be a validating parser for ID attributes, default
             * attribute values, NMTOKENS, etc.
             * Unfortunately, the test vectors do use different DTDs or
             * even no DTD. So Xerces 1.3.1 fires many warnings about using
             * ErrorHandlers.
             *
             * Text from the spec:
             *
             * The input octet stream MUST contain a well-formed XML document,
             * but the input need not be validated. However, the attribute
             * value normalization and entity reference resolution MUST be
             * performed in accordance with the behaviors of a validating
             * XML processor. As well, nodes for default attributes (declared
             * in the ATTLIST with an AttValue but not specified) are created
             * in each element. Thus, the declarations in the document type
             * declaration are used to help create the canonical form, even
             * though the document type declaration is not retained in the
             * canonical form.
             */
            XmlDocument xmlDoc = new XmlDocument();
            try 
            {
                string xml = Encoding.UTF8.GetString(inputBytes);
                xmlDoc.LoadXml(xml);
            }
            catch(Exception ex)
            {
                
            }
            return this.canonicalizeSubtree(xmlDoc);
        }

        /**
         * Method register
         *
         * @param algorithmURI
         * @param implementingClass
         * @throws AlgorithmAlreadyRegisteredException
         * @throws SecurityException if a security manager is installed and the
         *    caller does not have permission to register the canonicalizer
         */
        public static void register(String algorithmURI, String implementingClass)
        {
            try
            {
                CanonicalizerSpi registeredClass = canonicalizerHash[algorithmURI];

                if (registeredClass != null)
                {
                    Object[] exArgs = { algorithmURI, registeredClass };
                    throw new Exception("algorithm.alreadyRegistered" + exArgs);
                }

                CanonicalizerSpi canonicalizerSpiDummy = new DummyClass();
                canonicalizerHash.Add(algorithmURI, canonicalizerSpiDummy);

            }
            catch (Exception ex)
            {
                //LOG MENSAJE
            }



            
            /*canonicalizerHash.put(
                algorithmURI, (Class<? extends CanonicalizerSpi>)
                ClassLoaderUtils.loadClass(implementingClass, Canonicalizer.class)
            );*/
        }


        /**
         * Method register
         *
         * @param algorithmURI
         * @param implementingClass
         * @throws AlgorithmAlreadyRegisteredException
         * @throws SecurityException if a security manager is installed and the
         *    caller does not have permission to register the canonicalizer
         */
        public static void register(String algorithmURI, CanonicalizerSpi implementingClass)
        {
            try
            {
                // check whether URI is already registered
                CanonicalizerSpi registeredClass = canonicalizerHash[algorithmURI];

                if (registeredClass != null)
                {
                    Object[] exArgs = { algorithmURI, registeredClass };
                    throw new Exception("algorithm.alreadyRegistered " + exArgs);
                }

                canonicalizerHash.Add(algorithmURI, implementingClass);
            }
            catch (Exception ex)
            {
                //MENSAJE
            }
        }



        /**
         * This method registers the default algorithms.
         */
        public static void registerDefaultAlgorithms() 
        {
            /*canonicalizerHash.Add(
                Canonicalizer.ALGO_ID_C14N_OMIT_COMMENTS, 
                Canonicalizer20010315OmitComments
            );
            canonicalizerHash.put(
                Canonicalizer.ALGO_ID_C14N_WITH_COMMENTS, 
                Canonicalizer20010315WithComments
            );
            canonicalizerHash.put(
                Canonicalizer.ALGO_ID_C14N_EXCL_OMIT_COMMENTS, 
                Canonicalizer20010315ExclOmitComments
            );
            canonicalizerHash.put(
                Canonicalizer.ALGO_ID_C14N_EXCL_WITH_COMMENTS, 
                Canonicalizer20010315ExclWithComments
            );*/
            Canonicalizer11_WithComments C14N11_WC = new Canonicalizer11_WithComments();
            Canonicalizer11_OmitComments C14N11_OC = new Canonicalizer11_OmitComments();
            CanonicalizerPhysical C14NP = new CanonicalizerPhysical();
            canonicalizerHash.Add(
                Canonicalizer.ALGO_ID_C14N11_OMIT_COMMENTS, 
                C14N11_OC
            );
            canonicalizerHash.Add(
                Canonicalizer.ALGO_ID_C14N11_WITH_COMMENTS, 
                C14N11_WC
            );
            canonicalizerHash.Add(
                Canonicalizer.ALGO_ID_C14N_PHYSICAL, 
                C14NP
            );
        }

        /**
         * Method getURI
         *
         * @return the URI defined for this c14n instance.
         */
        public String getURI() {
            return canonicalizerSpi.engineGetURI();
        }


        /**
         * Method getIncludeComments
         *
         * @return true if the c14n respect the comments.
         */
         public bool getIncludeComments()
         {
             return canonicalizerSpi.engineGetIncludeComments();
         }

        /**
         * Canonicalizes the subtree rooted by <CODE>node</CODE>.
         *
         * @param node The node to canonicalize
         * @return the result of the c14n.
         *
         * @throws CanonicalizationException
         */
         public byte[] canonicalizeSubtree(XmlNode node)
         {
            canonicalizerSpi.secureValidation = secureValidation;
            return canonicalizerSpi.engineCanonicalizeSubTree(node);
         }


        /**
         * Canonicalizes the subtree rooted by <CODE>node</CODE>.
         *
         * @param node
         * @param inclusiveNamespaces
         * @return the result of the c14n.
         * @throws CanonicalizationException
         */
        public byte[] canonicalizeSubtree(XmlNode node, String inclusiveNamespaces)
        {
            canonicalizerSpi.secureValidation = secureValidation;
            return canonicalizerSpi.engineCanonicalizeSubTree(node, inclusiveNamespaces);
        }


        /**
         * Canonicalizes the subtree rooted by <CODE>node</CODE>.
         *
         * @param node
         * @param inclusiveNamespaces
         * @return the result of the c14n.
         * @throws CanonicalizationException
         */
        public byte[] canonicalizeSubtree(XmlNode node, String inclusiveNamespaces, bool propagateDefaultNamespace)
        {
            canonicalizerSpi.secureValidation = secureValidation;
            return canonicalizerSpi.engineCanonicalizeSubTree(node, inclusiveNamespaces, propagateDefaultNamespace);
        }

        /**
         * Canonicalizes an XPath node set. The <CODE>xpathNodeSet</CODE> is treated
         * as a list of XPath nodes, not as a list of subtrees.
         *
         * @param xpathNodeSet
         * @return the result of the c14n.
         * @throws CanonicalizationException
         */
        public byte[] canonicalizeXPathNodeSet(XmlNodeList xpathNodeSet)
        {
            canonicalizerSpi.secureValidation = secureValidation;
            return canonicalizerSpi.engineCanonicalizeXPathNodeSet(xpathNodeSet);
        }


        /**
         * Canonicalizes an XPath node set. The <CODE>xpathNodeSet</CODE> is treated
         * as a list of XPath nodes, not as a list of subtrees.
         *
         * @param xpathNodeSet
         * @param inclusiveNamespaces
         * @return the result of the c14n.
         * @throws CanonicalizationException
         */
        public byte[] canonicalizeXPathNodeSet(XmlNodeList xpathNodeSet, String inclusiveNamespaces)
        {
            canonicalizerSpi.secureValidation = secureValidation;
            return canonicalizerSpi.engineCanonicalizeXPathNodeSet(xpathNodeSet, inclusiveNamespaces);
        }

        /**
         * Canonicalizes an XPath node set.
         *
         * @param xpathNodeSet
         * @return the result of the c14n.
         * @throws CanonicalizationException
         */
        public byte[] canonicalizeXPathNodeSet(HashSet<XmlNode> xpathNodeSet) 
        {
            canonicalizerSpi.secureValidation = secureValidation;
            return canonicalizerSpi.engineCanonicalizeXPathNodeSet(xpathNodeSet);
        }

        /**
         * Sets the writer where the canonicalization ends.  ByteArrayOutputStream 
         * if none is set.
         * @param os
         */
        public void setWriter(StreamWriter os)
        {
            canonicalizerSpi.setWriter(os);
        }

        /**
         * Returns the name of the implementing {@link CanonicalizerSpi} class
         *
         * @return the name of the implementing {@link CanonicalizerSpi} class
         */
        public String getImplementingCanonicalizerClass()
        {
            return typeof(CanonicalizerSpi).Name;
        }

        /**
         * Set the canonicalizer behaviour to not reset.
         */
        public void notReset()
        {
            canonicalizerSpi.reset = false;
        }

        public bool isSecureValidation()
        {
            return secureValidation;
        }

        public void setSecureValidation(bool secureValidation)
        {
            this.secureValidation = secureValidation;
        }
    
    }
}
