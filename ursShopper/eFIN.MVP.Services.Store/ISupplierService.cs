using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace eFIN.MVP.Services.Store
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISupplierService" in both code and config file together.
    [ServiceContract]
    public interface ISupplierService
    {
        [OperationContract]
        Supplier GetSupplier(int SupplierId);
        [OperationContract]
        long GetSupplierId(string SupplierName);
        [OperationContract]
        List<Supplier> GetAllSuppliers();
        [OperationContract]
        List<Supplier> GetAllSuppliersList();
        [OperationContract]
        long SaveSupplier(string SupplierName, string SupplierDomain, string SupplierType);
        [OperationContract]
        void DeleteSupplier(string SupplierName);
        [OperationContract]
        void DeleteSupplierById(int SupplierId);

        [OperationContract]
        void SaveSupplierProduct(string SupplierName, string ProductName, string ProductDescription, string ProductManufacturer, decimal ProductPrice, string ProductSKU, string ProductUPC, string ProductEAN, string ProductGTIN, string ProductUOM);
        [OperationContract]
        void AddProductToSupplier(string SupplierName, string ProductName);
        [OperationContract]
        bool RemoveProductFromSupplier(string SupplierName, string ProductName);
        [OperationContract]
        SupplierProducts GetSupplierProduct(string SupplierName, string ProductName);
        [OperationContract]
        long GetSupplierProductId(long SupplierId, long ProductId);
        [OperationContract]
        SupplierProducts GetSupplierProducts(string SupplierName);
        [OperationContract]
        ProductSuppliers GetProductSuppliers(string ProductName);


        [OperationContract]
        long SaveSupplierOffer(string SupplierName, string ProductSKU, decimal PriceOffered, DateTime OfferValidUpto);
        [OperationContract]
        bool RemoveSupplierOffer(string SupplierName, string ProductSKU, decimal PriceOffered, DateTime OfferValidUpto);
        [OperationContract]
        SupplierOffer GetSupplierOffer(string SupplierName, string ProductSKU, decimal PriceOffered, DateTime OfferValidUpto);
        [OperationContract]
        List<SupplierOffer> GetSupplierOffers(string SupplierName);

    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "eFIN.MVP.Services.Store.ContractType".
    [DataContract]
    public class Supplier
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
    public class ProductSuppliers
    {
        [DataMember]
        public Product Product { get; set; }

        [DataMember]
        public List<RelatedSupplier> Suppliers { get; set; }
    }

    [DataContract]
    public class SupplierProducts
    {
        [DataMember]
        public Supplier Supplier { get; set; }

        [DataMember]
        public List<RelatedProduct> Products { get; set; }
    }


    [DataContract]
    public class RelatedSupplier
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public Supplier Supplier { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public DateTime LastUpdatedOn { get; set; }
    }

    [DataContract]
    public class RelatedProduct
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public Product Product { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public DateTime LastUpdatedOn { get; set; }
    }
}
