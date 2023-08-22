using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using C5;
using System.Collections.ObjectModel;

namespace Canonicalizacion_Port
{
    class Canonicalizer11 : CanonicalizerBase
    {
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
        private  XmlAttrStack xmlattrStack;

        private bool comments = false;

        public bool Comments
        {
            get { return comments; }
            set { comments = value; }
        }

       

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
        public override IEnumerator<XmlAttribute> handleAttributesSubtree(XmlElement element, NameSpaceSymbTable ns)
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
                    XmlAttribute attribute = (XmlAttribute) attrs.Item(i);
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

            if (firstCall) {

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
            if (baseURI != null) {
                if (baseURI.EndsWith("..")) {
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
            if (rscheme != null && rscheme.Equals(bscheme)) {
                rscheme = null;
            }
            if (rscheme != null) {
                tscheme = rscheme;
                tauthority = rauthority;
                tpath = removeDotSegments(rpath);
                tquery = rquery;
            } else {
                if (rauthority != null) {
                    tauthority = rauthority;
                    tpath = removeDotSegments(rpath);
                    tquery = rquery;
                } else {
                    if (rpath.Length == 0) {
                        tpath = bpath;
                        if (rquery != null) {
                            tquery = rquery;
                        } else {
                            tquery = bquery;
                        }
                    } else {
                        if (rpath.StartsWith("/")) {
                            tpath = removeDotSegments(rpath);
                        } else {
                            if (bauthority != null && bpath.Length == 0) {
                                tpath = "/" + rpath;
                            } else { 
                                int last = bpath.LastIndexOf('/');
                                if (last == -1) {
                                    tpath = rpath;
                                } else {
                                    tpath = bpath.Substring(0, last+1) + rpath;
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
                    input = ReplaceFirst(input,"/..", "/");
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

        public override String engineGetURI()
        {
            if (comments == true)
            {
                return Canonicalizer.ALGO_ID_C14N11_OMIT_COMMENTS;
            }
            else
            {
                return Canonicalizer.ALGO_ID_C14N11_WITH_COMMENTS;
            }
        }

        public override bool engineGetIncludeComments()
        {
            if (comments == true)
            {
                return false;
            }
            else
            {
                return true;
            } 
        }


    }
}
