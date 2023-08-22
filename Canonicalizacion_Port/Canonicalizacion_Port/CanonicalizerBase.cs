using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO; 

namespace Canonicalizacion_Port
{
    abstract class CanonicalizerBase : CanonicalizerSpi
    {
        public const String XML = "xml";
        public const String XMLNS = "xmlns";
    
        protected AttrCompare COMPARE = new AttrCompare();
    
        // Make sure you clone the following mutable arrays before passing to
        // potentially untrusted objects such as OutputStreams.

        private readonly char[] cEND_PI = {'?','>'};
        private readonly byte[] END_PI = Encoding.Unicode.GetBytes(cEND_PI);

        private readonly char[] cBEGIN_PI = {'<','?'};
        private readonly byte[] BEGIN_PI = Encoding.Unicode.GetBytes(cBEGIN_PI);

        private readonly char[] cEND_COMM = {'-','-','>'};
        private readonly byte[] END_COMM = Encoding.Unicode.GetBytes(cEND_COMM);

        private readonly char[] cBEGIN_COMM = {'<','!','-','-'};
        private readonly byte[] BEGIN_COMM = Encoding.Unicode.GetBytes(cBEGIN_COMM);

        private readonly char[] cXA = {'&','#','x','A',';'};
        private readonly byte[] XA = Encoding.Unicode.GetBytes(cXA);

        private readonly char[] cX9 = {'&','#','x','9',';'};
        private readonly byte[] X9 = Encoding.Unicode.GetBytes(cX9);

        private readonly char[] cQUOT = {'&','q','u','o','t',';'};
        private readonly byte[] QUOT = Encoding.Unicode.GetBytes(cQUOT);

        private readonly char[] cXD = {'&','#','x','D',';'};
        private readonly byte[] XD = Encoding.Unicode.GetBytes(cXD);

        private readonly char[] cGT = {'&','g','t',';'};
        private readonly byte[] GT = Encoding.Unicode.GetBytes(cGT);

        private readonly char[] cLT = {'&','l','t',';'};
        private readonly byte[] LT = Encoding.Unicode.GetBytes(cLT);

        private readonly char[] cEND_TAG = {'<','/'};
        private readonly byte[] END_TAG = Encoding.Unicode.GetBytes(cEND_TAG);

        private readonly char[] cAMP = {'&','a','m','p',';'};
        private readonly byte[] AMP = Encoding.Unicode.GetBytes(cAMP);

        private char[] cEQUALS_STR = {'=','\"'};
        private byte[] EQUALS_STR = Encoding.Unicode.GetBytes(cEQUALS_STR);

        /*private readonly byte[] END_PI = {'?','>'};
        private readonly byte[] BEGIN_PI = {'<','?'};
        private readonly byte[] END_COMM = {'-','-','>'};
        private readonly byte[] BEGIN_COMM = {'<','!','-','-'};
        private readonly byte[] XA = {'&','#','x','A',';'};
        private readonly byte[] X9 = {'&','#','x','9',';'};
        private readonly byte[] QUOT = {'&','q','u','o','t',';'};
        private readonly byte[] XD = {'&','#','x','D',';'};
        private readonly byte[] GT = {'&','g','t',';'};
        private readonly byte[] LT = {'&','l','t',';'};
        private readonly byte[] END_TAG = {'<','/'};
        private readonly byte[] AMP = {'&','a','m','p',';'};
        private readonly byte[] EQUALS_STR = {'=','\"'};*/
    
        protected const int NODE_BEFORE_DOCUMENT_ELEMENT = -1;
        protected const int NODE_NOT_BEFORE_OR_AFTER_DOCUMENT_ELEMENT = 0;
        protected const int NODE_AFTER_DOCUMENT_ELEMENT = 1;
    
        private List<NodeFilter> nodeFilter;

        private bool includeComments;  
        private HashSet<XmlNode> xpathNodeSet;

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

        /**
         * Constructor CanonicalizerBase
         *
         * @param includeComments
         */
        public CanonicalizerBase(bool includeComments) 
        {
            this.includeComments = includeComments;
        }

        /**
         * Method engineCanonicalizeSubTree
         * @inheritDoc
         * @param rootNode
         * @throws CanonicalizationException
         */

        //ANTERIOR
        /*public byte[] engineCanonicalizeSubTree(XmlNode rootNode)
            throws CanonicalizationException {
            return engineCanonicalizeSubTree(rootNode, (Node)null);
        }*/

        //NUEVA
        public byte[] engineCanonicalizeSubTree(XmlNode rootNode){
            return engineCanonicalizeSubTree(rootNode, (XmlNode)null);
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
            try {

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

            }catch (Exception ex) 
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

                    case XmlNodeType.Entity :
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
                            outputCommentToWriter((XmlComment) currentNode, writer, documentLevel);
                        }
                        break;

                    case XmlNodeType.ProcessingInstruction :
                        outputPItoWriter((XmlProcessingInstruction) currentNode, writer, documentLevel);
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
                        if (sibling == null) {
                            writer.Write(END_TAG.Clone());
                            writeStringToUtf8(name, writer);        
                            writer.Write('>');
                            //We finished with this level, pop to the previous definitions.
                            ns.outputNodePop();
                            if (parentNode != null) {
                                sibling = currentNode.NextSibling;
                            }
                        } else {
                            parentNode = currentElement;
                        }
                        break;
                
                    case XmlNodeType.DocumentType:
                    default :
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
                }while(true);
        }


        /**
         * @param writer The writer to set.
         */
         public void setWriter(StreamWriter writer)
         {
                this.writer = writer;
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
            if (nsprefix != null && "".Equals(nsprefix.Value)) {
                ns.addMappingAndRender(
                        XMLNS, "", getNullNode(nsprefix.OwnerDocument));
            }
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

        /**
         * Method outputCommentToWriter
         *
         * @param currentComment
         * @param writer writer where to write the things
         * @throws IOException
         */
        protected void outputCommentToWriter(XmlComment currentComment, StreamWriter writer, int position)
        {   	  
            if (position == NODE_AFTER_DOCUMENT_ELEMENT) 
            {
                writer.Write('\n');
            }
            writer.Write(BEGIN_COMM.Clone());

            String data = currentComment.Data;
            int length = data.Length;      

            for (int i = 0; i < length; ) 
            {
                int c = (int)data[i];
                i += charCount(c);
                if (c == 0x0D) 
                {
                    writer.Write(XD.Clone());
                } 
                else 
                {
                    if (c < 0x80) 
                    {
                        writer.Write(c);
                    } 
                    else 
                    {
                        writeCodePointToUtf8(c, writer);
                    }    
                }      
            }

            writer.Write(END_COMM.Clone());
            if (position == NODE_BEFORE_DOCUMENT_ELEMENT) {
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
        protected void outputPItoWriter(XmlProcessingInstruction currentPI, StreamWriter writer, int position) 
        {   	  
            if (position == NODE_AFTER_DOCUMENT_ELEMENT) 
            {
                writer.Write('\n');
            }
            writer.Write(BEGIN_PI.Clone());

            String target = currentPI.Target;
            int length = target.Length;

            for (int i = 0; i < length; ) 
            {
                int c = (int)target[i];
                i += charCount(c);
                if (c == 0x0D) 
                {
                    writer.Write(XD.Clone());
                } 
                else 
                {
                    if (c < 0x80) 
                    {
                        writer.Write(c);
                    } 
                    else 
                    {
                        writeCodePointToUtf8(c, writer);
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
                        writer.Write(XD.Clone());
                    } 
                    else 
                    {
                        writeCodePointToUtf8(c, writer);
                    }
                }
            }

            writer.Write(END_PI.Clone());
            if (position == NODE_BEFORE_DOCUMENT_ELEMENT) 
            {
                writer.Write('\n');
            }
        }

        /**
         * Outputs a Text of CDATA section to the internal Writer.
         *
         * @param text
         * @param writer writer where to write the things
         * @throws IOException
         */
        protected static void outputTextToWriter(String text, StreamWriter writer)
        {
            int length = text.Length;
            byte[] toWrite;
            for (int i = 0; i < length; ) 
            {
                int c = (int)text[i];
                i += charCount(c);
                //Se quitaron clones
                switch (c) {

                case '&' :
                    toWrite = AMP;
                    break;

                case '<' :
                    toWrite = LT;
                    break;

                case '>' :
                    toWrite = GT;
                    break;

                case 0xD :
                    toWrite = XD;
                    break;

                default :
                    if (c < 0x80) 
                    {
                        writer.Write(c);
                    } 
                    else 
                    {
                        writeCodePointToUtf8(c, writer);
                    }
                    continue;
                }
                writer.Write(toWrite);
            }
        }

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
        protected static void outputAttrToWriter(String name, String value, StreamWriter writer, Dictionary<String, byte[]> cache) 
        {
            writer.Write(' ');
            writeByte(name, writer, cache);
            writer.Write(EQUALS_STR.Clone());
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

                case '&' :
                    toWrite = AMP;
                    break;

                case '<' :
                    toWrite = LT;
                    break;

                case '"' :
                    toWrite = QUOT;
                    break;

                case 0x09 :    // '\t'
                    toWrite = X9;
                    break;

                case 0x0A :    // '\n'
                    toWrite = XA;
                    break;

                case 0x0D :    // '\r'
                    toWrite = XD;
                    break;

                default :
                    if (c < 0x80) 
                    {
                        writer.Write(c);
                    } 
                    else 
                    {
                        writeCodePointToUtf8(c, writer);
                    }
                    continue;
                }
                writer.Write(toWrite);
            }

            writer.Write('\"');
        }

        public static int charCount(int codePoint)
        {
            int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;
            return codePoint >= MIN_SUPPLEMENTARY_CODE_POINT ? 2 : 1;
        }

        ////////////////////////////////////////////////////////////CLASE UTF8Helpper//////////////////////////////////////////////

        public static bool isValidCodePoint(int codePoint) 
        {
            int MAX_CODE_POINT = 0X10FFFF;
            // Optimized form of:
            //     codePoint >= MIN_CODE_POINT && codePoint <= MAX_CODE_POINT
            int plane = codePoint >> 16;
            return plane < ((MAX_CODE_POINT + 1) >> 16);
        }

        public static void writeCodePointToUtf8(int c, StreamWriter swOutput)
        {
            int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;
            if (!isValidCodePoint(c) || c >= 0xD800 && c <= 0xDBFF || c >= 0xDC00 && c <= 0xDFFF) 
            {
                // valid code point: c >= 0x0000 && c <= 0x10FFFF
                swOutput.Write(0x3f);
                return;
            }
            // OLD UTF8?
            bool OLD_UTF8 = false;
            if (OLD_UTF8 && c >= MIN_SUPPLEMENTARY_CODE_POINT) 
            {
                // version 2 or before output 2 question mark characters for 32 bit chars
                swOutput.Write(0x3f);
                swOutput.Write(0x3f);
                return;
            }

            if (c < 0x80) 
            {
                // 0x00000000 - 0x0000007F
                // 0xxxxxxx
                swOutput.Write(c);
                return;
            }
            byte extraByte = 0;
            if (c < 0x800) 
            {
                // 0x00000080 - 0x000007FF
                // 110xxxxx 10xxxxxx
                extraByte = 1;
            } 
            else if (c < 0x10000) 
            {
                // 0x00000800 - 0x0000FFFF
                // 1110xxxx 10xxxxxx 10xxxxxx
                extraByte = 2;
            } 
            else if (c < 0x200000) 
            {
                // 0x00010000 - 0x001FFFFF
                // 11110xxx 10xxxxx 10xxxxxx 10xxxxxx
                extraByte = 3;
            } 
            else if (c < 0x4000000) 
            {
                // 0x00200000 - 0x03FFFFFF
                // 111110xx 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx
                // already outside valid Character range, just for completeness
                extraByte = 4;
            } 
            else if (c <= 0x7FFFFFFF) 
            {
                // 0x04000000 - 0x7FFFFFFF
                // 1111110x 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx
                // already outside valid Character range, just for completeness
                extraByte = 5;
            } 
            else 
            {
                // 0x80000000 - 0xFFFFFFFF
                // case not possible as java has no unsigned int
                swOutput.Write(0x3f);
                return;
            }
        
            byte write;
            int shift = 6 * extraByte;

            write = (byte)((0xFE << (6 - extraByte)) | (c >> shift));
            swOutput.Write(write);

            for (int i = extraByte - 1; i >= 0; i--) 
            {
                shift -= 6;
                write = (byte)(0x80 | ((c >> shift) & 0x3F));
                swOutput.Write(write);
            }
        }
        public static void writeByte(String str, StreamWriter swOut, Dictionary<String, byte[]> cache)
        {
            //Se cambia el get por el valor del nombre del XmlElement
            byte[] result = cache[str];
            if (result == null) 
            {
                result = getStringInUtf8(str);
                cache.Add(str, result);
            }

            swOut.Write(result);
        }

        //EVALUAR SI PUEDE REEMPLAZARSE POR METODO EN c#
        public static byte[] getStringInUtf8(String str) 
        {
            int length = str.Length;
            bool expanded = false;
            byte[] result = new byte[length];
            int i = 0;
            int Iout = 0;
            int c;
            while (i < length) 
            {
                //Se cambia codepointat por [i]
                c = (int)str[i];
                i += charCount(c);
                if (!isValidCodePoint(c) || c >= 0xD800 && c <= 0xDBFF || c >= 0xDC00 && c <= 0xDFFF) 
                {
                    // valid code point: c >= 0x0000 && c <= 0x10FFFF
                    result[Iout++] = (byte)0x3f;
                    continue;
                }
                // OLD UTF8?
                bool OLD_UTF8 = false;
                int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;
                if (OLD_UTF8 && c >= MIN_SUPPLEMENTARY_CODE_POINT) 
                {
                    // version 2 or before output 2 question mark characters for 32 bit chars
                    result[Iout++] = (byte)0x3f;
                    result[Iout++] = (byte)0x3f;
                    continue;
                }
                if (c < 0x80) 
                {
                    result[Iout++] = (byte)c;
                    continue;
                }
                if (!expanded) 
                {
                    byte[] newResult = new byte[6*length];
                    //VERIFICAR COPY
                    Array.Copy(result, 0, newResult, 0, Iout);
                    result = newResult;
                    expanded = true;
                }
                byte extraByte = 0;
                if (c < 0x800) 
                {
                    // 0x00000080 - 0x000007FF
                    // 110xxxxx 10xxxxxx
                    extraByte = 1;
                } 
                else if (c < 0x10000) 
                {
                    // 0x00000800 - 0x0000FFFF
                    // 1110xxxx 10xxxxxx 10xxxxxx
                    extraByte = 2;
                } 
                else if (c < 0x200000) 
                {
                    // 0x00010000 - 0x001FFFFF
                    // 11110xxx 10xxxxx 10xxxxxx 10xxxxxx
                    extraByte = 3;
                } 
                else if (c < 0x4000000) 
                {
                    // 0x00200000 - 0x03FFFFFF
                    // 111110xx 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx
                    // already outside valid Character range, just for completeness
                    extraByte = 4;
                } 
                else if (c <= 0x7FFFFFFF) 
                {
                    // 0x04000000 - 0x7FFFFFFF
                    // 1111110x 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx
                    // already outside valid Character range, just for completeness
                    extraByte = 5;
                } 
                else 
                {
                    // 0x80000000 - 0xFFFFFFFF
                    // case not possible as java has no unsigned int
                    result[Iout++] = 0x3f;
                    continue;
                }
                byte write;
                int shift = 6 * extraByte;
                write = (byte)((0xFE << (6 - extraByte)) | (c >> shift));
                result[Iout++] = write;
                for (int j = extraByte - 1; j >= 0; j--) 
                {
                    shift -= 6;
                    write = (byte)(0x80 | ((c >> shift) & 0x3F));
                    result[Iout++] = write;
                }
            }
            if (expanded) 
            {
                byte[] newResult = new byte[Iout];
                Array.Copy(result, 0, newResult, 0, Iout);
                result = newResult;
            }
            return result;
        }

        public static void writeStringToUtf8(String str, StreamWriter swOut)
        {
            int length = str.Length;
            int i = 0;
            int c;
            while (i < length) 
            {
                c = (int)str[i];
                i += charCount(c);
                if (!isValidCodePoint(c) || c >= 0xD800 && c <= 0xDBFF || c >= 0xDC00 && c <= 0xDFFF) 
                {
                    // valid code point: c >= 0x0000 && c <= 0x10FFFF
                    swOut.Write(0x3f);
                    continue;
                }
                bool OLD_UTF8 = false;
                int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;
                if (OLD_UTF8 && c >= MIN_SUPPLEMENTARY_CODE_POINT) 
                {
                    // version 2 or before output 2 question mark characters for 32 bit chars
                    swOut.Write(0x3f);
                    swOut.Write(0x3f);
                    continue;
                }
                if (c < 0x80)  
                {
                    swOut.Write(c);
                    continue;
                }
                byte extraByte = 0;
                if (c < 0x800) 
                {
                    // 0x00000080 - 0x000007FF
                    // 110xxxxx 10xxxxxx
                    extraByte = 1;
                } 
                else if (c < 0x10000) 
                {
                    // 0x00000800 - 0x0000FFFF
                    // 1110xxxx 10xxxxxx 10xxxxxx
                    extraByte = 2;
                } 
                else if (c < 0x200000) 
                {
                    // 0x00010000 - 0x001FFFFF
                    // 11110xxx 10xxxxx 10xxxxxx 10xxxxxx
                    extraByte = 3;
                } 
                else if (c < 0x4000000) 
                {
                    // 0x00200000 - 0x03FFFFFF
                    // 111110xx 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx
                    // already outside valid Character range, just for completeness
                    extraByte = 4;
                } 
                else if (c <= 0x7FFFFFFF) 
                {
                    // 0x04000000 - 0x7FFFFFFF
                    // 1111110x 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx
                    // already outside valid Character range, just for completeness
                    extraByte = 5;
                } 
                else 
                {
                    // 0x80000000 - 0xFFFFFFFF
                    // case not possible as java has no unsigned int
                    swOut.Write(0x3f);
                    continue;
                }
                byte write;
                int shift = 6 * extraByte;
                write = (byte)((0xFE << (6 - extraByte)) | (c >> shift));
                swOut.Write(write);
                for (int j = extraByte - 1; j >= 0; j--) 
                {
                    shift -= 6;
                    write = (byte)(0x80 | ((c >> shift) & 0x3F));
                    swOut.Write(write);
                }

            }

        }
        ////////////////////////////////////////////////////////////CLASE UTF8Helpper//////////////////////////////////////////////

        public abstract IEnumerator<XmlAttribute> handleAttributesSubtree(XmlElement element, NameSpaceSymbTable ns);
        

      
    }

}
