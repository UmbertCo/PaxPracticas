using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections.ObjectModel;

namespace Canonicalizacion_Tests
{
    class XmlAttrStack
    {

        class XmlsStackElement
        {
            public int level;
            public bool rendered = false;
            public List<XmlAttribute> nodes = new List<XmlAttribute>();
        }

        int currentLevel = 0;
        int lastlevel = 0;
        XmlsStackElement cur;
        List<XmlsStackElement> levels = new List<XmlsStackElement>();

        public void push(int level)
        {
            currentLevel = level;
            if (currentLevel == -1)
            {
                return;
            }
            cur = null;
            while (lastlevel >= currentLevel)
            {
                levels.Remove(levels[levels.Count - 1]);
                int newSize = levels.Count;
                if (newSize == 0)
                {
                    lastlevel = 0;
                    return;
                }
                lastlevel = newSize - 1;
            }
        }

        public void addXmlnsAttr(XmlAttribute n)
        {
            if (cur == null)
            {
                cur = new XmlsStackElement();
                cur.level = currentLevel;
                levels.Add(cur);
                lastlevel = currentLevel;
            }
            cur.nodes.Add(n);
        }

        public void getXmlnsAttr(Collection<XmlAttribute> col) 
        {
            int size = levels.Count - 1;
            if (cur == null) 
            {
                cur = new XmlsStackElement();
                cur.level = currentLevel;
                lastlevel = currentLevel;
                levels.Add(cur);
            }
            bool parentRendered = false;
            XmlsStackElement e = null;
            if (size == -1) 
            {
                parentRendered = true;
            } else 
            {
                e = levels[size];
                if (e.rendered && e.level + 1 == currentLevel) 
                {
                    parentRendered = true;
                }
            }
            if (parentRendered) 
            {
                foreach (XmlAttribute attr in cur.nodes)
	            {
		            col.Add(attr);
	            }
                cur.rendered = true;
                return;
            }

            Dictionary<String, XmlAttribute> loa = new Dictionary<String, XmlAttribute>();    		
            List<XmlAttribute> baseAttrs = new List<XmlAttribute>();
            bool successiveOmitted = true;
            for (; size >= 0; size--) 
            {
                e = levels[size];
                if (e.rendered) 
                {
                    successiveOmitted = false;
                }
                IEnumerator<XmlAttribute> it = e.nodes.GetEnumerator();
                while (it.MoveNext() && successiveOmitted) 
                {
                    XmlAttribute n = it.Current;
                    if (n.LocalName.Equals("base") && !e.rendered) 
                    {
                        baseAttrs.Add(n);
                    } 
                    else if (!loa.ContainsKey(n.Name)) 
                    {
                        loa.Add(n.Name, n);
                    }
                }
            }
            if (!baseAttrs.Any()) 
            {
                IEnumerator<XmlAttribute> it = col.GetEnumerator();
                String sbase = null;
                XmlAttribute baseAttr = null;
                while (it.MoveNext()) 
                {
                    XmlAttribute n = it.Current;
                    if (n.LocalName.Equals("base")) 
                    {
                        sbase = n.Value;
                        baseAttr = n;
                        break;
                    }
                }
                it = baseAttrs.GetEnumerator();
                while (it.MoveNext()) 
                {
                    XmlAttribute n = it.Current;
                    if (sbase == null) 
                    {
                        sbase = n.Value;
                        baseAttr = n;
                    } 
                    else 
                    {
                        try 
                        {
                            sbase = Canonicalizacion.joinURI(n.Value, sbase);
                        } 
                        catch (Exception ue) 
                        {
                            /*if (log.isDebugEnabled()) 
                            {
                                log.debug(ue.getMessage(), ue);
                            }*/
                        }
                    }
                }
                if (sbase != null && sbase.Length != 0) {
                    baseAttr.Value = sbase;
                    col.Add(baseAttr);
                }
            }

            cur.rendered = true;

            foreach (XmlAttribute attr in loa.Values)
            {
                col.Add(attr);
            }
            //col.addAll(loa.values());
        }
    
        //Fin de clase
    }
}
