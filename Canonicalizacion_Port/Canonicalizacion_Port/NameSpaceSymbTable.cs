using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml;
using System.Collections;

namespace Canonicalizacion_Port
{
    class NameSpaceSymbTable
    {
        private const String XMLNS = "xmlns";
        private static readonly SymbMap initialMap = new SymbMap();
    
        static NameSpaceSymbTable() 
        {
            NameSpaceSymbEntry ne = new NameSpaceSymbEntry("", null, true, XMLNS);
            ne.lastrendered = "";		
            initialMap.put(XMLNS, ne);
            
        }
    
        /**The map betwen prefix-> entry table. */
        private SymbMap symb;
    
        /**The stacks for removing the definitions when doing pop.*/
        private List<SymbMap> level;
        private bool cloned = true;
    
        /**
         * Default constractor
         **/		
        public NameSpaceSymbTable() 
        {    	
            level = new List<SymbMap>();
            //Insert the default binding for xmlns.    	
            symb = (SymbMap) initialMap.clone();
        }

        /**
         * Get all the unrendered nodes in the name space.
         * For Inclusive rendering
         * @param result the list where to fill the unrendered xmlns definitions.
         **/
        public void getUnrenderedNodes(Collection<XmlAttribute> result) 
        {		
            IEnumerator<NameSpaceSymbEntry> it = symb.entrySet().GetEnumerator();
            while (it.MoveNext()) 
            {	   	   
                NameSpaceSymbEntry n = it.Current;
                //put them rendered?
                if (!n.rendered && n.n != null) 
                {
                    n = (NameSpaceSymbEntry) n.clone();
                    needsClone();
                    symb.put(n.prefix, n);         
                    n.lastrendered = n.uri;
                    n.rendered = true;

                    result.Add(n.n);
                }
            }	   
        }

        /**
         * Push a frame for visible namespace. 
         * For Inclusive rendering.
         **/
        public void outputNodePush() {
            push();
        }

        /**
         * Pop a frame for visible namespace.
         **/
        public void outputNodePop() {
            pop();
        }

        /**
         * Push a frame for a node.
         * Inclusive or Exclusive.
         **/
        public void push() {		
            //Put the number of namespace definitions in the stack.
            level.Add(null);
            cloned = false;
        }

        /**
         * Pop a frame.
         * Inclusive or Exclusive.
         **/
        public void pop() {
            int size = level.Count() - 1;

            //REVSIAR
            Object ob = level[size];
            level.RemoveAt(size);

            if (ob != null) 
            {
                symb = (SymbMap)ob;
                if (size == 0) 
                {
                    cloned = false;   
                } 
                else 
                {
                    cloned = level[size - 1] != symb;
                }
            } 
            else 
            {
                cloned = false;
            }
        }

       public void needsClone() {
            if (!cloned) 
            {		
                level[level.Count() - 1] =  symb;
                symb = (SymbMap) symb.clone();
                cloned = true;
            }
        }


        /**
         * Gets the attribute node that defines the binding for the prefix.      
         * @param prefix the prefix to obtain the attribute.
         * @return null if there is no need to render the prefix. Otherwise the node of
         * definition.
         **/
        public XmlAttribute getMapping(String prefix) 
        {					
            NameSpaceSymbEntry entry = symb.get(prefix);
            if (entry == null) 
            {
                //There is no definition for the prefix(a bug?).
                return null;
            }
            if (entry.rendered) 
            {		
                //No need to render an entry already rendered.
                return null;		
            }
            // Mark this entry as render.
            entry = (NameSpaceSymbEntry) entry.clone();
            needsClone();
            symb.put(prefix, entry);
            entry.rendered = true;
            entry.lastrendered = entry.uri;				
            // Return the node for outputing.
            return entry.n;
        }

        /**
         * Gets a definition without mark it as render. 
         * For render in exclusive c14n the namespaces in the include prefixes.
         * @param prefix The prefix whose definition is neaded.
         * @return the attr to render, null if there is no need to render
         **/
        public XmlAttribute getMappingWithoutRendered(String prefix) 
        {					
            NameSpaceSymbEntry entry = symb.get(prefix);
            if (entry == null) 
            {		   
                return null;
            }
            if (entry.rendered) 
            {		
                return null;		
            }
            return entry.n;
        }

        /**
         * Adds the mapping for a prefix.
         * @param prefix the prefix of definition
         * @param uri the Uri of the definition
         * @param n the attribute that have the definition
         * @return true if there is already defined.
         **/
        public bool addMapping(String prefix, String uri, XmlAttribute n) 
        {						
            NameSpaceSymbEntry ob = symb.get(prefix);		
            if (ob != null && uri.Equals(ob.uri)) 
            {
                //If we have it previously defined. Don't keep working.
                return false;
            }			
            //Creates and entry in the table for this new definition.
            NameSpaceSymbEntry ne = new NameSpaceSymbEntry(uri, n, false, prefix);		
            needsClone();
            symb.put(prefix, ne);
            if (ob != null) 
            {
                //We have a previous definition store it for the pop.			
                //Check if a previous definition(not the inmidiatly one) has been rendered.			
                ne.lastrendered = ob.lastrendered;			
                if (ob.lastrendered != null && ob.lastrendered.Equals(uri)) 
                {
                    //Yes it is. Mark as rendered.
                    ne.rendered = true;
                }			
            } 			
            return true;
        }

        /**
         * Adds a definition and mark it as render.
         * For inclusive c14n.
         * @param prefix the prefix of definition
         * @param uri the Uri of the definition
         * @param n the attribute that have the definition
         * @return the attr to render, null if there is no need to render
         **/
        public XmlNode addMappingAndRender(String prefix, String uri, XmlAttribute n) 
        {                     
            NameSpaceSymbEntry ob = symb.get(prefix);

            if (ob != null && uri.Equals(ob.uri)) 
            {
                if (!ob.rendered) 
                {                 
                    ob = (NameSpaceSymbEntry) ob.clone();
                    needsClone();
                    symb.put(prefix, ob);         
                    ob.lastrendered = uri;
                    ob.rendered = true;
                    return ob.n;
                }           
                return null;
            }   

            NameSpaceSymbEntry ne = new NameSpaceSymbEntry(uri,n,true,prefix);
            ne.lastrendered = uri;
            needsClone();
            symb.put(prefix, ne);
            if (ob != null && ob.lastrendered != null && ob.lastrendered.Equals(uri)) 
            {
                ne.rendered = true;
                return null;
            }
            return ne.n;
        }

        public int getLevel() 
        {
            return level.Count();
        }

        public void removeMapping(String prefix) 
        {
            NameSpaceSymbEntry ob = symb.get(prefix);

            if (ob != null) 
            {
                needsClone();
                symb.put(prefix, null);         
            }
        }

        public void removeMappingIfNotRender(String prefix) 
        {
            NameSpaceSymbEntry ob = symb.get(prefix);

            if (ob != null && !ob.rendered) 
            {
                needsClone();
                symb.put(prefix, null);         
            }
        }

        public bool removeMappingIfRender(String prefix) 
        {
            NameSpaceSymbEntry ob = symb.get(prefix);

            if (ob != null && ob.rendered) 
            {
                needsClone();
                symb.put(prefix, null);         
            }
            return false;
        }
    }

    class NameSpaceSymbEntry
    {
    
        public String prefix;
    
        /**The URI that the prefix defines */
        public String uri;
    
        /**The last output in the URI for this prefix (This for speed reason).*/
        public String lastrendered = null;
    
        /**This prefix-URI has been already render or not.*/
        public bool rendered = false;
    
        /**The attribute to include.*/
        public XmlAttribute n;     
    
        public NameSpaceSymbEntry(String name, XmlAttribute n, bool rendered, String prefix) 
        {
            this.uri = name;          
            this.rendered = rendered;
            this.n = n;            
            this.prefix = prefix;
        }
    
        /** @inheritDoc */
        public Object clone() 
        {         
            try 
            {
                return base.MemberwiseClone();

            } catch (Exception e) 
            {
                return null;
            }
        }
    }

    class SymbMap 
    {
        public int free = 23;
        public NameSpaceSymbEntry[] entries;
        public String[] keys;
    
        public SymbMap() 
        {
            entries = new NameSpaceSymbEntry[free];
            keys = new String[free];
        }
    
        public void put(String key, NameSpaceSymbEntry value) 
        {		
            int index = indexA(key);
            Object oldKey = keys[index];
            keys[index] = key;
            entries[index] = value;
            if ((oldKey == null || !oldKey.Equals(key)) && --free == 0) 
            {	        	        
                free = entries.Length;
                int newCapacity = free << 2;				
                rehash(newCapacity);			
            }
        }

        public List<NameSpaceSymbEntry> entrySet() 
        {
            List<NameSpaceSymbEntry> a = new List<NameSpaceSymbEntry>();
            for (int i = 0;i < entries.Length;i++) 
            {
                if (entries[i] != null && !"".Equals(entries[i].uri)) 
                {
                    a.Add(entries[i]);
                }
            }
            return a;		
        }

        protected int indexA(Object obj) 
        {		
            Object[] set = keys;
            int length = set.Length;
            //abs of index
            int index = (obj.GetHashCode() & 0x7fffffff) % length;
            Object cur = set[index];

            if (cur == null || cur.Equals(obj)) 
            {
                return index;
            }
            length--;
            do 
            {
                index = index == length ? 0 : ++index;
                cur = set[index];
            } 
            while (cur != null && !cur.Equals(obj));       
            return index;
        }

        /**
         * rehashes the map to the new capacity.
         *
         * @param newCapacity an <code>int</code> value
         */
        protected void rehash(int newCapacity) 
        {
            int oldCapacity = keys.Length;
            String[] oldKeys = keys;
            NameSpaceSymbEntry[] oldVals = entries;

            keys = new String[newCapacity];        
            entries = new NameSpaceSymbEntry[newCapacity];

            for (int i = oldCapacity; i-- > 0;) 
            {
                if (oldKeys[i] != null) 
                {
                    String o = oldKeys[i];
                    int index = indexA(o);
                    keys[index] = o;
                    entries[index] = oldVals[i];
                }
            }
        }

        public NameSpaceSymbEntry get(String key) 
        {
            return entries[indexA(key)];
        }

        public Object clone()  
        {
            try 
            {
                SymbMap copy = (SymbMap) base.MemberwiseClone();
                copy.entries = new NameSpaceSymbEntry[entries.Length];
                Array.Copy(entries, 0, copy.entries, 0, entries.Length);
                copy.keys = new String[keys.Length];
                Array.Copy(keys, 0, copy.keys, 0, keys.Length);

                return copy;
            } catch (Exception e) 
            {
                String Excep = e.StackTrace;
            }
            return null;
        }
    }
    
}
