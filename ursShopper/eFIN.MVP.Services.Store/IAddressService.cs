using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace eFIN.MVP.Services.Store
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOfferService" in both code and config file together.
    [ServiceContract]
    public interface IAddressService
    {
        [OperationContract]
        long SaveAddress(string Line1, string Line2, string Line3, string City, string ZipPostalCode, string StateProvinceCounty, string Country);

        [OperationContract]
        void RemoveAddress(string Line1, string Line2, string Line3, string City, string ZipPostalCode, string StateProvinceCounty, string Country);

        [OperationContract]
        Address GetAddress(string Line1, string City, string ZipPostalCode, string StateProvinceCounty, string Country);
        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "eFIN.MVP.Services.Store.ContractType".
    [DataContract]
    public class Address
    {
        [DataMember]
        public long AddressId { get; set; }

        [DataMember]
        public string Line1 { get; set; }

        [DataMember]
        public string Line2 { get; set; }

        [DataMember]
        public string Line3 { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string ZipPostalCode { get; set; }

        [DataMember]
        public string StateProvinceCounty { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public DateTime LastUpdatedOn { get; set; }
    }
}
