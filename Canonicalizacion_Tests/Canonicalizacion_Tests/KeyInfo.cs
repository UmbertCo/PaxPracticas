﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canonicalizacion_Tests
{
    class KeyInfo : SignatureElementProxy
    {

        // We need at least one StorageResolver otherwise
        // the KeyResolvers would not be called.
        // The default StorageResolver is null.

        private List<X509Data> x509Datas = null;
        private List<EncryptedKey> encryptedKeys = null;
    
        private static final List<StorageResolver> nullList;
        static {
            List<StorageResolver> list = new ArrayList<StorageResolver>(1);
            list.add(null);
            nullList = java.util.Collections.unmodifiableList(list);
        }

        /** Field storageResolvers */
        private List<StorageResolver> storageResolvers = nullList;
    
        /**
         * Stores the individual (per-KeyInfo) {@link KeyResolverSpi}s
         */
        private List<KeyResolverSpi> internalKeyResolvers = new ArrayList<KeyResolverSpi>();
    
        private boolean secureValidation;

        /**
         * Constructor KeyInfo
         * @param doc
         */
        public KeyInfo(Document doc) {
            super(doc);
            addReturnToSelf();
        
            String prefix = ElementProxy.getDefaultPrefix(this.getBaseNamespace());
            if (prefix != null && prefix.length() > 0) {
                getElement().setAttributeNS(Constants.NamespaceSpecNS, "xmlns:" + prefix, 
                                            this.getBaseNamespace());
            }
        
        }

        /**
         * Constructor KeyInfo
         *
         * @param element
         * @param baseURI
         * @throws XMLSecurityException
         */
        public KeyInfo(Element element, String baseURI) throws XMLSecurityException {
            super(element, baseURI);
        
            Attr attr = element.getAttributeNodeNS(null, "Id");
            if (attr != null) {
                element.setIdAttributeNode(attr, true);
            }
        }
    
        /**
         * Set whether secure processing is enabled or not. The default is false.
         */
        public void setSecureValidation(boolean secureValidation) {
            this.secureValidation = secureValidation;
        }

        /**
         * Sets the <code>Id</code> attribute
         *
         * @param id ID
         */
        public void setId(String id) {
            if (id != null) {
                setLocalIdAttribute(Constants._ATT_ID, id);
            }
        }

        /**
         * Returns the <code>Id</code> attribute
         *
         * @return the <code>Id</code> attribute
         */
        public String getId() {
            return getLocalAttribute(Constants._ATT_ID);
        }

        /**
         * Method addKeyName
         *
         * @param keynameString
         */
        public void addKeyName(String keynameString) {
            this.add(new KeyName(getDocument(), keynameString));
        }

        /**
         * Method add
         *
         * @param keyname
         */
        public void add(KeyName keyname) {
            appendSelf(keyname);
            addReturnToSelf();
        }

        /**
         * Method addKeyValue
         *
         * @param pk
         */
        public void addKeyValue(PublicKey pk) {
            this.add(new KeyValue(getDocument(), pk));
        }

        /**
         * Method addKeyValue
         *
         * @param unknownKeyValueElement
         */
        public void addKeyValue(Element unknownKeyValueElement) {
            this.add(new KeyValue(getDocument(), unknownKeyValueElement));
        }

        /**
         * Method add
         *
         * @param dsakeyvalue
         */
        public void add(DSAKeyValue dsakeyvalue) {
            this.add(new KeyValue(getDocument(), dsakeyvalue));
        }

        /**
         * Method add
         *
         * @param rsakeyvalue
         */
        public void add(RSAKeyValue rsakeyvalue) {
            this.add(new KeyValue(getDocument(), rsakeyvalue));
        }

        /**
         * Method add
         *
         * @param pk
         */
        public void add(PublicKey pk) {
            this.add(new KeyValue(getDocument(), pk));
        }

        /**
         * Method add
         *
         * @param keyvalue
         */
        public void add(KeyValue keyvalue) {
            appendSelf(keyvalue);
            addReturnToSelf();
        }

        /**
         * Method addMgmtData
         *
         * @param mgmtdata
         */
        public void addMgmtData(String mgmtdata) {
            this.add(new MgmtData(getDocument(), mgmtdata));
        }

        /**
         * Method add
         *
         * @param mgmtdata
         */
        public void add(MgmtData mgmtdata) {
            appendSelf(mgmtdata);
            addReturnToSelf();
        }

        /**
         * Method addPGPData
         *
         * @param pgpdata
         */
        public void add(PGPData pgpdata) {
            appendSelf(pgpdata);
            addReturnToSelf();
        }

        /**
         * Method addRetrievalMethod
         *
         * @param uri
         * @param transforms
         * @param Type  
         */
        public void addRetrievalMethod(String uri, Transforms transforms, String Type) {
            this.add(new RetrievalMethod(getDocument(), uri, transforms, Type));
        }

        /**
         * Method add
         *
         * @param retrievalmethod
         */
        public void add(RetrievalMethod retrievalmethod) {
            appendSelf(retrievalmethod);
            addReturnToSelf();
        }

        /**
         * Method add
         *
         * @param spkidata
         */
        public void add(SPKIData spkidata) {
            appendSelf(spkidata);
            addReturnToSelf();
        }

        /**
         * Method addX509Data
         *
         * @param x509data
         */
        public void add(X509Data x509data) {
            if (x509Datas == null) {
                x509Datas = new ArrayList<X509Data>();
            }
            x509Datas.add(x509data);
            appendSelf(x509data);
            addReturnToSelf();
        }

        /**
         * Method addEncryptedKey
         *
         * @param encryptedKey
         * @throws XMLEncryptionException
         */

        public void add(EncryptedKey encryptedKey) throws XMLEncryptionException {
            if (encryptedKeys == null) {
                encryptedKeys = new ArrayList<EncryptedKey>();
            }
            encryptedKeys.add(encryptedKey);
            XMLCipher cipher = XMLCipher.getInstance();
            appendSelf(cipher.martial(encryptedKey));
        }
    
        /**
         * Method addDEREncodedKeyValue
         *
         * @param pk
         * @throws XMLSecurityException 
         */
        public void addDEREncodedKeyValue(PublicKey pk) throws XMLSecurityException {
            this.add(new DEREncodedKeyValue(getDocument(), pk));
        }

        /**
         * Method add
         *
         * @param derEncodedKeyValue
         */
        public void add(DEREncodedKeyValue derEncodedKeyValue) {
            appendSelf(derEncodedKeyValue);
            addReturnToSelf();
        }
    
        /**
         * Method addKeyInfoReference
         *
         * @param URI
         * @throws XMLSecurityException 
         */
        public void addKeyInfoReference(String URI) throws XMLSecurityException {
            this.add(new KeyInfoReference(getDocument(), URI));
        }

        /**
         * Method add
         *
         * @param keyInfoReference
         */
        public void add(KeyInfoReference keyInfoReference) {
            appendSelf(keyInfoReference);
            addReturnToSelf();
        }

        /**
         * Method addUnknownElement
         *
         * @param element
         */
        public void addUnknownElement(Element element) {
            appendSelf(element);
            addReturnToSelf();
        }

        /**
         * Method lengthKeyName
         *
         * @return the number of the KeyName tags
         */
        public int lengthKeyName() {
            return this.length(Constants.SignatureSpecNS, Constants._TAG_KEYNAME);
        }

        /**
         * Method lengthKeyValue
         *
         *@return the number of the KeyValue tags
         */
        public int lengthKeyValue() {
            return this.length(Constants.SignatureSpecNS, Constants._TAG_KEYVALUE);
        }

        /**
         * Method lengthMgmtData
         *
         *@return the number of the MgmtData tags
         */
        public int lengthMgmtData() {
            return this.length(Constants.SignatureSpecNS, Constants._TAG_MGMTDATA);
        }

        /**
         * Method lengthPGPData
         *
         *@return the number of the PGPDat. tags
         */
        public int lengthPGPData() {
            return this.length(Constants.SignatureSpecNS, Constants._TAG_PGPDATA);
        }

        /**
         * Method lengthRetrievalMethod
         *
         *@return the number of the RetrievalMethod tags
         */
        public int lengthRetrievalMethod() {
            return this.length(Constants.SignatureSpecNS, Constants._TAG_RETRIEVALMETHOD);
        }

        /**
         * Method lengthSPKIData
         *
         *@return the number of the SPKIData tags
         */
        public int lengthSPKIData() {
            return this.length(Constants.SignatureSpecNS, Constants._TAG_SPKIDATA);
        }

        /**
         * Method lengthX509Data
         *
         *@return the number of the X509Data tags
         */
        public int lengthX509Data() {
            if (x509Datas != null) {
                return x509Datas.size(); 
            }
            return this.length(Constants.SignatureSpecNS, Constants._TAG_X509DATA);
        }

        /**
         * Method lengthDEREncodedKeyValue
         *
         *@return the number of the DEREncodedKeyValue tags
         */
        public int lengthDEREncodedKeyValue() {
            return this.length(Constants.SignatureSpec11NS, Constants._TAG_DERENCODEDKEYVALUE);
        }

        /**
         * Method lengthKeyInfoReference
         *
         *@return the number of the KeyInfoReference tags
         */
        public int lengthKeyInfoReference() {
            return this.length(Constants.SignatureSpec11NS, Constants._TAG_KEYINFOREFERENCE);
        }

        /**
         * Method lengthUnknownElement
         * NOTE possibly buggy.
         * @return the number of the UnknownElement tags
         */
        public int lengthUnknownElement() {
            int res = 0;
            Node childNode = getElement().getFirstChild();
            while (childNode != null) {
                /**
                 * $todo$ using this method, we don't see unknown Elements
                 *  from Signature NS; revisit
                 */
                if (childNode.getNodeType() == Node.ELEMENT_NODE
                    && childNode.getNamespaceURI().equals(Constants.SignatureSpecNS)) {
                    res++;
                }
                childNode = childNode.getNextSibling();
            }
        
            return res;
        }

        /**
         * Method itemKeyName
         *
         * @param i
         * @return the asked KeyName element, null if the index is too big
         * @throws XMLSecurityException
         */
        public KeyName itemKeyName(int i) throws XMLSecurityException {
            Element e = 
                XMLUtils.selectDsNode(
                    getFirstChild(), Constants._TAG_KEYNAME, i);

            if (e != null) {
                return new KeyName(e, this.baseURI);
            } 
            return null;      
        }

        /**
         * Method itemKeyValue
         *
         * @param i
         * @return the asked KeyValue element, null if the index is too big
         * @throws XMLSecurityException
         */
        public KeyValue itemKeyValue(int i) throws XMLSecurityException {
            Element e = 
                XMLUtils.selectDsNode(
                    getFirstChild(), Constants._TAG_KEYVALUE, i);

            if (e != null) {
                return new KeyValue(e, this.baseURI);
            } 
            return null;      
        }

        /**
         * Method itemMgmtData
         *
         * @param i
         * @return the asked MgmtData element, null if the index is too big
         * @throws XMLSecurityException
         */
        public MgmtData itemMgmtData(int i) throws XMLSecurityException {
            Element e = 
                XMLUtils.selectDsNode(
                    getFirstChild(), Constants._TAG_MGMTDATA, i);

            if (e != null) {
                return new MgmtData(e, this.baseURI);
            } 
            return null;      
        }

        /**
         * Method itemPGPData
         *
         * @param i
         * @return the asked PGPData element, null if the index is too big
         * @throws XMLSecurityException
         */
        public PGPData itemPGPData(int i) throws XMLSecurityException {
            Element e = 
                XMLUtils.selectDsNode(
                    getFirstChild(), Constants._TAG_PGPDATA, i);

            if (e != null) {
                return new PGPData(e, this.baseURI);
            } 
            return null;      
        }

        /**
         * Method itemRetrievalMethod
         *
         * @param i
         *@return the asked RetrievalMethod element, null if the index is too big
         * @throws XMLSecurityException
         */
        public RetrievalMethod itemRetrievalMethod(int i) throws XMLSecurityException {
            Element e = 
                XMLUtils.selectDsNode(
                    getFirstChild(), Constants._TAG_RETRIEVALMETHOD, i);

            if (e != null) {
                return new RetrievalMethod(e, this.baseURI);
            } 
            return null;
        }

        /**
         * Method itemSPKIData
         *
         * @param i
         * @return the asked SPKIData element, null if the index is too big
         * @throws XMLSecurityException
         */
        public SPKIData itemSPKIData(int i) throws XMLSecurityException {
            Element e = 
                XMLUtils.selectDsNode(
                    getFirstChild(), Constants._TAG_SPKIDATA, i);

            if (e != null) {
                return new SPKIData(e, this.baseURI);
            } 
            return null;     
        }
    
        /**
         * Method itemX509Data
         * 
         * @param i
         * @return the asked X509Data element, null if the index is too big
         * @throws XMLSecurityException
         */
        public X509Data itemX509Data(int i) throws XMLSecurityException {
            if (x509Datas != null) {
                return x509Datas.get(i); 
            }
            Element e = 
                XMLUtils.selectDsNode(
                    getFirstChild(), Constants._TAG_X509DATA, i);

            if (e != null) {
                return new X509Data(e, this.baseURI);
            } 
            return null;
        }

        /**
         * Method itemEncryptedKey
         *
         * @param i
         * @return the asked EncryptedKey element, null if the index is too big
         * @throws XMLSecurityException
         */
        public EncryptedKey itemEncryptedKey(int i) throws XMLSecurityException {
            if (encryptedKeys != null) {
                return encryptedKeys.get(i);
            }
            Element e = 
                XMLUtils.selectXencNode(
                    getFirstChild(), EncryptionConstants._TAG_ENCRYPTEDKEY, i);

            if (e != null) {
                XMLCipher cipher = XMLCipher.getInstance();
                cipher.init(XMLCipher.UNWRAP_MODE, null);
                return cipher.loadEncryptedKey(e);
            }
            return null;
        }
    
        /**
         * Method itemDEREncodedKeyValue
         *
         * @param i
         * @return the asked DEREncodedKeyValue element, null if the index is too big
         * @throws XMLSecurityException
         */
        public DEREncodedKeyValue itemDEREncodedKeyValue(int i) throws XMLSecurityException {
            Element e = 
                XMLUtils.selectDs11Node(
                    getFirstChild(), Constants._TAG_DERENCODEDKEYVALUE, i);

            if (e != null) {
                return new DEREncodedKeyValue(e, this.baseURI);
            } 
            return null;     
        }

        /**
         * Method itemKeyInfoReference
         *
         * @param i
         * @return the asked KeyInfoReference element, null if the index is too big
         * @throws XMLSecurityException
         */
        public KeyInfoReference itemKeyInfoReference(int i) throws XMLSecurityException {
            Element e = 
                XMLUtils.selectDs11Node(
                    getFirstChild(), Constants._TAG_KEYINFOREFERENCE, i);

            if (e != null) {
                return new KeyInfoReference(e, this.baseURI);
            } 
            return null;     
        }

        /**
         * Method itemUnknownElement
         *
         * @param i index
         * @return the element number of the unknown elements
         */
        public Element itemUnknownElement(int i) {
            int res = 0;
            Node childNode = getElement().getFirstChild();
            while (childNode != null) {
                /**
                 * $todo$ using this method, we don't see unknown Elements
                 *  from Signature NS; revisit
                 */
                if (childNode.getNodeType() == Node.ELEMENT_NODE
                    && childNode.getNamespaceURI().equals(Constants.SignatureSpecNS)) {
                    res++;

                    if (res == i) {
                        return (Element) childNode;
                    }
                }
                childNode = childNode.getNextSibling();
            }

            return null;
        }

        /**
         * Method isEmpty
         *
         * @return true if the element has no descendants.
         */
        public boolean isEmpty() {
            return getFirstChild() == null;
        }

        /**
         * Method containsKeyName
         *
         * @return If the KeyInfo contains a KeyName node
         */
        public boolean containsKeyName() {
            return this.lengthKeyName() > 0;
        }

        /**
         * Method containsKeyValue
         *
         * @return If the KeyInfo contains a KeyValue node
         */
        public boolean containsKeyValue() {
            return this.lengthKeyValue() > 0;
        }

        /**
         * Method containsMgmtData
         *
         * @return If the KeyInfo contains a MgmtData node
         */
        public boolean containsMgmtData() {
            return this.lengthMgmtData() > 0;
        }

        /**
         * Method containsPGPData
         *
         * @return If the KeyInfo contains a PGPData node
         */
        public boolean containsPGPData() {
            return this.lengthPGPData() > 0;
        }

        /**
         * Method containsRetrievalMethod
         *
         * @return If the KeyInfo contains a RetrievalMethod node
         */
        public boolean containsRetrievalMethod() {
            return this.lengthRetrievalMethod() > 0;
        }

        /**
         * Method containsSPKIData
         *
         * @return If the KeyInfo contains a SPKIData node
         */
        public boolean containsSPKIData() {
            return this.lengthSPKIData() > 0;
        }

        /**
         * Method containsUnknownElement
         *
         * @return If the KeyInfo contains a UnknownElement node
         */
        public boolean containsUnknownElement() {
            return this.lengthUnknownElement() > 0;
        }

        /**
         * Method containsX509Data
         *
         * @return If the KeyInfo contains a X509Data node
         */
        public boolean containsX509Data() {
            return this.lengthX509Data() > 0;
        }
    
        /**
         * Method containsDEREncodedKeyValue
         *
         * @return If the KeyInfo contains a DEREncodedKeyValue node
         */
        public boolean containsDEREncodedKeyValue() {
            return this.lengthDEREncodedKeyValue() > 0;
        }

        /**
         * Method containsKeyInfoReference
         *
         * @return If the KeyInfo contains a KeyInfoReference node
         */
        public boolean containsKeyInfoReference() {
            return this.lengthKeyInfoReference() > 0;
        }
    
        /**
         * This method returns the public key.
         *
         * @return If the KeyInfo contains a PublicKey node
         * @throws KeyResolverException
         */
        public PublicKey getPublicKey() throws KeyResolverException {
            PublicKey pk = this.getPublicKeyFromInternalResolvers();

            if (pk != null) {
                if (log.isDebugEnabled()) {
                    log.debug("I could find a key using the per-KeyInfo key resolvers");
                }

                return pk;
            }
            if (log.isDebugEnabled()) {
                log.debug("I couldn't find a key using the per-KeyInfo key resolvers");
            }

            pk = this.getPublicKeyFromStaticResolvers();

            if (pk != null) {
                if (log.isDebugEnabled()) {
                    log.debug("I could find a key using the system-wide key resolvers");
                }

                return pk;
            }
            if (log.isDebugEnabled()) {
                log.debug("I couldn't find a key using the system-wide key resolvers");
            }

            return null;
        }

        /**
         * Searches the library wide KeyResolvers for public keys
         *
         * @return The public key contained in this Node.
         * @throws KeyResolverException
         */
        PublicKey getPublicKeyFromStaticResolvers() throws KeyResolverException {
            Iterator<KeyResolverSpi> it = KeyResolver.iterator();
            while (it.hasNext()) {
                KeyResolverSpi keyResolver = it.next();
                keyResolver.setSecureValidation(secureValidation);
                Node currentChild = getFirstChild();
                String uri = this.getBaseURI();
                while (currentChild != null) {       
                    if (currentChild.getNodeType() == Node.ELEMENT_NODE) {
                        for (StorageResolver storage : storageResolvers) {
                            PublicKey pk =
                                keyResolver.engineLookupAndResolvePublicKey(
                                    (Element) currentChild, uri, storage
                                );

                            if (pk != null) {
                                return pk;
                            }                     
                        }                              
                    }
                    currentChild = currentChild.getNextSibling();
                }      
            }
            return null;
        }

        /**
         * Searches the per-KeyInfo KeyResolvers for public keys
         *
         * @return The public key contained in this Node.
         * @throws KeyResolverException
         */
        PublicKey getPublicKeyFromInternalResolvers() throws KeyResolverException {
            for (KeyResolverSpi keyResolver : internalKeyResolvers) {
                if (log.isDebugEnabled()) {
                    log.debug("Try " + keyResolver.getClass().getName());
                }
                keyResolver.setSecureValidation(secureValidation);
                Node currentChild = getFirstChild();
                String uri = this.getBaseURI();
                while (currentChild != null)      {    
                    if (currentChild.getNodeType() == Node.ELEMENT_NODE) {            
                        for (StorageResolver storage : storageResolvers) {
                            PublicKey pk = 
                                keyResolver.engineLookupAndResolvePublicKey(
                                    (Element) currentChild, uri, storage
                                );

                            if (pk != null) {
                                return pk;
                            }                     
                        }               
                    }
                    currentChild = currentChild.getNextSibling();
                }
            }

            return null;
        }

        /**
         * Method getX509Certificate
         *
         * @return The certificate contained in this KeyInfo
         * @throws KeyResolverException
         */
        public X509Certificate getX509Certificate() throws KeyResolverException {
            // First search using the individual resolvers from the user
            X509Certificate cert = this.getX509CertificateFromInternalResolvers();

            if (cert != null) {
                if (log.isDebugEnabled()) {
                    log.debug("I could find a X509Certificate using the per-KeyInfo key resolvers");
                }

                return cert;
            }
            if (log.isDebugEnabled()) {
                log.debug("I couldn't find a X509Certificate using the per-KeyInfo key resolvers");
            }

            // Then use the system-wide Resolvers
            cert = this.getX509CertificateFromStaticResolvers();

            if (cert != null) {
                if (log.isDebugEnabled()) {
                    log.debug("I could find a X509Certificate using the system-wide key resolvers");
                }

                return cert;
            } 
            if (log.isDebugEnabled()) {
                log.debug("I couldn't find a X509Certificate using the system-wide key resolvers");
            }

            return null;
        }

        /**
         * This method uses each System-wide {@link KeyResolver} to search the
         * child elements. Each combination of {@link KeyResolver} and child element
         * is checked against all {@link StorageResolver}s.
         *
         * @return The certificate contained in this KeyInfo
         * @throws KeyResolverException
         */
        X509Certificate getX509CertificateFromStaticResolvers()
            throws KeyResolverException {
            if (log.isDebugEnabled()) {
                log.debug(
                    "Start getX509CertificateFromStaticResolvers() with " + KeyResolver.length() 
                    + " resolvers"
                );
            }
            String uri = this.getBaseURI();
            Iterator<KeyResolverSpi> it = KeyResolver.iterator();
            while (it.hasNext()) {
                KeyResolverSpi keyResolver = it.next();
                keyResolver.setSecureValidation(secureValidation);
                X509Certificate cert = applyCurrentResolver(uri, keyResolver);
                if (cert != null) {
                    return cert;
                }
            }
            return null;
        }

        private X509Certificate applyCurrentResolver(
            String uri, KeyResolverSpi keyResolver
        ) throws KeyResolverException {
            Node currentChild = getFirstChild();
            while (currentChild != null)      {       
                if (currentChild.getNodeType() == Node.ELEMENT_NODE) {
                    for (StorageResolver storage : storageResolvers) {
                        X509Certificate cert = 
                            keyResolver.engineLookupResolveX509Certificate(
                                (Element) currentChild, uri, storage
                            );

                        if (cert != null) {                	   
                            return cert;
                        }                  
                    }               
                }
                currentChild = currentChild.getNextSibling();
            }
            return null;
        }

        /**
         * Method getX509CertificateFromInternalResolvers
         *
         * @return The certificate contained in this KeyInfo
         * @throws KeyResolverException
         */
        X509Certificate getX509CertificateFromInternalResolvers()
            throws KeyResolverException {
            if (log.isDebugEnabled()) {
                log.debug(
                    "Start getX509CertificateFromInternalResolvers() with " 
                    + this.lengthInternalKeyResolver() + " resolvers"
                );
            }
            String uri = this.getBaseURI();
            for (KeyResolverSpi keyResolver : internalKeyResolvers) {
                if (log.isDebugEnabled()) {
                    log.debug("Try " + keyResolver.getClass().getName());
                }
                keyResolver.setSecureValidation(secureValidation);
                X509Certificate cert = applyCurrentResolver(uri, keyResolver);
                if (cert != null) {        	
                    return cert;
                }      
            }

            return null;
        }

        /**
         * This method returns a secret (symmetric) key. This is for XML Encryption.
         * @return the secret key contained in this KeyInfo
         * @throws KeyResolverException
         */
        public SecretKey getSecretKey() throws KeyResolverException {
            SecretKey sk = this.getSecretKeyFromInternalResolvers();

            if (sk != null) {
                if (log.isDebugEnabled()) {
                    log.debug("I could find a secret key using the per-KeyInfo key resolvers");
                }

                return sk;
            }
            if (log.isDebugEnabled()) {
                log.debug("I couldn't find a secret key using the per-KeyInfo key resolvers");
            }

            sk = this.getSecretKeyFromStaticResolvers();

            if (sk != null) {
                if (log.isDebugEnabled()) {
                    log.debug("I could find a secret key using the system-wide key resolvers");
                }

                return sk;
            }
            if (log.isDebugEnabled()) {
                log.debug("I couldn't find a secret key using the system-wide key resolvers");
            }

            return null;
        }

        /**
         * Searches the library wide KeyResolvers for Secret keys
         *
         * @return the secret key contained in this KeyInfo
         * @throws KeyResolverException
         */
        SecretKey getSecretKeyFromStaticResolvers() throws KeyResolverException {
            Iterator<KeyResolverSpi> it = KeyResolver.iterator();
            while (it.hasNext()) {
                KeyResolverSpi keyResolver = it.next();
                keyResolver.setSecureValidation(secureValidation);

                Node currentChild = getFirstChild();
                String uri = this.getBaseURI();
                while (currentChild != null)      {    
                    if (currentChild.getNodeType() == Node.ELEMENT_NODE) {
                        for (StorageResolver storage : storageResolvers) {
                            SecretKey sk =
                                keyResolver.engineLookupAndResolveSecretKey(
                                    (Element) currentChild, uri, storage
                                );

                            if (sk != null) {
                                return sk;
                            }                     
                        }               
                    }
                    currentChild = currentChild.getNextSibling();
                }
            }
            return null;
        }

        /**
         * Searches the per-KeyInfo KeyResolvers for secret keys
         *
         * @return the secret key contained in this KeyInfo
         * @throws KeyResolverException
         */

        SecretKey getSecretKeyFromInternalResolvers() throws KeyResolverException {
            for (KeyResolverSpi keyResolver : internalKeyResolvers) {
                if (log.isDebugEnabled()) {
                    log.debug("Try " + keyResolver.getClass().getName());
                }
                keyResolver.setSecureValidation(secureValidation);
                Node currentChild = getFirstChild();
                String uri = this.getBaseURI();
                while (currentChild != null)      {    
                    if (currentChild.getNodeType() == Node.ELEMENT_NODE) {
                        for (StorageResolver storage : storageResolvers) {
                            SecretKey sk = 
                                keyResolver.engineLookupAndResolveSecretKey(
                                    (Element) currentChild, uri, storage
                                );

                            if (sk != null) {
                                return sk;
                            }                    
                        }
                    }            
                    currentChild = currentChild.getNextSibling();
                }
            }

            return null;
        }

        /**
         * This method returns a private key. This is for Key Transport in XML Encryption.
         * @return the private key contained in this KeyInfo
         * @throws KeyResolverException
         */
        public PrivateKey getPrivateKey() throws KeyResolverException {
            PrivateKey pk = this.getPrivateKeyFromInternalResolvers();

            if (pk != null) {
                if (log.isDebugEnabled()) {
                    log.debug("I could find a private key using the per-KeyInfo key resolvers");
                }
                return pk;
            }
            if (log.isDebugEnabled()) {
                log.debug("I couldn't find a secret key using the per-KeyInfo key resolvers");
            }

            pk = this.getPrivateKeyFromStaticResolvers();
            if (pk != null) {
                if (log.isDebugEnabled()) {
                    log.debug("I could find a private key using the system-wide key resolvers");
                }
                return pk;
            }
            if (log.isDebugEnabled()) {
                log.debug("I couldn't find a private key using the system-wide key resolvers");
            }

            return null;
        }

        /**
         * Searches the library wide KeyResolvers for Private keys
         *
         * @return the private key contained in this KeyInfo
         * @throws KeyResolverException
         */
        PrivateKey getPrivateKeyFromStaticResolvers() throws KeyResolverException {
            Iterator<KeyResolverSpi> it = KeyResolver.iterator();
            while (it.hasNext()) {
                KeyResolverSpi keyResolver = it.next();
                keyResolver.setSecureValidation(secureValidation);

                Node currentChild = getFirstChild();
                String uri = this.getBaseURI();
                while (currentChild != null)      {    
                    if (currentChild.getNodeType() == Node.ELEMENT_NODE) {
                        // not using StorageResolvers at the moment
                        // since they cannot return private keys
                        PrivateKey pk = 
                            keyResolver.engineLookupAndResolvePrivateKey(
                                (Element) currentChild, uri, null
                            );

                        if (pk != null) {
                            return pk;
                        }                     
                    }
                    currentChild = currentChild.getNextSibling();
                }
            }
            return null;
        }

        /**
         * Searches the per-KeyInfo KeyResolvers for private keys
         *
         * @return the private key contained in this KeyInfo
         * @throws KeyResolverException
         */
        PrivateKey getPrivateKeyFromInternalResolvers() throws KeyResolverException {
            for (KeyResolverSpi keyResolver : internalKeyResolvers) {
                if (log.isDebugEnabled()) {
                    log.debug("Try " + keyResolver.getClass().getName());
                }
                keyResolver.setSecureValidation(secureValidation);
                Node currentChild = getFirstChild();
                String uri = this.getBaseURI();
                while (currentChild != null) {    
                    if (currentChild.getNodeType() == Node.ELEMENT_NODE) {
                        // not using StorageResolvers at the moment
                        // since they cannot return private keys
                        PrivateKey pk = 
                            keyResolver.engineLookupAndResolvePrivateKey(
                                (Element) currentChild, uri, null
                            );

                        if (pk != null) {
                            return pk;
                        }                    
                    }            
                    currentChild = currentChild.getNextSibling();
                }
            }

            return null;
        }

        /**
         * This method is used to add a custom {@link KeyResolverSpi} to a KeyInfo
         * object.
         *
         * @param realKeyResolver
         */
        public void registerInternalKeyResolver(KeyResolverSpi realKeyResolver) {
            this.internalKeyResolvers.add(realKeyResolver);
        }

        /**
         * Method lengthInternalKeyResolver
         * @return the length of the key
         */
        int lengthInternalKeyResolver() {
            return this.internalKeyResolvers.size();
        }

        /**
         * Method itemInternalKeyResolver
         *
         * @param i the index
         * @return the KeyResolverSpi for the index.
         */
        KeyResolverSpi itemInternalKeyResolver(int i) {
            return this.internalKeyResolvers.get(i);
        }

        /**
         * Method addStorageResolver
         *
         * @param storageResolver
         */
        public void addStorageResolver(StorageResolver storageResolver) {
            if (storageResolvers == nullList) {
                // Replace the default null StorageResolver
                storageResolvers = new ArrayList<StorageResolver>();
            }      
            this.storageResolvers.add(storageResolver);
        }


        /** @inheritDoc */
        public String getBaseLocalName() 
        {
            return Constants._TAG_KEYINFO;
        }
    }
}
