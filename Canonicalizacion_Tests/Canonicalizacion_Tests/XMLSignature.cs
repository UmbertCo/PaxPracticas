﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Canonicalizacion_Tests
{
    class XMLSignature : SignatureElementProxy
    {
        /** ds:Signature.ds:SignedInfo element */
        private SignedInfo signedInfo;

        /** ds:Signature.ds:KeyInfo */
        private KeyInfo keyInfo;

        /**
         * Checking the digests in References in a Signature are mandatory, but for
         * References inside a Manifest it is application specific. This boolean is
         * to indicate that the References inside Manifests should be validated.
         */
        private bool followManifestsDuringValidation = false;

        private XmlElement signatureValueElement;

        private static const int MODE_SIGN = 0;
        private static const int MODE_VERIFY = 1;
        private int state = MODE_SIGN;

        /**
         * This creates a new <CODE>ds:Signature</CODE> Element and adds an empty
         * <CODE>ds:SignedInfo</CODE>.
         * The <code>ds:SignedInfo</code> is initialized with the specified Signature
         * algorithm and Canonicalizer.ALGO_ID_C14N_OMIT_COMMENTS which is REQUIRED
         * by the spec. This method's main use is for creating a new signature.
         *
         * @param doc Document in which the signature will be appended after creation.
         * @param baseURI URI to be used as context for all relative URIs.
         * @param signatureMethodURI signature algorithm to use.
         * @throws XMLSecurityException
         */
        public XMLSignature(XmlDocument doc, String baseURI, String signatureMethodURI)
        {
            this(doc, baseURI, signatureMethodURI, 0, Canonicalizacion.ALGO_ID_C14N_OMIT_COMMENTS);
        }

        /**
         * Constructor XMLSignature
         *
         * @param doc
         * @param baseURI
         * @param signatureMethodURI the Signature method to be used.
         * @param hmacOutputLength
         * @throws XMLSecurityException
         */
        public XMLSignature(XmlDocument doc, String baseURI, String signatureMethodURI, int hmacOutputLength)
        {
            this(doc, baseURI, signatureMethodURI, hmacOutputLength, Canonicalizacion.ALGO_ID_C14N_OMIT_COMMENTS);
        }

        /**
         * Constructor XMLSignature
         *
         * @param doc
         * @param baseURI
         * @param signatureMethodURI the Signature method to be used.
         * @param canonicalizationMethodURI the canonicalization algorithm to be 
         * used to c14nize the SignedInfo element.
         * @throws XMLSecurityException
         */
        public XMLSignature(
            Document doc, 
            String baseURI, 
            String signatureMethodURI,
            String canonicalizationMethodURI
        ) throws XMLSecurityException {
            this(doc, baseURI, signatureMethodURI, 0, canonicalizationMethodURI);
        }

        /**
         * Constructor XMLSignature
         *
         * @param doc
         * @param baseURI
         * @param signatureMethodURI
         * @param hmacOutputLength
         * @param canonicalizationMethodURI
         * @throws XMLSecurityException
         */
        public XMLSignature(
            Document doc, 
            String baseURI, 
            String signatureMethodURI,
            int hmacOutputLength, 
            String canonicalizationMethodURI
        ) throws XMLSecurityException {
            super(doc);

            String xmlnsDsPrefix = getDefaultPrefix(Constants.SignatureSpecNS);
            if (xmlnsDsPrefix == null || xmlnsDsPrefix.length() == 0) {
                getElement().setAttributeNS(
                    Constants.NamespaceSpecNS, "xmlns", Constants.SignatureSpecNS
                );
            } else {
                getElement().setAttributeNS(
                    Constants.NamespaceSpecNS, "xmlns:" + xmlnsDsPrefix, Constants.SignatureSpecNS
                );
            }
            addReturnToSelf();

            this.baseURI = baseURI;
            this.signedInfo = 
                new SignedInfo(
                    getDocument(), signatureMethodURI, hmacOutputLength, canonicalizationMethodURI
                );

            appendSelf(this.signedInfo);
            addReturnToSelf();

            // create an empty SignatureValue; this is filled by setSignatureValueElement
            signatureValueElement = 
                XMLUtils.createElementInSignatureSpace(getDocument(), Constants._TAG_SIGNATUREVALUE);

            appendSelf(signatureValueElement);
            addReturnToSelf();
        }

        /**
         *  Creates a XMLSignature in a Document
         * @param doc
         * @param baseURI
         * @param SignatureMethodElem
         * @param CanonicalizationMethodElem
         * @throws XMLSecurityException
         */
        public XMLSignature(
            Document doc, 
            String baseURI, 
            Element SignatureMethodElem, 
            Element CanonicalizationMethodElem
        ) throws XMLSecurityException {
            super(doc);

            String xmlnsDsPrefix = getDefaultPrefix(Constants.SignatureSpecNS);
            if (xmlnsDsPrefix == null || xmlnsDsPrefix.length() == 0) {
                getElement().setAttributeNS(
                    Constants.NamespaceSpecNS, "xmlns", Constants.SignatureSpecNS
                );
            } else {
                getElement().setAttributeNS(
                    Constants.NamespaceSpecNS, "xmlns:" + xmlnsDsPrefix, Constants.SignatureSpecNS
                );
            }
            addReturnToSelf();

            this.baseURI = baseURI;
            this.signedInfo = 
                new SignedInfo(getDocument(), SignatureMethodElem, CanonicalizationMethodElem);

            appendSelf(this.signedInfo);
            addReturnToSelf();

            // create an empty SignatureValue; this is filled by setSignatureValueElement
            signatureValueElement = 
                XMLUtils.createElementInSignatureSpace(getDocument(), Constants._TAG_SIGNATUREVALUE);

            appendSelf(signatureValueElement);
            addReturnToSelf();
        }
    
        /**
         * This will parse the element and construct the Java Objects.
         * That will allow a user to validate the signature.
         *
         * @param element ds:Signature element that contains the whole signature
         * @param baseURI URI to be prepended to all relative URIs
         * @throws XMLSecurityException
         * @throws XMLSignatureException if the signature is badly formatted
         */
        public XMLSignature(Element element, String baseURI)
            throws XMLSignatureException, XMLSecurityException {
            this(element, baseURI, false);
        }

        /**
         * This will parse the element and construct the Java Objects.
         * That will allow a user to validate the signature.
         *
         * @param element ds:Signature element that contains the whole signature
         * @param baseURI URI to be prepended to all relative URIs
         * @param secureValidation whether secure secureValidation is enabled or not
         * @throws XMLSecurityException
         * @throws XMLSignatureException if the signature is badly formatted
         */
        public XMLSignature(Element element, String baseURI, boolean secureValidation)
            throws XMLSignatureException, XMLSecurityException {
            super(element, baseURI);

            // check out SignedInfo child
            Element signedInfoElem = XMLUtils.getNextElement(element.getFirstChild());

            // check to see if it is there
            if (signedInfoElem == null) {
                Object exArgs[] = { Constants._TAG_SIGNEDINFO, Constants._TAG_SIGNATURE };
                throw new XMLSignatureException("xml.WrongContent", exArgs);
            }

            // create a SignedInfo object from that element
            this.signedInfo = new SignedInfo(signedInfoElem, baseURI, secureValidation);
            // get signedInfoElem again in case it has changed
            signedInfoElem = XMLUtils.getNextElement(element.getFirstChild());

            // check out SignatureValue child
            this.signatureValueElement = 
                XMLUtils.getNextElement(signedInfoElem.getNextSibling());

            // check to see if it exists
            if (signatureValueElement == null) {
                Object exArgs[] = { Constants._TAG_SIGNATUREVALUE, Constants._TAG_SIGNATURE };
                throw new XMLSignatureException("xml.WrongContent", exArgs);
            }
            Attr signatureValueAttr = signatureValueElement.getAttributeNodeNS(null, "Id");
            if (signatureValueAttr != null) {
                signatureValueElement.setIdAttributeNode(signatureValueAttr, true);
            }

            // <element ref="ds:KeyInfo" minOccurs="0"/>
            Element keyInfoElem = 
                XMLUtils.getNextElement(signatureValueElement.getNextSibling());

            // If it exists use it, but it's not mandatory
            if (keyInfoElem != null 
                && Constants.SignatureSpecNS.equals(keyInfoElem.getNamespaceURI()) 
                && Constants._TAG_KEYINFO.equals(keyInfoElem.getLocalName())) {
                this.keyInfo = new KeyInfo(keyInfoElem, baseURI);
                this.keyInfo.setSecureValidation(secureValidation);
            }
        
            // <element ref="ds:Object" minOccurs="0" maxOccurs="unbounded"/>
            Element objectElem =
                XMLUtils.getNextElement(signatureValueElement.getNextSibling());
            while (objectElem != null) {
                Attr objectAttr = objectElem.getAttributeNodeNS(null, "Id");
                if (objectAttr != null) {
                    objectElem.setIdAttributeNode(objectAttr, true);
                }

                Node firstChild = objectElem.getFirstChild();
                // Register Ids of the Object child elements
                while (firstChild != null) {
                    if (firstChild.getNodeType() == Node.ELEMENT_NODE) {
                        Element childElem = (Element)firstChild;
                        String tag = childElem.getLocalName();
                        if (tag.equals("Manifest")) {
                            new Manifest(childElem, baseURI);
                        } else if (tag.equals("SignatureProperties")) {
                            new SignatureProperties(childElem, baseURI);
                        }
                    }
                    firstChild = firstChild.getNextSibling();
                }

                objectElem = XMLUtils.getNextElement(objectElem.getNextSibling());
            }
        
            this.state = MODE_VERIFY;
        }

        /**
         * Sets the <code>Id</code> attribute
         *
         * @param id Id value for the id attribute on the Signature Element
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
         * Returns the completely parsed <code>SignedInfo</code> object.
         *
         * @return the completely parsed <code>SignedInfo</code> object.
         */
        public SignedInfo getSignedInfo() {
            return this.signedInfo;
        }

        /**
         * Returns the octet value of the SignatureValue element.
         * Throws an XMLSignatureException if it has no or wrong content.
         *
         * @return the value of the SignatureValue element.
         * @throws XMLSignatureException If there is no content
         */
        public byte[] getSignatureValue() throws XMLSignatureException {
            try {
                return Base64.decode(signatureValueElement);
            } catch (Base64DecodingException ex) {
                throw new XMLSignatureException(ex, "empty");
            }
        }

        /**
         * Base64 encodes and sets the bytes as the content of the SignatureValue
         * Node.
         *
         * @param bytes bytes to be used by SignatureValue before Base64 encoding
         */
        private void setSignatureValueElement(byte[] bytes) {

            while (signatureValueElement.hasChildNodes()) {
                signatureValueElement.removeChild(signatureValueElement.getFirstChild());
            }

            String base64codedValue = Base64.encode(bytes);

            if (base64codedValue.length() > 76 && !XMLUtils.ignoreLineBreaks()) {
                base64codedValue = "\n" + base64codedValue + "\n";
            }

            Text t = createText(base64codedValue);
            signatureValueElement.appendChild(t);
        }

        /**
         * Returns the KeyInfo child. If we are in signing mode and the KeyInfo
         * does not exist yet, it is created on demand and added to the Signature.
         * <br>
         * This allows to add arbitrary content to the KeyInfo during signing.
         *
         * @return the KeyInfo object
         */
        public KeyInfo getKeyInfo() {
            // check to see if we are signing and if we have to create a keyinfo
            if (this.state == MODE_SIGN && this.keyInfo == null) {

                // create the KeyInfo
                this.keyInfo = new KeyInfo(getDocument());

                // get the Element from KeyInfo
                Element keyInfoElement = this.keyInfo.getElement();
                Element firstObject = 
                    XMLUtils.selectDsNode(
                        getElement().getFirstChild(), Constants._TAG_OBJECT, 0
                    );

                if (firstObject != null) {
                    // add it before the object
                    getElement().insertBefore(keyInfoElement, firstObject);
                    XMLUtils.addReturnBeforeChild(getElement(), firstObject);
                } else {
                    // add it as the last element to the signature
                    appendSelf(keyInfoElement);
                    addReturnToSelf();
                }         
            }

            return this.keyInfo;
        }

        /**
         * Appends an Object (not a <code>java.lang.Object</code> but an Object
         * element) to the Signature. Please note that this is only possible
         * when signing.
         *
         * @param object ds:Object to be appended.
         * @throws XMLSignatureException When this object is used to verify.
         */
        public void appendObject(ObjectContainer object) throws XMLSignatureException {
            //try {
            //if (this.state != MODE_SIGN) {
            // throw new XMLSignatureException(
            //  "signature.operationOnlyBeforeSign");
            //}

            appendSelf(object);
            addReturnToSelf();
            //} catch (XMLSecurityException ex) {
            // throw new XMLSignatureException(ex);
            //}
        }

        /**
         * Returns the <code>i<code>th <code>ds:Object</code> child of the signature
         * or null if no such <code>ds:Object</code> element exists.
         *
         * @param i
         * @return the <code>i<code>th <code>ds:Object</code> child of the signature
         * or null if no such <code>ds:Object</code> element exists.
         */
        public ObjectContainer getObjectItem(int i) {
            Element objElem = 
                XMLUtils.selectDsNode(
                    getFirstChild(), Constants._TAG_OBJECT, i
                );

            try {
                return new ObjectContainer(objElem, this.baseURI);
            } catch (XMLSecurityException ex) {
                return null;
            }
        }

        /**
         * Returns the number of all <code>ds:Object</code> elements.
         *
         * @return the number of all <code>ds:Object</code> elements.
         */
        public int getObjectLength() {
            return this.length(Constants.SignatureSpecNS, Constants._TAG_OBJECT);
        }

        /**
         * Digests all References in the SignedInfo, calculates the signature value 
         * and sets it in the SignatureValue Element.
         *
         * @param signingKey the {@link java.security.PrivateKey} or 
         * {@link javax.crypto.SecretKey} that is used to sign.
         * @throws XMLSignatureException
         */
        public void sign(Key signingKey) throws XMLSignatureException {

            if (signingKey instanceof PublicKey) {
                throw new IllegalArgumentException(
                    I18n.translate("algorithms.operationOnlyVerification")
                );
            }

            try {
                //Create a SignatureAlgorithm object
                SignedInfo si = this.getSignedInfo();
                SignatureAlgorithm sa = si.getSignatureAlgorithm();
                OutputStream so = null;
                try {
                    // initialize SignatureAlgorithm for signing
                    sa.initSign(signingKey);            

                    // generate digest values for all References in this SignedInfo
                    si.generateDigestValues();
                    so = new UnsyncBufferedOutputStream(new SignerOutputStream(sa));
                    // get the canonicalized bytes from SignedInfo
                    si.signInOctetStream(so);
                } catch (XMLSecurityException ex) {
                    throw ex;
                } finally {
                    if (so != null) {
                        try {
                            so.close();
                        } catch (IOException ex) {
                            if (log.isDebugEnabled()) {
                                log.debug(ex.getMessage(), ex);
                            }
                        }
                    }
                }

                // set them on the SignatureValue element
                this.setSignatureValueElement(sa.sign());
            } catch (XMLSignatureException ex) {
                throw ex;
            } catch (CanonicalizationException ex) {
                throw new XMLSignatureException(ex);
            } catch (InvalidCanonicalizerException ex) {
                throw new XMLSignatureException(ex);
            } catch (XMLSecurityException ex) {
                throw new XMLSignatureException(ex);
            }
        }

        /**
         * Adds a {@link ResourceResolver} to enable the retrieval of resources.
         *
         * @param resolver
         */
        public void addResourceResolver(ResourceResolver resolver) {
            this.getSignedInfo().addResourceResolver(resolver);
        }

        /**
         * Adds a {@link ResourceResolverSpi} to enable the retrieval of resources.
         *
         * @param resolver
         */
        public void addResourceResolver(ResourceResolverSpi resolver) {
            this.getSignedInfo().addResourceResolver(resolver);
        }

        /**
         * Extracts the public key from the certificate and verifies if the signature
         * is valid by re-digesting all References, comparing those against the
         * stored DigestValues and then checking to see if the Signatures match on
         * the SignedInfo.
         *
         * @param cert Certificate that contains the public key part of the keypair 
         * that was used to sign.
         * @return true if the signature is valid, false otherwise
         * @throws XMLSignatureException
         */
        public boolean checkSignatureValue(X509Certificate cert)
            throws XMLSignatureException {
            // see if cert is null
            if (cert != null) {
                // check the values with the public key from the cert
                return this.checkSignatureValue(cert.getPublicKey());
            } 

            Object exArgs[] = { "Didn't get a certificate" };
            throw new XMLSignatureException("empty", exArgs);
        }

        /**
         * Verifies if the signature is valid by redigesting all References,
         * comparing those against the stored DigestValues and then checking to see
         * if the Signatures match on the SignedInfo.
         *
         * @param pk {@link java.security.PublicKey} part of the keypair or 
         * {@link javax.crypto.SecretKey} that was used to sign
         * @return true if the signature is valid, false otherwise
         * @throws XMLSignatureException
         */
        public boolean checkSignatureValue(Key pk) throws XMLSignatureException {
            //COMMENT: pk suggests it can only be a public key?
            //check to see if the key is not null
            if (pk == null) {
                Object exArgs[] = { "Didn't get a key" };
                throw new XMLSignatureException("empty", exArgs);
            }
            // all references inside the signedinfo need to be dereferenced and
            // digested again to see if the outcome matches the stored value in the
            // SignedInfo.
            // If followManifestsDuringValidation is true it will do the same for
            // References inside a Manifest.
            try {
                SignedInfo si = this.getSignedInfo();
                //create a SignatureAlgorithms from the SignatureMethod inside
                //SignedInfo. This is used to validate the signature.
                SignatureAlgorithm sa = si.getSignatureAlgorithm();               
                if (log.isDebugEnabled()) {
                    log.debug("signatureMethodURI = " + sa.getAlgorithmURI());
                    log.debug("jceSigAlgorithm    = " + sa.getJCEAlgorithmString());
                    log.debug("jceSigProvider     = " + sa.getJCEProviderName());
                    log.debug("PublicKey = " + pk);
                }
                byte sigBytes[] = null;
                try {
                    sa.initVerify(pk);

                    // Get the canonicalized (normalized) SignedInfo
                    SignerOutputStream so = new SignerOutputStream(sa);
                    OutputStream bos = new UnsyncBufferedOutputStream(so);

                    si.signInOctetStream(bos);
                    bos.close();
                    // retrieve the byte[] from the stored signature
                    sigBytes = this.getSignatureValue();
                } catch (IOException ex) {
                    if (log.isDebugEnabled()) {
                        log.debug(ex.getMessage(), ex);
                    }
                    // Impossible...
                } catch (XMLSecurityException ex) {
                    throw ex;
                }

                // have SignatureAlgorithm sign the input bytes and compare them to 
                // the bytes that were stored in the signature.
                if (!sa.verify(sigBytes)) {
                    log.warn("Signature verification failed.");
                    return false;
                }

                return si.verify(this.followManifestsDuringValidation);
            } catch (XMLSignatureException ex) {
                throw ex;
            } catch (XMLSecurityException ex) {
                throw new XMLSignatureException(ex);
            } 
        }

        /**
         * Add a Reference with full parameters to this Signature
         *
         * @param referenceURI URI of the resource to be signed. Can be null in 
         * which case the dereferencing is application specific. Can be "" in which 
         * it's the parent node (or parent document?). There can only be one "" in 
         * each signature.
         * @param trans Optional list of transformations to be done before digesting
         * @param digestURI Mandatory URI of the digesting algorithm to use.
         * @param referenceId Optional id attribute for this Reference
         * @param referenceType Optional mimetype for the URI
         * @throws XMLSignatureException
         */
        public void addDocument(
            String referenceURI, 
            Transforms trans, 
            String digestURI, 
            String referenceId, 
            String referenceType
        ) throws XMLSignatureException {
            this.signedInfo.addDocument(
                this.baseURI, referenceURI, trans, digestURI, referenceId, referenceType
            );
        }

        /**
         * This method is a proxy method for the {@link Manifest#addDocument} method.
         *
         * @param referenceURI URI according to the XML Signature specification.
         * @param trans List of transformations to be applied.
         * @param digestURI URI of the digest algorithm to be used.
         * @see Manifest#addDocument
         * @throws XMLSignatureException
         */
        public void addDocument(
            String referenceURI, 
            Transforms trans, 
            String digestURI
        ) throws XMLSignatureException {
            this.signedInfo.addDocument(this.baseURI, referenceURI, trans, digestURI, null, null);
        }

        /**
         * Adds a Reference with just the URI and the transforms. This used the
         * SHA1 algorithm as a default digest algorithm.
         *
         * @param referenceURI URI according to the XML Signature specification.
         * @param trans List of transformations to be applied.
         * @throws XMLSignatureException
         */
        public void addDocument(String referenceURI, Transforms trans)
            throws XMLSignatureException {
            this.signedInfo.addDocument(
                this.baseURI, referenceURI, trans, Constants.ALGO_ID_DIGEST_SHA1, null, null
            );
        }

        /**
         * Add a Reference with just this URI. It uses SHA1 by default as the digest
         * algorithm
         *
         * @param referenceURI URI according to the XML Signature specification.
         * @throws XMLSignatureException
         */
        public void addDocument(String referenceURI) throws XMLSignatureException {
            this.signedInfo.addDocument(
                this.baseURI, referenceURI, null, Constants.ALGO_ID_DIGEST_SHA1, null, null
            );
        }

        /**
         * Add an X509 Certificate to the KeyInfo. This will include the whole cert
         * inside X509Data/X509Certificate tags.
         *
         * @param cert Certificate to be included. This should be the certificate of
         * the key that was used to sign.
         * @throws XMLSecurityException
         */
        public void addKeyInfo(X509Certificate cert) throws XMLSecurityException {
            X509Data x509data = new X509Data(getDocument());

            x509data.addCertificate(cert);
            this.getKeyInfo().add(x509data);
        }

        /**
         * Add this public key to the KeyInfo. This will include the complete key in
         * the KeyInfo structure.
         *
         * @param pk
         */
        public void addKeyInfo(PublicKey pk) {
            this.getKeyInfo().add(pk);
        }

        /**
         * Proxy method for {@link SignedInfo#createSecretKey(byte[])}. If you want 
         * to create a MAC, this method helps you to obtain the 
         * {@link javax.crypto.SecretKey} from octets.
         *
         * @param secretKeyBytes
         * @return the secret key created.
         * @see SignedInfo#createSecretKey(byte[])
         */
        public SecretKey createSecretKey(byte[] secretKeyBytes) {
            return this.getSignedInfo().createSecretKey(secretKeyBytes);
        }

        /**
         * Signal whether Manifest should be automatically validated.
         * Checking the digests in References in a Signature are mandatory, but for
         * References inside a Manifest it is application specific. This boolean is
         * to indicate that the References inside Manifests should be validated.
         *
         * @param followManifests
         * @see <a href="http://www.w3.org/TR/xmldsig-core/#sec-CoreValidation">
         * Core validation section in the XML Signature Rec.</a>
         */
        public void setFollowNestedManifests(boolean followManifests) {
            this.followManifestsDuringValidation = followManifests;
        }

        /**
         * Get the local name of this element
         *
         * @return Constants._TAG_SIGNATURE
         */
        public String getBaseLocalName() {
            return Constants._TAG_SIGNATURE;
        }
    }
}
