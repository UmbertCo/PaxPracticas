using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canonicalizacion_Tests
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

        /** MAC - Required HMAC-SHA1 */
        public static String ALGO_ID_MAC_HMAC_SHA1 = 
            Constants.SignatureSpecNS + "hmac-sha1";

        /** Signature - Required DSAwithSHA1 (DSS) */
        public static String ALGO_ID_SIGNATURE_DSA = 
            Constants.SignatureSpecNS + "dsa-sha1";

        /** Signature - Optional DSAwithSHA256 */
        public static String ALGO_ID_SIGNATURE_DSA_SHA256 =
            Constants.SignatureSpec11NS + "dsa-sha256";

        /** Signature - Recommended RSAwithSHA1 */
        public static String ALGO_ID_SIGNATURE_RSA = 
            Constants.SignatureSpecNS + "rsa-sha1";
    
        /** Signature - Recommended RSAwithSHA1 */
        public static String ALGO_ID_SIGNATURE_RSA_SHA1 = 
            Constants.SignatureSpecNS + "rsa-sha1";
    
        /** Signature - NOT Recommended RSAwithMD5 */
        public static String ALGO_ID_SIGNATURE_NOT_RECOMMENDED_RSA_MD5 = 
            Constants.MoreAlgorithmsSpecNS + "rsa-md5";
    
        /** Signature - Optional RSAwithRIPEMD160 */
        public static String ALGO_ID_SIGNATURE_RSA_RIPEMD160 = 
            Constants.MoreAlgorithmsSpecNS + "rsa-ripemd160";
    
        /** Signature - Optional RSAwithSHA224 */
        public static String ALGO_ID_SIGNATURE_RSA_SHA224 = 
            Constants.MoreAlgorithmsSpecNS + "rsa-sha224";
    
        /** Signature - Optional RSAwithSHA256 */
        public static String ALGO_ID_SIGNATURE_RSA_SHA256 = 
            Constants.MoreAlgorithmsSpecNS + "rsa-sha256";
    
        /** Signature - Optional RSAwithSHA384 */
        public static String ALGO_ID_SIGNATURE_RSA_SHA384 = 
            Constants.MoreAlgorithmsSpecNS + "rsa-sha384";
    
        /** Signature - Optional RSAwithSHA512 */
        public static String ALGO_ID_SIGNATURE_RSA_SHA512 = 
            Constants.MoreAlgorithmsSpecNS + "rsa-sha512";
    
        /** Signature - Optional RSAwithSHA1andMGF1 */
        public static String ALGO_ID_SIGNATURE_RSA_SHA1_MGF1 = 
            Constants.XML_DSIG_NS_MORE_07_05 + "sha1-rsa-MGF1";
    
        /** Signature - Optional RSAwithSHA224andMGF1 */
        public static String ALGO_ID_SIGNATURE_RSA_SHA224_MGF1 = 
            Constants.XML_DSIG_NS_MORE_07_05 + "sha224-rsa-MGF1";

        /** Signature - Optional RSAwithSHA256andMGF1 */
        public static String ALGO_ID_SIGNATURE_RSA_SHA256_MGF1 = 
            Constants.XML_DSIG_NS_MORE_07_05 + "sha256-rsa-MGF1";

        /** Signature - Optional RSAwithSHA384andMGF1 */
        public static String ALGO_ID_SIGNATURE_RSA_SHA384_MGF1 = 
            Constants.XML_DSIG_NS_MORE_07_05 + "sha384-rsa-MGF1";

        /** Signature - Optional RSAwithSHA512andMGF1 */
        public static String ALGO_ID_SIGNATURE_RSA_SHA512_MGF1 = 
            Constants.XML_DSIG_NS_MORE_07_05 + "sha512-rsa-MGF1";

        /** HMAC - NOT Recommended HMAC-MD5 */
        public static String ALGO_ID_MAC_HMAC_NOT_RECOMMENDED_MD5 = 
            Constants.MoreAlgorithmsSpecNS + "hmac-md5";
    
        /** HMAC - Optional HMAC-RIPEMD160 */
        public static String ALGO_ID_MAC_HMAC_RIPEMD160 = 
            Constants.MoreAlgorithmsSpecNS + "hmac-ripemd160";
    
        /** HMAC - Optional HMAC-SHA2224 */
        public static String ALGO_ID_MAC_HMAC_SHA224 = 
            Constants.MoreAlgorithmsSpecNS + "hmac-sha224";
    
        /** HMAC - Optional HMAC-SHA256 */
        public static String ALGO_ID_MAC_HMAC_SHA256 = 
            Constants.MoreAlgorithmsSpecNS + "hmac-sha256";
    
        /** HMAC - Optional HMAC-SHA284 */
        public static String ALGO_ID_MAC_HMAC_SHA384 = 
            Constants.MoreAlgorithmsSpecNS + "hmac-sha384";
    
        /** HMAC - Optional HMAC-SHA512 */
        public static String ALGO_ID_MAC_HMAC_SHA512 = 
            Constants.MoreAlgorithmsSpecNS + "hmac-sha512";
    
        /**Signature - Optional ECDSAwithSHA1 */
        public static String ALGO_ID_SIGNATURE_ECDSA_SHA1 = 
            "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha1";
    
        /**Signature - Optional ECDSAwithSHA224 */
        public static String ALGO_ID_SIGNATURE_ECDSA_SHA224 = 
            "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha224";
    
        /**Signature - Optional ECDSAwithSHA256 */
        public static String ALGO_ID_SIGNATURE_ECDSA_SHA256 = 
            "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256";
    
        /**Signature - Optional ECDSAwithSHA384 */
        public static String ALGO_ID_SIGNATURE_ECDSA_SHA384 = 
            "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha384";
    
        /**Signature - Optional ECDSAwithSHA512 */
        public static String ALGO_ID_SIGNATURE_ECDSA_SHA512 = 
            "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512";
    
        /**Signature - Optional ECDSAwithRIPEMD160 */
        public static String ALGO_ID_SIGNATURE_ECDSA_RIPEMD160 = 
            "http://www.w3.org/2007/05/xmldsig-more#ecdsa-ripemd160";

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
