using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace eFIN.MVP.Services.Store
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMarketerService" in both code and config file together.
    [ServiceContract]
    public interface IMarketerService
    {
        [OperationContract]
        Marketer GetMarketer(int MarketerId);
        [OperationContract]
        long GetMarketerId(string MarketerName);
        [OperationContract]
        List<Marketer> GetAllMarketers();
        [OperationContract]
        List<Marketer> GetAllMarketersList();
        [OperationContract]
        long SaveMarketer(string MarketerName, string MarketerDomain, string MarketerType);
        [OperationContract]
        void DeleteMarketer(string MarketerName);
        [OperationContract]
        void DeleteMarketerById(int MarketerId);
        [OperationContract]
        void SaveMarketerProduct(string MarketerName, string ProductName, string ProductDescription, string ProductManufacturer, decimal ProductPrice, string ProductSKU, string ProductUPC, string ProductEAN, string ProductGTIN, string ProductUOM);
        [OperationContract]
        void AddProductToMarketer(string MarketerName, string ProductName);
        [OperationContract]
        void RemoveProductFromMarketer(string MarketerName, string ProductName);
        [OperationContract]
        MarketerProducts GetMarketerProduct(string MarketerName, string ProductName);
        [OperationContract]
        long GetMarketerProductId(long MarketerId, long ProductId);
        [OperationContract]
        MarketerProducts GetMarketerProducts(string MarketerName);
        [OperationContract]
        ProductMarketers GetProductMarketers(string ProductName);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "eFIN.MVP.Services.Store.ContractType".
    [DataContract]
    public class Marketer
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Domain { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public DateTime LastUpdatedOn { get; set; }
    }

    [DataContract]
    public class ProductMarketers
    {
        [DataMember]
        public Product Product { get; set; }

        [DataMember]
        public List<RelatedMarketer> Marketers { get; set; }
    }

    [DataContract]
    public class MarketerProducts
    {
        [DataMember]
        public Marketer Marketer { get; set; }

        [DataMember]
        public List<RelatedProduct> Products { get; set; }
    }

    [DataContract]
    public class RelatedMarketer
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public Marketer Marketer { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public DateTime LastUpdatedOn { get; set; }
    }
}
