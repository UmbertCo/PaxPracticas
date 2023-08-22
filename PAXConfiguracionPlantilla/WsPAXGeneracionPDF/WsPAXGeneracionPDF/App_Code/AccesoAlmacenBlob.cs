using System;

namespace SAT.CFDI.Cliente.Procesamiento
{
    using System.Configuration;
    using System.IO;

    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;


    public class AccesoAlmacenBlob
    {
        private const int KB = 1024;
        private const int MB = 1024 * KB;        
        private const long MaximumBlobSizeBeforeTransmittingAsBlocks = 62 * MB;        

        #region Métodos Públicos
        public string AlmacenarCfdiFramework4(byte[] cfdi, string uuid, string sversion)
        {
            var sharedAccessSignature = new StorageCredentialsSharedAccessSignature(ConfigurationManager.AppSettings["SharedAccesSignature"].Replace('|', '&'));
            var blobClient = new CloudBlobClient(ConfigurationManager.AppSettings["BlobStorageEndpoint"],
                                                 sharedAccessSignature);
            blobClient.RetryPolicy = RetryPolicies.RetryExponential(15, TimeSpan.FromSeconds(25));
            blobClient.Timeout = TimeSpan.FromMinutes(30);
            var blobContainer = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerName"]);
            //var blob = blobContainer.GetBlobReference(uuid);

            var blob = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerName"]).GetBlobReference(uuid);

            //Primer paso, subir el contenido al blob
            blob.UploadByteArray(cfdi);

            //Definir la metadata
            blob.Metadata["versionCFDI"] = sversion; //"3.2";  //Aquí se colocará la versión del CFDI a enviar.

            //Ultimo, siempre colocar este método para enviar la información en la metadata.
            blob.SetMetadata();

            //if (cfdi.Length <= MaximumBlobSizeBeforeTransmittingAsBlocks)
            //{                
            //    blob.UploadFromStream(cfdi);
            //}
            //else
            //{
            //    var blockBlob = blobContainer.GetBlockBlobReference(blob.Uri.AbsoluteUri);
            //    blockBlob.UploadFromStream(cfdi);
            //}

            return blob.Uri.AbsoluteUri;
        }           
        #endregion
    }
}
