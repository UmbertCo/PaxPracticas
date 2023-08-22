using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canonicalizacion_Port
{
    class Constants
    {
         /** Field configurationFile */
        public const String configurationFile = "data/websig.conf";

        /** Field configurationFileNew */
        public const String configurationFileNew = ".xmlsecurityconfig";

        /** Field exceptionMessagesResourceBundleDir */
        public const String exceptionMessagesResourceBundleDir =
            "org/apache/xml/security/resource";

        /** Field exceptionMessagesResourceBundleBase is the location of the <CODE>ResourceBundle</CODE> */
        public const String exceptionMessagesResourceBundleBase =
            exceptionMessagesResourceBundleDir + "/" + "xmlsecurity";

        /**
         * The URL of the 
         * <A HREF="http://www.w3.org/TR/2001/CR-xmldsig-core-20010419/">XML Signature specification</A>
         */
        public const String SIGNATURESPECIFICATION_URL = 
            "http://www.w3.org/TR/2001/CR-xmldsig-core-20010419/";

        /**
         * The namespace of the 
         * <A HREF="http://www.w3.org/TR/2001/CR-xmldsig-core-20010419/">XML Signature specification</A>
         */
        public const String SignatureSpecNS = "http://www.w3.org/2000/09/xmldsig#";
    
        /**
         * The namespace of the 
         * <A HREF="http://www.w3.org/TR/xmldsig-core1/">XML Signature specification</A>
         */
        public const String SignatureSpec11NS = "http://www.w3.org/2009/xmldsig11#";
    
        /** The URL for more algorithms **/
        public const String MoreAlgorithmsSpecNS = "http://www.w3.org/2001/04/xmldsig-more#";
    
        /** The (newer) URL for more algorithms **/
        public const String XML_DSIG_NS_MORE_07_05 = "http://www.w3.org/2007/05/xmldsig-more#";
    
        /** The URI for XML spec*/
        public const String XML_LANG_SPACE_SpecNS = "http://www.w3.org/XML/1998/namespace";
    
        /** The URI for XMLNS spec*/
        public const String NamespaceSpecNS = "http://www.w3.org/2000/xmlns/";

        /** Tag of Attr Algorithm**/
        public const String _ATT_ALGORITHM = "Algorithm";
    
        /** Tag of Attr URI**/
        public const String _ATT_URI = "URI";
    
        /** Tag of Attr Type**/
        public const String _ATT_TYPE = "Type";
    
        /** Tag of Attr Id**/
        public const String _ATT_ID = "Id";
    
        /** Tag of Attr MimeType**/
        public const String _ATT_MIMETYPE = "MimeType";
    
        /** Tag of Attr Encoding**/
        public const String _ATT_ENCODING = "Encoding";
    
        /** Tag of Attr Target**/
        public const String _ATT_TARGET = "Target";

        // KeyInfo (KeyName|KeyValue|RetrievalMethod|X509Data|PGPData|SPKIData|MgmtData)
        // KeyValue (DSAKeyValue|RSAKeyValue)
        // DSAKeyValue (P, Q, G, Y, J?, (Seed, PgenCounter)?)
        // RSAKeyValue (Modulus, Exponent)
        // RetrievalMethod (Transforms?)
        // X509Data ((X509IssuerSerial | X509SKI | X509SubjectName | X509Certificate)+ | X509CRL)
        // X509IssuerSerial (X509IssuerName, X509SerialNumber)
        // PGPData ((PGPKeyID, PGPKeyPacket?) | (PGPKeyPacket))
        // SPKIData (SPKISexp)

        /** Tag of Element CanonicalizationMethod **/
        public const String _TAG_CANONICALIZATIONMETHOD = "CanonicalizationMethod";
    
        /** Tag of Element DigestMethod **/
        public const String _TAG_DIGESTMETHOD = "DigestMethod";
    
        /** Tag of Element DigestValue **/
        public const String _TAG_DIGESTVALUE = "DigestValue";
    
        /** Tag of Element Manifest **/
        public const String _TAG_MANIFEST = "Manifest";
    
        /** Tag of Element Methods **/
        public const String _TAG_METHODS = "Methods";
    
        /** Tag of Element Object **/
        public const String _TAG_OBJECT = "Object";
    
        /** Tag of Element Reference **/
        public const String _TAG_REFERENCE = "Reference";
    
        /** Tag of Element Signature **/
        public const String _TAG_SIGNATURE = "Signature";
    
        /** Tag of Element SignatureMethod **/
        public const String _TAG_SIGNATUREMETHOD = "SignatureMethod";
    
        /** Tag of Element HMACOutputLength **/
        public const String _TAG_HMACOUTPUTLENGTH = "HMACOutputLength";
    
        /** Tag of Element SignatureProperties **/
        public const String _TAG_SIGNATUREPROPERTIES = "SignatureProperties";
    
        /** Tag of Element SignatureProperty **/
        public const String _TAG_SIGNATUREPROPERTY = "SignatureProperty";
    
        /** Tag of Element SignatureValue **/
        public const String _TAG_SIGNATUREVALUE = "SignatureValue";
    
        /** Tag of Element SignedInfo **/
        public const String _TAG_SIGNEDINFO = "SignedInfo";
    
        /** Tag of Element Transform **/
        public const String _TAG_TRANSFORM = "Transform";
    
        /** Tag of Element Transforms **/
        public const String _TAG_TRANSFORMS = "Transforms";
    
        /** Tag of Element XPath **/
        public const String _TAG_XPATH = "XPath";
    
        /** Tag of Element KeyInfo **/
        public const String _TAG_KEYINFO = "KeyInfo";
    
        /** Tag of Element KeyName **/
        public const String _TAG_KEYNAME = "KeyName";
    
        /** Tag of Element KeyValue **/
        public const String _TAG_KEYVALUE = "KeyValue";
    
        /** Tag of Element RetrievalMethod **/
        public const String _TAG_RETRIEVALMETHOD = "RetrievalMethod";
    
        /** Tag of Element X509Data **/
        public const String _TAG_X509DATA = "X509Data";
    
        /** Tag of Element PGPData **/
        public const String _TAG_PGPDATA = "PGPData";
    
        /** Tag of Element SPKIData **/
        public const String _TAG_SPKIDATA = "SPKIData";
    
        /** Tag of Element MgmtData **/
        public const String _TAG_MGMTDATA = "MgmtData";
    
        /** Tag of Element RSAKeyValue **/
        public const String _TAG_RSAKEYVALUE = "RSAKeyValue";
    
        /** Tag of Element Exponent **/
        public const String _TAG_EXPONENT = "Exponent";
    
        /** Tag of Element Modulus **/
        public const String _TAG_MODULUS = "Modulus";
    
        /** Tag of Element DSAKeyValue **/
        public const String _TAG_DSAKEYVALUE = "DSAKeyValue";
    
        /** Tag of Element P **/
        public const String _TAG_P = "P";
    
        /** Tag of Element Q **/
        public const String _TAG_Q   = "Q";
    
        /** Tag of Element G **/
        public const String _TAG_G = "G";
    
        /** Tag of Element Y **/
        public const String _TAG_Y = "Y";
    
        /** Tag of Element J **/
        public const String _TAG_J = "J";
    
        /** Tag of Element Seed **/
        public const String _TAG_SEED = "Seed";
    
        /** Tag of Element PgenCounter **/
        public const String _TAG_PGENCOUNTER = "PgenCounter";
    
        /** Tag of Element rawX509Certificate **/
        public const String _TAG_RAWX509CERTIFICATE = "rawX509Certificate";
    
        /** Tag of Element X509IssuerSerial **/
        public const String _TAG_X509ISSUERSERIAL= "X509IssuerSerial";
    
        /** Tag of Element X509SKI **/
        public const String _TAG_X509SKI = "X509SKI";
    
        /** Tag of Element X509SubjectName **/
        public const String _TAG_X509SUBJECTNAME = "X509SubjectName";
    
        /** Tag of Element X509Certificate **/
        public const String _TAG_X509CERTIFICATE = "X509Certificate";
    
        /** Tag of Element X509CRL **/
        public const String _TAG_X509CRL = "X509CRL";
    
        /** Tag of Element X509IssuerName **/
        public const String _TAG_X509ISSUERNAME = "X509IssuerName";
    
        /** Tag of Element X509SerialNumber **/
        public const String _TAG_X509SERIALNUMBER = "X509SerialNumber";
    
        /** Tag of Element PGPKeyID **/
        public const String _TAG_PGPKEYID = "PGPKeyID";

        /** Tag of Element PGPKeyPacket **/
        public const String _TAG_PGPKEYPACKET = "PGPKeyPacket";

        /** Tag of Element PGPKeyPacket **/
        public const String _TAG_DERENCODEDKEYVALUE = "DEREncodedKeyValue";
    
        /** Tag of Element PGPKeyPacket **/
        public const String _TAG_KEYINFOREFERENCE = "KeyInfoReference";
    
        /** Tag of Element PGPKeyPacket **/
        public const String _TAG_X509DIGEST = "X509Digest";
    
        /** Tag of Element SPKISexp **/
        public const String _TAG_SPKISEXP = "SPKISexp";

        /** Digest - Required SHA1 */
        public const String ALGO_ID_DIGEST_SHA1 = SignatureSpecNS + "sha1";

        /**
         * @see <A HREF="http://www.ietf.org/internet-drafts/draft-blake-wilson-xmldsig-ecdsa-02.txt">
         *  draft-blake-wilson-xmldsig-ecdsa-02.txt</A>
         */
        public const String ALGO_ID_SIGNATURE_ECDSA_CERTICOM = 
            "http://www.certicom.com/2000/11/xmlecdsig#ecdsa-sha1";

        private Constants() {
            // we don't allow instantiation
        }
    }
}
