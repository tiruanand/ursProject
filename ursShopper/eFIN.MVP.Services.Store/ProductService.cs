using eFIN.MVP.AzureEF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eFIN.MVP.Services.Store
{
    /// <summary>
    /// Maintains the products in urshopper from the suppliers, marketers.
    /// </summary>
    public class ProductService : IProductService
    {
        ursCoreMVPEntities azureDB;

        public ProductService()
        {
            azureDB = new ursCoreMVPEntities();
        }

        public ProductService(ursCoreMVPEntities AzureEFEntities)
        {
            azureDB = AzureEFEntities;
        }

        public Product GetProduct(int ProductId)
        {
            var EFProduct = (from p
                            in azureDB.ProductEntities
                             where p.product_id == ProductId
                             select p).FirstOrDefault();
            if (EFProduct != null)
                return TranslateEntityToObject(EFProduct);
            else
                throw new Exception(string.Format("Product: {0} does not exists in urshopper.", ProductId));
        }

        public Product GetMarketerProduct(string MarketerName, string ProductName)
        {
            var EFProduct = (from p
                            in azureDB.ProductEntities
                             where p.MarketerProducts.Any(mp => mp.Marketer.marketer_name.Equals(MarketerName, StringComparison.InvariantCultureIgnoreCase))
                             && p.product_name.Equals(ProductName, StringComparison.InvariantCultureIgnoreCase)
                             select p).FirstOrDefault();
            if (EFProduct != null)
                return TranslateEntityToObject(EFProduct);
            else
                throw new Exception(string.Format("Marketer:{0} does not have Product: {1} does not exists in urshopper.", MarketerName, ProductName));
        }

        public Product GetMarketerProductById(long MarketerProductId)
        {
            var EFProduct = (from p
                            in azureDB.ProductEntities
                              where p.MarketerProducts.Any(mp => mp.marketerproduct_id == MarketerProductId)
                              select p).FirstOrDefault();
            if (EFProduct != null)
                return TranslateEntityToObject(EFProduct);
            else
                throw new Exception(string.Format("MarketerProduct: {0} does not exists in urshopper.", MarketerProductId));
        }

        public Product GetSupplierProduct(string SupplierName, string ProductName)
        {
            var EFProduct = (from p
                            in azureDB.ProductEntities
                             where p.SupplierProducts.Any(sp => sp.Supplier.supplier_name.Equals(SupplierName, StringComparison.InvariantCultureIgnoreCase))
                             && p.product_name.Equals(ProductName, StringComparison.InvariantCultureIgnoreCase)
                             select p).FirstOrDefault();
            if (EFProduct != null)
                return TranslateEntityToObject(EFProduct);
            else
                throw new Exception(string.Format("Supplier:{0} does not have Product: {1} does not exists in urshopper.", SupplierName, ProductName));
        }

        public Product GetSupplierProductById(long SupplierProductId)
        {
            var EFProduct = (from p
                            in azureDB.ProductEntities
                             where p.SupplierProducts.Any(sp => sp.supplierproduct_id == SupplierProductId)
                             select p).FirstOrDefault();
            if (EFProduct != null)
                return TranslateEntityToObject(EFProduct);
            else
                throw new Exception(string.Format("SupplierProduct: {0} does not exists in urshopper.", SupplierProductId));
        }

        public long GetProductId(string SKU)
        {
            var EFProduct = (from p
                            in azureDB.ProductEntities
                             where p.product_sku.Equals(SKU, StringComparison.InvariantCulture)
                             select p).FirstOrDefault();
            if (EFProduct != null)
                return  EFProduct.product_id;
            else
                throw new Exception(string.Format("Product: {0} does not exists in urshopper.", SKU));
        }

        public List<Product> GetAllProductsList()
        {
            var allProducts = (from p
                            in azureDB.ProductEntities
                               select p).ToList();

            var targetList = allProducts.Select(x => new Product()
            {
                ID = x.product_id, Name = x.product_name
            }).ToList();
            return targetList;
        }

        public List<Product> GetAllProducts()
        {
            var allProducts = (from p
                            in azureDB.ProductEntities
                                select p).ToList();

            var targetList = allProducts.Select(x => new Product()
                                    { ID = x.product_id, Name = x.product_name, Manufacturer = x.product_manufacturer, Description = x.product_description
                                        , CreatedOn = x.product_create, LastUpdatedOn = x.product_update ?? DateTime.Now })
                                .ToList();
            return targetList;
        }
        
        public long SaveProduct(string PName, string PDescription, string PManufacturer, decimal Price, string PSKU, string PUPC, string PEAN, string PGTIN, string PUOM)
        {
            var EFProduct = (from p
                            in azureDB.ProductEntities
                             where p.product_sku.Equals(PSKU, StringComparison.InvariantCultureIgnoreCase)
                             select p).FirstOrDefault();
            if (EFProduct == null)
            {
                var productEntity = new ProductEntity()
                {
                    product_sku = PSKU,
                    product_upc = PUPC,
                    product_ean = PEAN,
                    product_gtin = PGTIN,
                    product_uom = PUOM,
                    product_price = Price,
                    product_name = PName,
                    product_manufacturer = PManufacturer,
                    product_description = PDescription,
                    product_create = DateTime.Now,
                    product_update = DateTime.Now
                };
                azureDB.ProductEntities.Add(productEntity);
            }
            else
            {
                EFProduct.product_sku = PSKU;
                EFProduct.product_upc = PUPC;
                EFProduct.product_ean = PEAN;
                EFProduct.product_gtin = PGTIN;
                EFProduct.product_uom = PUOM;
                EFProduct.product_price = Price;
                EFProduct.product_name = PName;
                EFProduct.product_manufacturer = PManufacturer;
                EFProduct.product_description = PDescription;
                EFProduct.product_update = DateTime.Now;
            }
            azureDB.SaveChanges();

            return GetProductId(PName);
        }

        public void DeleteProduct(string SKU)
        {
            var EFProduct = (from p
                            in azureDB.ProductEntities
                             where p.product_sku.Equals(SKU, StringComparison.InvariantCultureIgnoreCase)
                             select p).FirstOrDefault();
            if (EFProduct != null)
            {
                azureDB.ProductEntities.Remove(EFProduct);
                azureDB.SaveChanges();
            }
            else
                throw new Exception(string.Format("Product: {0} does not exists in urshopper.", SKU));
        }

        public void DeleteProductById(int Id)
        {
            var EFProduct = (from p
                            in azureDB.ProductEntities
                              where p.product_id == Id
                              select p).FirstOrDefault();
            if (EFProduct != null)
            {
                azureDB.ProductEntities.Remove(EFProduct);
                azureDB.SaveChanges();
            }
            else
                throw new Exception(string.Format("Product: {0} does not exists in urshopper.", Id));
        }

        public Product TranslateEntityToObject(ProductEntity productObj)
        {
            Product objProduct = new Product()
            {
                ID = productObj.product_id,
                Name = productObj.product_name,
                Description = productObj.product_description,
                Manufacturer = productObj.product_manufacturer,
                SKU = productObj.product_sku,
                UPC = productObj.product_upc,
                EAN = productObj.product_ean,
                GTIN = productObj.product_gtin,
                UOM = productObj.product_uom,
                Price = productObj.product_price,
                CreatedOn = productObj.product_create,
                LastUpdatedOn = productObj.product_update ?? DateTime.Now
            };
            return objProduct;
        }
    }
}
