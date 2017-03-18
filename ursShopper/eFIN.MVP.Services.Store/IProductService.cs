using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace eFIN.MVP.Services.Store
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IProductService" in both code and config file together.
    [ServiceContract]
    public interface IProductService
    {
        [OperationContract]
        Product GetProduct(int ProductId);

        [OperationContract]
        Product GetMarketerProduct(string MarketerName, string ProductName);

        [OperationContract]
        Product GetMarketerProductById(long MarketerProductId);

        [OperationContract]
        Product GetSupplierProduct(string SupplierName, string ProductName);

        [OperationContract]
        Product GetSupplierProductById(long SupplierProductId);

        [OperationContract]
        long GetProductId(string ProductSKU);

        [OperationContract]
        List<Product> GetAllProducts();

        [OperationContract]
        List<Product> GetAllProductsList();

        [OperationContract]
        long SaveProduct(string PName, string PDescription, string PManufacturer, decimal Price, string PSKU, string PUPC, string PEAN, string PGTIN, string PUOM);

        [OperationContract]
        void DeleteProduct(string ProductSKU);

        [OperationContract]
        void DeleteProductById(int Id);

        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "eFIN.MVP.Services.Store.ContractType".
    [DataContract]
    public class Product
    {
        [DataMember]
        public long ID { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Manufacturer { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public DateTime LastUpdatedOn { get; set; }

        [DataMember]
        public string SKU { get; set; }

        [DataMember]
        public string UPC { get; set; }

        [DataMember]
        public string EAN { get; set; }

        [DataMember]
        public string GTIN { get; set; }

        [DataMember]
        public string UOM { get; set; }

        [DataMember]
        public decimal Price { get; set; }
    }

}
