using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using C5;
using System.Collections.ObjectModel;

namespace Canonicalizacion_Port
{
    class Canonicalizacion
    {

        public const String XML = "xml";
        public const String XMLNS = "xmlns";
        protected bool reset = false;

        protected static AttrCompare COMPARE = new AttrCompare();

        // Make sure you clone the following mutable arrays before passing to
        // potentially untrusted objects such as OutputStreams.

        private List<NodeFilter> nodeFilter;

        private bool includeComments;
        private System.Collections.Generic.HashSet<XmlNode> xpathNodeSet;

        /**
         * The node to be skipped/excluded from the DOM tree 
         * in subtree canonicalizations.
         */
        private XmlNode excludeNode;
        private StreamWriter writer = new StreamWriter();
        //private MemoryStream writer = new MemoryStream();

        /**
         * The null xmlns definition.
         */
        private XmlAttribute nullNode;

        protected const int NODE_BEFORE_DOCUMENT_ELEMENT = -1;
        protected const int NODE_NOT_BEFORE_OR_AFTER_DOCUMENT_ELEMENT = 0;
        protected const int NODE_AFTER_DOCUMENT_ELEMENT = 1;

        ////////VARIABLES CANONICALIZACION 1.1/////////////////////////////////////

        /// <summary>
        /// Clase canonicalizadora de C14N11
        /// </summary>
        //SE USA libreria C5 para emular en TreeSet

        private const String XMLNS_URI = Constants.NamespaceSpecNS;
        private const String XML_LANG_URI = Constants.XML_LANG_SPACE_SpecNS;
        /*private static org.slf4j.Logger log = 
            org.slf4j.LoggerFactory.getLogger(Canonicalizer11.class);*/

        // LINEA ORIGINAL: private final SortedSet<Attr> result = new TreeSet<Attr>(COMPARE); ///
        private readonly TreeSet<XmlAttribute> result = new TreeSet<XmlAttribute>(COMPARE);

        private bool firstCall = true;

        //Se movio la clase XmlAttrStack a otro archivo

        //NO SE PUEDE DECLARAR PORQUE ES ESTATICA... WTF JAVA
        //private XmlAttrStack xmlattrStack = new XmlAttrStack();
        private XmlAttrStack xmlattrStack;

        private bool comments = false;

        public bool Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        ////////////////////////////////////
        //Funcion principal para canonicalizar
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
            catch (Exception ex)
            {

            }
            return this.engineCanonicalizeSubTree(xmlDoc, null);
        }


        /**
         * Canonicalizes a Subtree node.
         * 
         * @param rootNode
         *            the root of the subtree to canonicalize
         * @param excludeNode
         *            a node to be excluded from the canonicalize operation
         * @return The canonicalize stream.
         * @throws CanonicalizationException
         */
        protected byte[] engineCanonicalizeSubTree(XmlNode rootNode, XmlNode excludeNode)
        {
            this.excludeNode = excludeNode;
            try
            {

                NameSpaceSymbTable ns = new NameSpaceSymbTable();

                int nodeLevel = NODE_BEFORE_DOCUMENT_ELEMENT;

                if (rootNode != null && typeof(XmlElement) == rootNode.GetType())
                {
                    //Fills the nssymbtable with the definitions of the parent of the root subnode
                    getParentNameSpaces((XmlElement)rootNode, ns);
                    nodeLevel = NODE_NOT_BEFORE_OR_AFTER_DOCUMENT_ELEMENT;
                }
                this.canonicalizeSubTree(rootNode, ns, rootNode, nodeLevel);
                this.writer.Flush();
                //if (this.writer instanceof ByteArrayOutputStream) {
                if (typeof(StreamWriter) == this.writer.GetType() || typeof(MemoryStream) == this.writer.GetType())
                {
                    string sStreamResult = string.Empty;
                    writer.Write(sStreamResult);

                    byte[] result = System.Text.Encoding.BigEndianUnicode.GetBytes(sStreamResult);
                    if (reset)
                    {
                        //((ByteArrayOutputStream)this.writer).reset();
                        this.writer.Flush();
                    }
                    else
                    {
                        this.writer.Close();
                    }
                    return result;
                }//Se removio el else if.
                else
                {
                    this.writer.Close();
                }
                return null;

            }
            catch (Exception ex)
            {
                //throw new CanonicalizationException(ex);
                return null;
            }
        }

        /**
         * Method canonicalizeSubTree, this function is a recursive one.
         *    
         * @param currentNode
         * @param ns 
         * @param endnode 
         * @throws CanonicalizationException
         * @throws IOException
         */
        protected void canonicalizeSubTree(XmlNode currentNode, NameSpaceSymbTable ns, XmlNode endnode, int documentLevel)
        {
            if (currentNode == null || isVisibleInt(currentNode) == -1)
            {
                return;
            }
            XmlNode sibling = null;
            XmlNode parentNode = null;
            StreamWriter writer = this.writer;
            XmlNode excludeNode = this.excludeNode;
            bool includeComments = this.includeComments;
            Dictionary<String, byte[]> cache = new Dictionary<String, byte[]>();

            do
            {
                switch (currentNode.NodeType)
                {

                    case XmlNodeType.Entity:
                    case XmlNodeType.Notation:
                    case XmlNodeType.Attribute:
                        // illegal node type during traversal
                        throw new Exception("empty illegal node type during traversal");

                    case XmlNodeType.DocumentFragment:
                    case XmlNodeType.Document:
                        ns.outputNodePush();
                        sibling = currentNode.FirstChild;
                        break;

                    case XmlNodeType.Comment:
                        if (includeComments)
                        {
                            Uft8Helper.outputCommentToWriter((XmlComment)currentNode, writer, documentLevel);
                        }
                        break;

                    case XmlNodeType.ProcessingInstruction:
                        Uft8Helper.outputPItoWriter((XmlProcessingInstruction)currentNode, writer, documentLevel);
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                        Uft8Helper.outputTextToWriter(currentNode.Value, writer);
                        break;

                    case XmlNodeType.Element:
                        documentLevel = NODE_NOT_BEFORE_OR_AFTER_DOCUMENT_ELEMENT;
                        if (currentNode == excludeNode)
                        {
                            break;
                        }
                        XmlElement currentElement = (XmlElement)currentNode;
                        //Add a level to the nssymbtable. So latter can be pop-back.
                        ns.outputNodePush();
                        writer.Write('<');
                        String name = currentElement.Name;
                        writeByte(name, writer, cache);

                        IEnumerator<XmlAttribute> attrs = this.handleAttributesSubtree(currentElement, ns);
                        if (attrs != null)
                        {
                            //we output all Attrs which are available
                            while (attrs.MoveNext())
                            {
                                XmlAttribute attr = attrs.Current;
                                Uft8Helper.outputAttrToWriter(attr.Name, attr.Value, writer, cache);
                            }
                        }
                        writer.Write('>');
                        sibling = currentNode.FirstChild;
                        if (sibling == null)
                        {
                            writer.Write(END_TAG.Clone());
                            Uft8Helper.writeStringToUtf8(name, writer);
                            writer.Write('>');
                            //We finished with this level, pop to the previous definitions.
                            ns.outputNodePop();
                            if (parentNode != null)
                            {
                                sibling = currentNode.NextSibling;
                            }
                        }
                        else
                        {
                            parentNode = currentElement;
                        }
                        break;

                    case XmlNodeType.DocumentType:
                    default:
                        break;
                }
                while (sibling == null && parentNode != null)
                {
                    writer.Write(END_TAG.Clone());
                    writeByte(((XmlElement)parentNode).Name, writer, cache);
                    writer.Write('>');
                    //We finished with this level, pop to the previous definitions.
                    ns.outputNodePop();
                    if (parentNode == endnode)
                    {
                        return;
                    }
                    sibling = parentNode.NextSibling;
                    parentNode = parentNode.ParentNode;
                    if (parentNode == null || XmlNodeType.Element != parentNode.NodeType)
                    {
                        documentLevel = NODE_AFTER_DOCUMENT_ELEMENT;
                        parentNode = null;
                    }
                }
                if (sibling == null)
                {
                    return;
                }
                currentNode = sibling;
                sibling = currentNode.NextSibling;
            } while (true);
        }

        /**
         * Adds to ns the definitions from the parent elements of el
         * @param el
         * @param ns
         */
        protected void getParentNameSpaces(XmlElement el, NameSpaceSymbTable ns)
        {
            XmlNode n1 = el.ParentNode;
            if (n1 == null || XmlNodeType.Element != n1.NodeType)
            {
                return;
            }
            //Obtain all the parents of the element
            List<XmlElement> parents = new List<XmlElement>();
            XmlNode parent = n1;
            while (parent != null && XmlNodeType.Element == parent.NodeType)
            {
                parents.Add((XmlElement)parent);
                parent = parent.ParentNode;
            }
            //Visit them in reverse order.
            /*IEnumerator<XmlElement> it = parents.GetEnumerator();
            while (it.hasPrevious()) {
                Element ele = it.previous();
                handleParent(ele, ns);
            }
            */
            parents.Reverse();
            foreach (XmlElement ele in parents)
            {
                handleParent(ele, ns);
            }


            parents.Clear();
            XmlAttribute nsprefix = ns.getMappingWithoutRendered(XMLNS);
            if (nsprefix != null && "".Equals(nsprefix.Value))
            {
                ns.addMappingAndRender(
                        XMLNS, "", getNullNode(nsprefix.OwnerDocument));
            }
        }

        // The null xmlns definition.
        protected XmlAttribute getNullNode(XmlDocument ownerDocument)
        {
            if (nullNode == null)
            {
                try
                {
                    nullNode = ownerDocument.CreateAttribute(Constants.NamespaceSpecNS, XMLNS);
                    nullNode.Value = "";
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to create nullNode: " + e);
                }
            }
            return nullNode;
        }

        protected int isVisibleInt(XmlNode currentNode)
        {
            if (nodeFilter != null)
            {
                IEnumerator<NodeFilter> it = nodeFilter.GetEnumerator();
                while (it.MoveNext())
                {
                    int i = it.Current.isNodeInclude(currentNode);
                    if (i != 1)
                    {
                        return i;
                    }
                }
            }
            if (this.xpathNodeSet != null && !this.xpathNodeSet.Contains(currentNode))
            {
                return 0;
            }
            return 1;
        }

        protected void handleParent(XmlElement e, NameSpaceSymbTable ns)
        {
            if (!e.HasAttributes && e.NamespaceURI == null)
            {
                return;
            }
            XmlNamedNodeMap attrs = e.Attributes;
            int attrsLength = attrs.Count;
            for (int i = 0; i < attrsLength; i++)
            {
                XmlAttribute attribute = (XmlAttribute)attrs.Item(i);
                String NName = attribute.LocalName;
                String NValue = attribute.Value;

                if (Constants.NamespaceSpecNS.Equals(attribute.NamespaceURI)
                    && (!XML.Equals(NName) || !Constants.XML_LANG_SPACE_SpecNS.Equals(NValue)))
                {
                    ns.addMapping(NName, NValue, attribute);
                }
            }
            if (e.NamespaceURI != null)
            {
                String NName = e.Prefix;
                String NValue = e.NamespaceURI;
                String Name;
                if (NName == null || NName.Equals(""))
                {
                    NName = XMLNS;
                    Name = XMLNS;
                }
                else
                {
                    Name = XMLNS + ":" + NName;
                }
                XmlAttribute n = e.OwnerDocument.CreateAttribute("http://www.w3.org/2000/xmlns/", Name);
                n.Value = NValue;
                ns.addMapping(NName, NValue, n);
            }
        }

        public static void writeByte(String str, StreamWriter swOut, Dictionary<String, byte[]> cache)
        {
            //Se cambia el get por el valor del nombre del XmlElement
            byte[] result = cache[str];
            if (result == null)
            {
                result = Uft8Helper.getStringInUtf8(str);
                cache.Add(str, result);
            }

            swOut.Write(result);
        }



        /////////////////////////////C14N11/////////////////////////////////////

        /**
         * Returns the Attr[]s to be output for the given element.
         * <br>
         * The code of this method is a copy of {@link #handleAttributes(Element,
         * NameSpaceSymbTable)},
         * whereas it takes into account that subtree-c14n is -- well -- 
         * subtree-based.
         * So if the element in question isRoot of c14n, it's parent is not in the
         * node set, as well as all other ancestors.
         *
         * @param element
         * @param ns
         * @return the Attr[]s to be output
         * @throws CanonicalizationException
         */
        public IEnumerator<XmlAttribute> handleAttributesSubtree(XmlElement element, NameSpaceSymbTable ns)
        {
            if (!element.HasAttributes && !firstCall)
            {
                return null;
            }
            // result will contain the attrs which have to be output
            TreeSet<XmlAttribute> result = this.result;
            result.Clear();

            if (element.HasAttributes)
            {
                XmlNamedNodeMap attrs = element.Attributes;
                int attrsLength = attrs.Count;

                for (int i = 0; i < attrsLength; i++)
                {
                    XmlAttribute attribute = (XmlAttribute)attrs.Item(i);
                    String NUri = attribute.NamespaceURI;
                    String NName = attribute.LocalName;
                    String NValue = attribute.Value;

                    if (!XMLNS_URI.Equals(NUri))
                    {
                        // It's not a namespace attr node. Add to the result and continue.
                        result.Add(attribute);
                    }
                    else if (!(XML.Equals(NName) && XML_LANG_URI.Equals(NValue)))
                    {
                        // The default mapping for xml must not be output.
                        XmlNode n = ns.addMappingAndRender(NName, NValue, attribute);

                        if (n != null)
                        {
                            // Render the ns definition
                            result.Add((XmlAttribute)n);
                            if (C14nHelper.namespaceIsRelative(attribute))
                            {
                                Object[] exArgs = { element.Name, NName, attribute.Value };
                                throw new Exception("c14n.Canonicalizer.RelativeNamespace" + exArgs);
                            }
                        }
                    }
                }
            }

            if (firstCall)
            {

                //REQUIEREN COLLECTION NO TREES?!?! 
                //SE AGREGA COLECCION NUEVA:
                Collection<XmlAttribute> col1 = new Collection<XmlAttribute>();

                foreach (XmlAttribute attr in result)
                {
                    col1.Add(attr);
                }
                //COLECCION NUEVA///////////////////

                // It is the first node of the subtree
                // Obtain all the namespaces defined in the parents, and added to the output.
                ns.getUnrenderedNodes(col1);
                // output the attributes in the xml namespace.
                xmlattrStack.getXmlnsAttr(col1);

                firstCall = false;
            }

            return result.GetEnumerator();
        }
        
    }
}
