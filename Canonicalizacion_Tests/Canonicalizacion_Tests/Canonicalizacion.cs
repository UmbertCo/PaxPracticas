using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.ObjectModel;
using C5;

namespace Canonicalizacion_Tests
{
    class Canonicalizacion
    {

        /**
         * The URI for inclusive c14n 1.1 <b>without</b> comments.
         */
        public static String ALGO_ID_C14N11_OMIT_COMMENTS = 
            "http://www.w3.org/2006/12/xml-c14n11";
        /**
         * The URI for inclusive c14n 1.1 <b>with</b> comments.
         */
        public static String ALGO_ID_C14N11_WITH_COMMENTS = 
            ALGO_ID_C14N11_OMIT_COMMENTS + "#WithComments";

        /**
         * The URL defined in XML-SEC Rec for inclusive c14n <b>without</b> comments.
         */
        public static String ALGO_ID_C14N_OMIT_COMMENTS = 
            "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
        /**
         * Non-standard algorithm to serialize the physical representation for XML Encryption
         */

        private static char[] cEND_PI = { '?', '>' };
        private static byte[] END_PI = Encoding.Unicode.GetBytes(cEND_PI);

        private static char[] cBEGIN_PI = { '<', '?' };
        private static byte[] BEGIN_PI = Encoding.Unicode.GetBytes(cBEGIN_PI);

        private static char[] cEND_COMM = { '-', '-', '>' };
        private static byte[] END_COMM = Encoding.Unicode.GetBytes(cEND_COMM);

        private static char[] cBEGIN_COMM = { '<', '!', '-', '-' };
        private static byte[] BEGIN_COMM = Encoding.Unicode.GetBytes(cBEGIN_COMM);

        private static char[] cXA = { '&', '#', 'x', 'A', ';' };
        private static byte[] XA = Encoding.Unicode.GetBytes(cXA);

        private static char[] cX9 = { '&', '#', 'x', '9', ';' };
        private static byte[] X9 = Encoding.Unicode.GetBytes(cX9);

        private static char[] cQUOT = { '&', 'q', 'u', 'o', 't', ';' };
        private static byte[] QUOT = Encoding.Unicode.GetBytes(cQUOT);

        private static char[] cXD = { '&', '#', 'x', 'D', ';' };
        private static byte[] XD = Encoding.Unicode.GetBytes(cXD);

        private static char[] cGT = { '&', 'g', 't', ';' };
        private static byte[] GT = Encoding.Unicode.GetBytes(cGT);

        private static char[] cLT = { '&', 'l', 't', ';' };
        private static byte[] LT = Encoding.Unicode.GetBytes(cLT);

        private static char[] cEND_TAG = { '<', '/' };
        private static byte[] END_TAG = Encoding.Unicode.GetBytes(cEND_TAG);

        private static char[] cAMP = { '&', 'a', 'm', 'p', ';' };
        private static byte[] AMP = Encoding.Unicode.GetBytes(cAMP);

        private static char[] cEQUALS_STR = { '=', '\"' };
        private static byte[] EQUALS_STR = Encoding.Unicode.GetBytes(cEQUALS_STR);

        public const String XML = "xml";
        public const String XMLNS = "xmlns";
        protected bool reset = false;

        protected static AttrCompare COMPARE = new AttrCompare();

        // Make sure you clone the following mutable arrays before passing to
        // potentially untrusted objects such as OutputStreams.

        private List<NodeFilter> nodeFilter;

        private System.Collections.Generic.HashSet<XmlNode> xpathNodeSet;
        /**
         * The node to be skipped/excluded from the DOM tree 
         * in subtree canonicalizations.
         */
        private XmlNode excludeNode;

        private static MemoryStream msDatosXML = new MemoryStream();
        private BinaryWriter bwDatosXML = new BinaryWriter(msDatosXML);

        protected const int NODE_BEFORE_DOCUMENT_ELEMENT = -1;
        protected const int NODE_NOT_BEFORE_OR_AFTER_DOCUMENT_ELEMENT = 0;
        protected const int NODE_AFTER_DOCUMENT_ELEMENT = 1;

        /**
         * The null xmlns definition.
         */
        private XmlAttribute nullNode;



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
        private TreeSet<XmlAttribute> result = new TreeSet<XmlAttribute>(COMPARE);

        private bool firstCall = true;

        //Se movio la clase XmlAttrStack a otro archivo

        //NO SE PUEDE DECLARAR PORQUE ES ESTATICA... WTF JAVA
        private XmlAttrStack xmlattrStack = new XmlAttrStack();
        //private XmlAttrStack xmlattrStack;

        private bool includeComments = false;

        public bool IncludeComments
        {
            get { return includeComments; }
            set { includeComments = value; }
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
                //-1
                int nodeLevel = NODE_BEFORE_DOCUMENT_ELEMENT;

                if (rootNode != null && typeof(XmlElement) == rootNode.GetType())
                {
                    //Fills the nssymbtable with the definitions of the parent of the root subnode

                    //Obtiene la lista de atributos de los elementos padres de este nodo y los namespaces, los namespaces los regstra en NS
                    getParentNameSpaces((XmlElement)rootNode, ns);
                    //0
                    nodeLevel = NODE_NOT_BEFORE_OR_AFTER_DOCUMENT_ELEMENT;
                }
                this.canonicalizeSubTree(rootNode, ns, rootNode, nodeLevel);
                this.bwDatosXML.Flush();
                //if (this.writer instanceof ByteArrayOutputStream) {
                if (typeof(BinaryWriter) == this.bwDatosXML.GetType() || typeof(MemoryStream) == this.bwDatosXML.GetType())
                {

                    byte[] result = msDatosXML.ToArray();
                    //byte[] result = msDatosXML.GetBuffer();
                    if (reset)
                    {
                        //((ByteArrayOutputStream)this.writer).reset();
                        this.bwDatosXML.Flush();
                    }
                    else
                    {
                        this.bwDatosXML.Close();
                    }
                    return result;
                }//Se removio el else if.
                else
                {
                    this.bwDatosXML.Close();
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
            BinaryWriter writer = this.bwDatosXML;
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
                            outputCommentToWriter((XmlComment)currentNode, writer, documentLevel);
                        }
                        break;

                    case XmlNodeType.ProcessingInstruction:
                        outputPItoWriter((XmlProcessingInstruction)currentNode, writer, documentLevel);
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                        outputTextToWriter(currentNode.Value, writer);
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
                                outputAttrToWriter(attr.Name, attr.Value, writer, cache);
                            }
                        }
                        writer.Write('>');
                        sibling = currentNode.FirstChild;
                        if (sibling == null)
                        {
                            writer.Write(cEND_TAG);
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
                    writer.Write(cEND_TAG);
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

        //
        /**
         * Outputs an Attribute to the internal Writer.
         *
         * The string value of the node is modified by replacing
         * <UL>
         * <LI>all ampersands (&) with <CODE>&amp;amp;</CODE></LI>
         * <LI>all open angle brackets (<) with <CODE>&amp;lt;</CODE></LI>
         * <LI>all quotation mark characters with <CODE>&amp;quot;</CODE></LI>
         * <LI>and the whitespace characters <CODE>#x9</CODE>, #xA, and #xD, with character
         * references. The character references are written in uppercase
         * hexadecimal with no leading zeroes (for example, <CODE>#xD</CODE> is represented
         * by the character reference <CODE>&amp;#xD;</CODE>)</LI>
         * </UL>
         *
         * @param name
         * @param value
         * @param writer 
         * @throws IOException
         */
        public static void outputAttrToWriter(String name, String value, BinaryWriter writer, Dictionary<String, byte[]> cache)
        {
            writer.Write(' ');
            writeByte(name, writer, cache);
            writer.Write(cEQUALS_STR);
            byte[] toWrite;
            int length = value.Length;
            int i = 0;
            while (i < length)
            {
                //Se cambia codePointAt
                int c = (int)value[i];
                i += charCount(c);

                switch (c)
                {

                    case '&':
                        toWrite = AMP;
                        break;

                    case '<':
                        toWrite = LT;
                        break;

                    case '"':
                        toWrite = QUOT;
                        break;

                    case 0x09:    // '\t'
                        toWrite = X9;
                        break;

                    case 0x0A:    // '\n'
                        toWrite = XA;
                        break;

                    case 0x0D:    // '\r'
                        toWrite = XD;
                        break;

                    default:
                        if (c < 0x80)
                        {
                            //Encoding.ASCII.GetString(new byte[]{ 65 });
                            //Convert.ToChar(65);
                            //(char)65;
                            writer.Write((char)c);
                        }
                        else
                        {
                            Uft8Helper.writeCodePointToUtf8(c, writer);
                        }
                        continue;
                }
                //writer.Write(System.Text.Encoding.BigEndianUnicode.GetString(toWrite));
                writer.Write(toWrite);
            }

            writer.Write('\"');
        }


        /**
         * Outputs a Text of CDATA section to the internal Writer.
         *
         * @param text
         * @param writer writer where to write the things
         * @throws IOException
         */
        public static void outputTextToWriter(String text, BinaryWriter writer)
        {
            int length = text.Length;
            byte[] toWrite;
            for (int i = 0; i < length; )
            {
                int c = (int)text[i];
                i += charCount(c);
                //Se quitaron clones
                switch (c)
                {

                    case '&':
                        toWrite = AMP;
                        break;

                    case '<':
                        toWrite = LT;
                        break;

                    case '>':
                        toWrite = GT;
                        break;

                    case 0xD:
                        toWrite = XD;
                        break;

                    default:
                        if (c < 0x80)
                        {
                            writer.Write((char)c);
                        }
                        else
                        {
                            Uft8Helper.writeCodePointToUtf8(c, writer);
                        }
                        continue;
                }
                //writer.Write(System.Text.Encoding.BigEndianUnicode.GetString(toWrite));
                writer.Write(toWrite);
            }
        }

        /**
         * Method outputCommentToWriter
         *
         * @param currentComment
         * @param writer writer where to write the things
         * @throws IOException
         */
        public void outputCommentToWriter(XmlComment currentComment, BinaryWriter writer, int position)
        {
            if (position == NODE_AFTER_DOCUMENT_ELEMENT)
            {
                writer.Write('\n');
            }
            writer.Write(cBEGIN_COMM);

            String data = currentComment.Data;
            int length = data.Length;

            for (int i = 0; i < length; )
            {
                int c = (int)data[i];
                i += charCount(c);
                if (c == 0x0D)
                {
                    writer.Write(cXD);
                }
                else
                {
                    if (c < 0x80)
                    {
                        writer.Write((char)c);
                    }
                    else
                    {
                        Uft8Helper.writeCodePointToUtf8(c, writer);
                    }
                }
            }

            writer.Write(cEND_COMM);
            if (position == NODE_BEFORE_DOCUMENT_ELEMENT)
            {
                writer.Write('\n');
            }
        }

        /**
         * Outputs a PI to the internal Writer.
         *
         * @param currentPI
         * @param writer where to write the things
         * @throws IOException
         */
        public void outputPItoWriter(XmlProcessingInstruction currentPI, BinaryWriter writer, int position)
        {
            if (position == NODE_AFTER_DOCUMENT_ELEMENT)
            {
                writer.Write('\n');
            }
            writer.Write(cBEGIN_PI);

            String target = currentPI.Target;
            int length = target.Length;

            for (int i = 0; i < length; )
            {
                int c = (int)target[i];
                i += charCount(c);
                if (c == 0x0D)
                {
                    writer.Write(cXD);
                }
                else
                {
                    if (c < 0x80)
                    {
                        writer.Write((char)c);
                    }
                    else
                    {
                        Uft8Helper.writeCodePointToUtf8(c, writer);
                    }
                }
            }

            String data = currentPI.Data;

            length = data.Length;

            if (length > 0)
            {
                writer.Write(' ');

                for (int i = 0; i < length; )
                {
                    //Se reemplaza codetopoint por indice [i]
                    int c = (int)data[i];
                    i += charCount(c);
                    if (c == 0x0D)
                    {
                        writer.Write(cXD);
                    }
                    else
                    {
                        Uft8Helper.writeCodePointToUtf8(c, writer);
                    }
                }
            }

            writer.Write(cEND_PI);
            if (position == NODE_BEFORE_DOCUMENT_ELEMENT)
            {
                writer.Write('\n');
            }
        }


        //
        public static bool isValidCodePoint(int codePoint)
        {
            int MAX_CODE_POINT = 0X10FFFF;
            // Optimized form of:
            //     codePoint >= MIN_CODE_POINT && codePoint <= MAX_CODE_POINT
            int plane = codePoint >> 16;
            return plane < ((MAX_CODE_POINT + 1) >> 16);
        }

        //
        public static int charCount(int codePoint)
        {
            int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;
            return codePoint >= MIN_SUPPLEMENTARY_CODE_POINT ? 2 : 1;
        }

        public static void writeByte(String str, BinaryWriter swOut, Dictionary<String, byte[]> cache)
        {
            //Se cambia el get por el valor del nombre del XmlElement

            byte[] result = null;

            try
            {
                result = cache[str];
            }
            catch { }

            if (result == null)
            {
                result = Uft8Helper.getStringInUtf8(str);
                cache.Add(str, result);
            }

            //swOut.Write(System.Text.Encoding.BigEndianUnicode.GetString(result));
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


        public static String joinURI(String baseURI, String relativeURI)
        {
            String bscheme = null;
            String bauthority = null;
            String bpath = "";
            String bquery = null;

            // pre-parse the baseURI
            if (baseURI != null)
            {
                if (baseURI.EndsWith(".."))
                {
                    baseURI = baseURI + "/";
                }
                Uri sbase = new Uri(baseURI);
                bscheme = sbase.Scheme;
                bauthority = sbase.Authority;
                bpath = sbase.AbsolutePath;
                bquery = sbase.Query;
            }

            Uri r = new Uri(relativeURI);
            String rscheme = r.Scheme;
            String rauthority = r.Authority;
            String rpath = r.AbsolutePath;
            String rquery = r.Query;

            String tscheme, tauthority, tpath, tquery;
            if (rscheme != null && rscheme.Equals(bscheme))
            {
                rscheme = null;
            }
            if (rscheme != null)
            {
                tscheme = rscheme;
                tauthority = rauthority;
                tpath = removeDotSegments(rpath);
                tquery = rquery;
            }
            else
            {
                if (rauthority != null)
                {
                    tauthority = rauthority;
                    tpath = removeDotSegments(rpath);
                    tquery = rquery;
                }
                else
                {
                    if (rpath.Length == 0)
                    {
                        tpath = bpath;
                        if (rquery != null)
                        {
                            tquery = rquery;
                        }
                        else
                        {
                            tquery = bquery;
                        }
                    }
                    else
                    {
                        if (rpath.StartsWith("/"))
                        {
                            tpath = removeDotSegments(rpath);
                        }
                        else
                        {
                            if (bauthority != null && bpath.Length == 0)
                            {
                                tpath = "/" + rpath;
                            }
                            else
                            {
                                int last = bpath.LastIndexOf('/');
                                if (last == -1)
                                {
                                    tpath = rpath;
                                }
                                else
                                {
                                    tpath = bpath.Substring(0, last + 1) + rpath;
                                }
                            }
                            tpath = removeDotSegments(tpath);
                        }
                        tquery = rquery;
                    }
                    tauthority = bauthority;
                }
                tscheme = bscheme;
            }

            //VERSION C# de clase de JAVA para Uris
            UriBuilder returnUri = new UriBuilder();
            returnUri.Scheme = tscheme;
            returnUri.Host = tauthority;
            returnUri.Path = tpath;
            returnUri.Query = tquery;
            returnUri.Fragment = null;

            //return new URI(tscheme, tauthority, tpath, tquery, null).toString();
            return returnUri.ToString();
        }

        private static String removeDotSegments(String path)
        {
            //SE QUITAN LAS PARTES DEL LOGGER
            /*if (log.isDebugEnabled())
            {
                log.debug("STEP   OUTPUT BUFFER\t\tINPUT BUFFER");
            }*/

            // 1. The input buffer is initialized with the now-appended path
            // components then replace occurrences of "//" in the input buffer
            // with "/" until no more occurrences of "//" are in the input buffer.
            String input = path;
            while (input.IndexOf("//") > -1)
            {
                input = input.Replace("//", "/");
            }

            // Initialize the output buffer with the empty string.
            StringBuilder output = new StringBuilder();

            // If the input buffer starts with a root slash "/" then move this
            // character to the output buffer.
            if (input[0] == '/')
            {
                output.Append("/");
                input = input.Substring(1);
            }

            //printStep("1 ", output.toString(), input);

            // While the input buffer is not empty, loop as follows
            while (input.Length != 0)
            {
                // 2A. If the input buffer begins with a prefix of "./",
                // then remove that prefix from the input buffer
                // else if the input buffer begins with a prefix of "../", then
                // if also the output does not contain the root slash "/" only,
                // then move this prefix to the end of the output buffer else
                // remove that prefix
                if (input.StartsWith("./"))
                {
                    input = input.Substring(2);
                    //printStep("2A", output.toString(), input);
                }
                else if (input.StartsWith("../"))
                {
                    input = input.Substring(3);
                    if (!output.ToString().Equals("/"))
                    {
                        output.Append("../");
                    }

                    //printStep("2A", output.toString(), input);

                    // 2B. if the input buffer begins with a prefix of "/./" or "/.",
                    // where "." is a complete path segment, then replace that prefix
                    // with "/" in the input buffer; otherwise,
                }
                else if (input.StartsWith("/./"))
                {
                    input = input.Substring(2);
                    //printStep("2B", output.toString(), input);
                }
                else if (input.Equals("/."))
                {
                    // FIXME: what is complete path segment?
                    input = ReplaceFirst(input, "/.", "/");

                    //printStep("2B", output.toString(), input);

                    // 2C. if the input buffer begins with a prefix of "/../" or "/..",
                    // where ".." is a complete path segment, then replace that prefix
                    // with "/" in the input buffer and if also the output buffer is
                    // empty, last segment in the output buffer equals "../" or "..",
                    // where ".." is a complete path segment, then append ".." or "/.."
                    // for the latter case respectively to the output buffer else
                    // remove the last segment and its preceding "/" (if any) from the
                    // output buffer and if hereby the first character in the output
                    // buffer was removed and it was not the root slash then delete a
                    // leading slash from the input buffer; otherwise,
                }
                else if (input.StartsWith("/../"))
                {
                    input = input.Substring(3);
                    if (output.Length == 0)
                    {
                        output.Append("/");
                    }
                    else if (output.ToString().EndsWith("../"))
                    {
                        output.Append("..");
                    }
                    else if (output.ToString().EndsWith(".."))
                    {
                        output.Append("/..");
                    }
                    else
                    {

                        int index = output.ToString().LastIndexOf("/");
                        if (index == -1)
                        {
                            output = new StringBuilder();
                            if (input[0] == '/')
                            {
                                input = input.Substring(1);
                            }
                        }
                        else
                        {
                            output = output.Remove(index, output.Length);
                        }
                    }
                    //printStep("2C", output.toString(), input);
                }
                else if (input.Equals("/.."))
                {
                    // FIXME: what is complete path segment?
                    input = ReplaceFirst(input, "/..", "/");
                    if (output.Length == 0)
                    {
                        output.Append("/");
                    }
                    else if (output.ToString().EndsWith("../"))
                    {
                        output.Append("..");
                    }
                    else if (output.ToString().EndsWith(".."))
                    {
                        output.Append("/..");
                    }
                    else
                    {
                        int index = output.ToString().LastIndexOf("/");
                        if (index == -1)
                        {
                            output = new StringBuilder();
                            if (input[0] == '/')
                            {
                                input = input.Substring(1);
                            }
                        }
                        else
                        {
                            output = output.Remove(index, output.Length);
                        }
                    }

                    //printStep("2C", output.ToString(), input);

                    // 2D. if the input buffer consists only of ".", then remove
                    // that from the input buffer else if the input buffer consists
                    // only of ".." and if the output buffer does not contain only
                    // the root slash "/", then move the ".." to the output buffer
                    // else delte it.; otherwise,
                }
                else if (input.Equals("."))
                {
                    input = "";
                    //printStep("2D", output.toString(), input);
                }
                else if (input.Equals(".."))
                {
                    if (!output.ToString().Equals("/"))
                    {
                        output.Append("..");
                    }
                    input = "";

                    //printStep("2D", output.toString(), input);
                    // 2E. move the first path segment (if any) in the input buffer
                    // to the end of the output buffer, including the initial "/"
                    // character (if any) and any subsequent characters up to, but not
                    // including, the next "/" character or the end of the input buffer.
                }
                else
                {
                    int end = -1;
                    int begin = input.IndexOf('/');
                    if (begin == 0)
                    {
                        end = input.IndexOf('/', 1);
                    }
                    else
                    {
                        end = begin;
                        begin = 0;
                    }
                    String segment;
                    if (end == -1)
                    {
                        segment = input.Substring(begin);
                        input = "";
                    }
                    else
                    {
                        segment = input.Substring(begin, end);
                        input = input.Substring(end);
                    }
                    output.Append(segment);
                    //printStep("2E", output.toString(), input);
                }
            }

            // 3. Finally, if the only or last segment of the output buffer is
            // "..", where ".." is a complete path segment not followed by a slash
            // then append a slash "/". The output buffer is returned as the result
            // of remove_dot_segments
            if (output.ToString().EndsWith(".."))
            {
                output.Append("/");
                //printStep("3 ", output.ToString(), input);
            }

            return output.ToString();
        }

        public static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
        
    }
}
