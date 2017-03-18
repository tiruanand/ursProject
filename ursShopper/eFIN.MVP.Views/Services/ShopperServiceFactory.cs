namespace eFIN.MVP.Views.Services
{
    public class ShopperServiceFactory
    {
        #region Variables
        static ConsumerServiceReference.ConsumerServiceClient consumerSvc;
        static SupplierServiceReference.SupplierServiceClient supplierSvc;
        static MarketerServiceReference.MarketerServiceClient marketerSvc;
        #endregion

        #region Service Factory Methods
        static object conSvc = new object();
        public static ConsumerServiceReference.ConsumerServiceClient ConsumerService
        {
            get
            {
                lock (conSvc)
                {
                    if (consumerSvc == null)
                        consumerSvc = new ConsumerServiceReference.ConsumerServiceClient();
                    return consumerSvc;
                }
            }
        }

        static object supSvc = new object();
        public static SupplierServiceReference.SupplierServiceClient SupplierService
        {
            get
            {
                lock (supSvc)
                {
                    if (supplierSvc == null)
                        supplierSvc = new SupplierServiceReference.SupplierServiceClient();
                    return supplierSvc;
                }
            }
        }

        static object mktSvc = new object();
        public static MarketerServiceReference.MarketerServiceClient MarketerService
        {
            get
            {
                lock (mktSvc)
                {
                    if (marketerSvc == null)
                        marketerSvc = new MarketerServiceReference.MarketerServiceClient();
                    return marketerSvc;
                }
            }
        }
        #endregion
    }
}