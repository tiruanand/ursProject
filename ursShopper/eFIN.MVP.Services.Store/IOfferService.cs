using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace eFIN.MVP.Services.Store
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOfferService" in both code and config file together.
    [ServiceContract]
    public interface ISupplierOfferService
    {
        [OperationContract]
        long SaveSupplierOffer(string SupplierName, string ProductSKU, decimal PriceOffered, DateTime OfferValidUpto);

        [OperationContract]
        bool RemoveSupplierOffer(string SupplierName, string ProductSKU, decimal PriceOffered, DateTime OfferValidUpto);

        [OperationContract]
        SupplierOffer GetSupplierOffer(string SupplierName, string ProductSKU, decimal PriceOffered, DateTime OfferValidUpto);

        [OperationContract]
        List<SupplierOffer> GetSupplierOffers(string SupplierName);

        // TODO: Add your service operations here
    }

    [ServiceContract]
    public interface IConsumerOfferService
    {
        [OperationContract]
        long SaveConsumerOffer(string ConsumerName, string MarketerName, string ProductSKU, DateTime ConsumerOfferDate, DateTime OfferValidUpto, long ConsumerOfferStatusId, int ConsumerOfferQty, decimal ConsumerOfferMaxPrice, bool IsConsumerOfferCurrent);
        [OperationContract]
        bool RemoveConsumerOffer(string ConsumerName, string MarketerName, string ProductSKU, decimal ConsumerOfferMaxPrice, DateTime OfferValidUpto);
        [OperationContract]
        ConsumerOffer GetConsumerOffer(string ConsumerName, string MarketerName, string ProductSKU, decimal ConsumerOfferMaxPrice, DateTime OfferValidUpto);
        [OperationContract]
        List<ConsumerOffer> GetConsumerOffers(string MarketerName, string ConsumerName);

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "eFIN.MVP.Services.Store.ContractType".
    [DataContract]
    public class SupplierOffer : Offer
    {
        [DataMember]
        public long SupplierId { get; set; }

        [DataMember]
        public Supplier OfferedSupplier { get; set; }

        [DataMember]
        public List<Offer> Offers { get; set; }
    }

    [DataContract]
    public class MarketerOffer : Offer
    {
        [DataMember]
        public long MarketerId { get; set; }

        [DataMember]
        public Marketer OfferMarketer { get; set; }
    }

    [DataContract]
    public class ConsumerOffer : Offer
    {
        [DataMember]
        public long ConsumerId { get; set; }

        [DataMember]
        public long MarketerId { get; set; }

        [DataMember]
        public Consumer OfferedConsumer { get; set; }

        [DataMember]
        public List<Offer> Offers { get; set; }
    }

    [DataContract]
    public class Offer
    {
        [DataMember]
        public long ID { get; set; }

        [DataMember]
        public long OfferId { get; set; }

        [DataMember]
        public long MarketerProductId { get; set; }

        [DataMember]
        public long ProductId { get; set; }

        [DataMember]
        public Product OfferedProduct { get; set; }

        [DataMember]
        public decimal MaxPrice { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public DateTime OfferDate { get; set; }
        
        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public long StatusId { get; set; }

        [DataMember]
        public bool IsCurrentOffer { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public DateTime LastUpdatedOn { get; set; }
    }
}
