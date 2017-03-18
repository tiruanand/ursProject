using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace eFIN.MVP.Services.Store
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IConsumerService" in both code and config file together.
    [ServiceContract]
    public interface IConsumerService
    {
        [OperationContract]
        Consumer GetConsumer(string ConsumerHandle);

        [OperationContract]
        Consumer GetConsumerById(long ConsumerId);

        [OperationContract]
        long GetConsumerId(string ConsumerHandle);

        [OperationContract]
        List<Consumer> GetAllConsumers();

        [OperationContract]
        List<Consumer> GetAllConsumersList();

        [OperationContract]
        long SaveMarketerConsumer(string MarketerName, string MarkterDomain, string MarketerType, string ConsumerHandle, string ConsumerEmail, string ConsumerFName, string ConsumerMName, string ConsumerLName, string ConsumerPhone, string ConsumerStatus);

        [OperationContract]
        long SaveConsumer(string MarketerName, string ConsumerHandle, string ConsumerEmail, string ConsumerFName, string ConsumerMName, string ConsumerLName, string ConsumerPhone, string ConsumerStatus);

        [OperationContract]
        long SaveConsumerBillingAddress(string ConsumerHandle, string Line1, string Line2, string Line3, string City, string ZipPostalCode, string StateProvinceCounty, string Country );

        [OperationContract]
        long SaveConsumerShippingAddress(string ConsumerHandle, string Line1, string Line2, string Line3, string City, string ZipPostalCode, string StateProvinceCounty, string Country);

        [OperationContract]
        void DeleteConsumer(string ConsumerHandle);

        [OperationContract]
        void DeleteConsumerById(int ConsumerId);

        [OperationContract]
        List<ConsumerOffer> GetConsumerOffers(string MarketerName, string ConsumerHandle);

        [OperationContract]
        long SaveConsumerOffer(string ConsumerHandle, string MarketerName, string ProductSKU, DateTime ConsumerOfferDate, DateTime OfferValidUpto, int ConsumerOfferQty, decimal ConsumerOfferMaxPrice);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "eFIN.MVP.Services.Store.ContractType".
    [KnownType(typeof(Address))]
    [KnownType(typeof(Marketer))]
    [DataContract]
    public class Consumer
    {
        [Display(Name = "ID")]
        [DataMember]
        public long ConsumerId { get; set; }

        [DataMember]
        public long MarketerId { get; set; }

        [Display(Name = "Marketer")]
        [DataMember]
        public Marketer Marketer { get; set; }

        [DataMember]
        public Address BillAddress { get; set; }

        [DataMember]
        public Address ShipAddress { get; set; }

        [DataMember]
        public long BillAddressId { get; set; }

        [DataMember]
        public long ShipAddressId { get; set; }

        [DataMember]
        public string ConsumerHandle { get; set; }

        [Display(Name = "Email")]
        [DataMember]
        public string ConsumerEmail { get; set; }

        [Display(Name = "Name")]
        [DataMember]
        public string ConsumerName { get { return string.Format("{0} {1}", ConsumerFName ?? string.Empty, ConsumerLName ?? string.Empty); } set { } }

        [DataMember]
        public string ConsumerFName { get; set; }

        [DataMember]
        public string ConsumerMName { get; set; }

        [DataMember]
        public string ConsumerLName { get; set; }

        [Display(Name = "Phone")]
        [DataMember]
        public string ConsumerPhone { get; set; }

        [Display(Name = "Status")]
        [DataMember]
        public string ConsumerStatus { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public DateTime LastUpdatedOn { get; set; }
    }
}
