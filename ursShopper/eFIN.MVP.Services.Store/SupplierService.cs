using eFIN.MVP.AzureEF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eFIN.MVP.Services.Store
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ISupplierService" in both code and config file together.
    public class SupplierService : ISupplierService
    {
        #region Member Vaiables
        ursCoreMVPEntities azureDB;
        ProductService prodSvc;
        SupplierOfferService supplierOfferSvc;
        #endregion

        #region Constructor
        /// <summary>
        /// Service that implements ISupplierService to maintain for Supplier related actions.
        /// </summary>
        public SupplierService()
        {
            azureDB = new ursCoreMVPEntities();
            prodSvc = new ProductService(azureDB);
            supplierOfferSvc = new SupplierOfferService(azureDB, this, prodSvc);
        }
        #endregion

        #region Supplier service methods
        /// <summary>
        /// Retrieve Supplier for the given Supplier Id.
        /// </summary>
        /// <param name="SupplierId"></param>
        /// <returns></returns>
        public Supplier GetSupplier(int SupplierId)
        {
            var EFSupplier = (from p
                            in azureDB.SupplierEntities
                              where p.supplier_id == SupplierId
                              select p).FirstOrDefault();
            if (EFSupplier != null)
                return TranslateEntityToObject(EFSupplier);
            else
                throw new Exception("Invalid supplier");
        }

        /// <summary>
        /// Retrieve supplier unique id for the given supplier name
        /// </summary>
        /// <param name="SupplierName"></param>
        /// <returns></returns>
        public long GetSupplierId(string SupplierName)
        {
            var EFSupplier = (from p
                            in azureDB.SupplierEntities
                              where p.supplier_name.Equals(SupplierName, StringComparison.InvariantCultureIgnoreCase)
                              select p).FirstOrDefault();
            if (EFSupplier != null)
                return  EFSupplier.supplier_id;
            else
                throw new Exception("Invalid supplier");
        }

        /// <summary>
        /// Retrieves all available suppliers.
        /// </summary>
        /// <returns></returns>
        public List<Supplier> GetAllSuppliers()
        {
            var allSuppliers = (from p
                            in azureDB.SupplierEntities
                                select p).ToList();

            var targetList = allSuppliers.Select(x => new Supplier()
            {
                Id = x.supplier_id,
                Name = x.supplier_name,
                Type = x.supplier_type,
                Domain = x.supplier_domain
                                        ,
                CreatedOn = x.supplier_create,
                LastUpdatedOn = x.supplier_update ?? DateTime.Now
            }).ToList();
            return targetList;
        }

        public List<Supplier> GetAllSuppliersList()
        {
            var allSuppliers = (from p
                            in azureDB.SupplierEntities
                                select p).ToList();

            var targetList = allSuppliers.Select(x => new Supplier()
            {
                Id = x.supplier_id,
                Name = x.supplier_name,
            }).ToList();
            return targetList;
        }


        /// <summary>
        /// Creates or updates supplier based on given supplier details.
        /// </summary>
        /// <param name="SupplierName"></param>
        /// <param name="SupplierDomain"></param>
        /// <param name="SupplierType"></param>
        /// <returns></returns>
        public long SaveSupplier(string SupplierName, string SupplierDomain, string SupplierType)
        {
            var EFSupplier = (from p
                            in azureDB.SupplierEntities
                              where p.supplier_name.Equals(SupplierName, StringComparison.InvariantCultureIgnoreCase)
                              select p).FirstOrDefault();
            if (EFSupplier == null)
            {
                var supplierEntity = new SupplierEntity() { supplier_name = SupplierName, supplier_type = SupplierType, supplier_domain = SupplierDomain, supplier_create = DateTime.Now, supplier_update = DateTime.Now };
                azureDB.SupplierEntities.Add(supplierEntity);
            }
            else
            {
                EFSupplier.supplier_type = SupplierType;
                EFSupplier.supplier_domain = SupplierDomain;
            }
            azureDB.SaveChanges();
            return GetSupplierId(SupplierName);
        }

        /// <summary>
        /// Removes the supplier for the given supplier name.
        /// </summary>
        /// <param name="Name"></param>
        public void DeleteSupplier(string SupplierName)
        {
            var EFSupplier = (from p
                            in azureDB.SupplierEntities
                              where p.supplier_name.Equals(SupplierName, StringComparison.InvariantCultureIgnoreCase)
                              select p).FirstOrDefault();
            if (EFSupplier != null)
            {
                azureDB.SupplierEntities.Remove(EFSupplier);
                azureDB.SaveChanges();
            }
            else
                throw new Exception(string.Format("{0} - Supplier does not exists.", SupplierName));
        }

        /// <summary>
        /// Removes the supplier for the given supplier unique id.
        /// </summary>
        /// <param name="Id"></param>
        public void DeleteSupplierById(int SupplierId)
        {
            var EFSupplier = (from p
                            in azureDB.SupplierEntities
                              where p.supplier_id == SupplierId
                              select p).FirstOrDefault();
            if (EFSupplier != null)
            {
                azureDB.SupplierEntities.Remove(EFSupplier);
                azureDB.SaveChanges();
            }
            else
                throw new Exception(string.Format("{0} - Supplier does not exists.", SupplierId));
        }
        
        /// <summary>
        /// Translate Supplier entity framework object to realtime supplier object.
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public Supplier TranslateEntityToObject(SupplierEntity supplier)
        {
            Supplier objSupplier = new Supplier()
            {
                Id = supplier.supplier_id,
                Name = supplier.supplier_name,
                Domain = supplier.supplier_domain,
                Type = supplier.supplier_type,
                CreatedOn = supplier.supplier_create,
                LastUpdatedOn = supplier.supplier_update ?? DateTime.Now
            };
            return objSupplier;
        }
        #endregion

        #region SupplierProduct service methods
        /// <summary>
        /// Retrieves supplier-product based on given supplier unique id and product unique id
        /// </summary>
        /// <param name="SupplierId"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public bool IsSupplierProductExists(long SupplierId, long ProductId)
        {
            return (from sp in azureDB.SupplierProductEntities
                    where sp.supplier_id == SupplierId && sp.product_id == ProductId
                    select sp.supplierproduct_id).Any();
        }

        /// <summary>
        /// Create or update supplier-product based on given supplier-product details.
        /// </summary>
        /// <param name="SupplierName"></param>
        /// <param name="ProductName"></param>
        /// <param name="ProductDescription"></param>
        /// <param name="ProductManufacturer"></param>
        /// <param name="ProductPrice"></param>
        /// <param name="ProductSKU"></param>
        /// <param name="ProductUPC"></param>
        /// <param name="ProductEAN"></param>
        /// <param name="ProductGTIN"></param>
        /// <param name="ProductUOM"></param>
        public void SaveSupplierProduct(string SupplierName, string ProductName, string ProductDescription, string ProductManufacturer, decimal ProductPrice, string ProductSKU, string ProductUPC, string ProductEAN, string ProductGTIN, string ProductUOM)
        {
            long productId = prodSvc.SaveProduct(ProductName, ProductDescription, ProductManufacturer, ProductPrice, ProductSKU, ProductUPC, ProductEAN, ProductGTIN, ProductUOM);
            long supplierId = this.GetSupplierId(SupplierName);

            if (!IsSupplierProductExists(supplierId, productId))
                azureDB.SupplierProductEntities.Add(new SupplierProductEntity() { product_id = productId, supplier_id = supplierId, supplierproduct_create=DateTime.Now, supplierproduct_update=DateTime.Now });
        }

        /// <summary>
        /// Associates the given product to the given supplier.
        /// </summary>
        /// <param name="SupplierName"></param>
        /// <param name="ProductName"></param>
        public void AddProductToSupplier(string SupplierName, string ProductName)
        {
            long productId = prodSvc.GetProductId(ProductName);
            long supplierId = this.GetSupplierId(SupplierName);

            if (!IsSupplierProductExists(supplierId, productId))
            {
                azureDB.SupplierProductEntities.Add(new SupplierProductEntity() { product_id = productId, supplier_id = supplierId, supplierproduct_create = DateTime.Now, supplierproduct_update = DateTime.Now });
                azureDB.SaveChanges();
            }
        }

        /// <summary>
        /// Disassociate given product from the given supplier.
        /// </summary>
        /// <param name="SupplierName"></param>
        /// <param name="ProductName"></param>
        public bool RemoveProductFromSupplier(string SupplierName, string ProductName)
        {
            long productId = prodSvc.GetProductId(ProductName);
            long supplierId = this.GetSupplierId(SupplierName);

            if (!IsSupplierProductExists(supplierId, productId))
            {
                azureDB.SupplierProductEntities.Remove(new SupplierProductEntity() { product_id = productId, supplier_id = supplierId, supplierproduct_create = DateTime.Now, supplierproduct_update = DateTime.Now });
                azureDB.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieves avaiable supplier-products for the given supplier and product.
        /// </summary>
        /// <param name="SupplierName"></param>
        /// <param name="ProductName"></param>
        /// <returns></returns>
        public SupplierProducts GetSupplierProduct(string SupplierName, string ProductName)
        {
            var supplierProducts = (from sp in azureDB.SupplierProductEntities
                                    where sp.Supplier.supplier_name.Equals(SupplierName, StringComparison.InvariantCultureIgnoreCase)
                                    select sp);

            var prodSuppliers = new SupplierProducts();
            if (supplierProducts != null && supplierProducts.Count() > 0)
            {
                prodSuppliers.Supplier = supplierProducts.Select(sp => new Supplier()
                {
                    Id = sp.Supplier.supplier_id,
                    Name = sp.Supplier.supplier_name,
                    Domain = sp.Supplier.supplier_domain,
                    Type = sp.Supplier.supplier_type
                }).FirstOrDefault();

                var supProduct = (from p in supplierProducts
                                  where p.Product.product_name.Equals(ProductName, StringComparison.InvariantCultureIgnoreCase)
                                  select p).FirstOrDefault();

                prodSuppliers.Products = new List<RelatedProduct>()
                { new RelatedProduct()
                        {
                            Id = supProduct.supplierproduct_id,
                            Product = new Product()
                            {
                                ID = supProduct.product_id,
                                Name = supProduct.Product.product_name,
                                Manufacturer = supProduct.Product.product_manufacturer,
                                Description = supProduct.Product.product_description,
                                CreatedOn = supProduct.Product.product_create,
                                LastUpdatedOn = supProduct.Product.product_update ?? DateTime.Now
                            },
                            CreatedOn = supProduct.supplierproduct_create ?? DateTime.Now,
                            LastUpdatedOn = supProduct.supplierproduct_update ?? DateTime.Now
                        }
                };
            }
            return prodSuppliers;
        }

        /// <summary>
        /// Retrieves supplier-product combination unique id from the given supplier unique id and product unique id.
        /// </summary>
        /// <param name="SupplierId"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public long GetSupplierProductId(long SupplierId, long ProductId)
        {
            return (from sp in azureDB.SupplierProductEntities
                    where sp.supplier_id == SupplierId && sp.product_id == ProductId
                    select sp.supplierproduct_id).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves supplier-products for the given supplier name.
        /// </summary>
        /// <param name="SupplierName"></param>
        /// <returns></returns>
        public SupplierProducts GetSupplierProducts(string SupplierName)
        {
            var supplierProducts = (from sp in azureDB.SupplierProductEntities
                    where sp.Supplier.supplier_name.Equals(SupplierName, StringComparison.InvariantCultureIgnoreCase)
                    select sp);

            var prodSuppliers = new SupplierProducts();
            if (supplierProducts != null && supplierProducts.Count() > 0)
            {
                prodSuppliers.Supplier = supplierProducts.Select(sp => new Supplier()
                {
                    Name = sp.Supplier.supplier_name,
                    Domain = sp.Supplier.supplier_domain,
                    Type = sp.Supplier.supplier_type,
                    CreatedOn = sp.Supplier.supplier_create,
                    LastUpdatedOn = sp.Supplier.supplier_update ?? DateTime.Now
                }).FirstOrDefault();

                prodSuppliers.Products = supplierProducts.Select(sp => new RelatedProduct()
                {
                    Id = sp.supplierproduct_id,
                    Product = new Product()
                    {
                        ID = sp.product_id,
                        Name = sp.Product.product_name,
                        SKU = sp.Product.product_sku,
                        Price = sp.Product.product_price,
                        Manufacturer = sp.Product.product_manufacturer,
                        Description = sp.Product.product_description,
                        CreatedOn = sp.Product.product_create,
                        LastUpdatedOn = sp.Product.product_update ?? DateTime.Now
                    },
                    CreatedOn = sp.supplierproduct_create ?? DateTime.Now,
                    LastUpdatedOn = sp.supplierproduct_update ?? DateTime.Now
                }).ToList();
            }
            return prodSuppliers;
        }        

        /// <summary>
        /// Retrieves product suppliers for the given product name.
        /// </summary>
        /// <param name="ProductName"></param>
        /// <returns></returns>
        public ProductSuppliers GetProductSuppliers(string ProductName)
        {
            var supplierProducts = (from sp in azureDB.SupplierProductEntities
                                    where sp.Product.product_name.Equals(ProductName, StringComparison.InvariantCultureIgnoreCase)
                                    select sp);

            var prodSuppliers = new ProductSuppliers();

            if (supplierProducts != null && supplierProducts.Count() > 0)
            {
                prodSuppliers.Product = supplierProducts.Select(sp => new Product()
                {
                    ID = sp.Product.product_id,
                    Name = sp.Product.product_name,
                    Manufacturer = sp.Product.product_manufacturer,
                    Description = sp.Product.product_description,
                    CreatedOn = sp.Product.product_create,
                    LastUpdatedOn = sp.Product.product_update ?? DateTime.Now
                }).FirstOrDefault();

                prodSuppliers.Suppliers = supplierProducts.Select(sp => new RelatedSupplier()
                {
                    Id = sp.supplierproduct_id,
                    Supplier = new Supplier()
                    {
                        Id = sp.Supplier.supplier_id,
                        Name = sp.Supplier.supplier_name,
                        Domain = sp.Supplier.supplier_domain,
                        Type = sp.Supplier.supplier_type,
                        CreatedOn = sp.Supplier.supplier_create,
                        LastUpdatedOn = sp.Supplier.supplier_update ?? DateTime.Now
                    },
                    CreatedOn = sp.supplierproduct_create ?? DateTime.Now,
                    LastUpdatedOn = sp.supplierproduct_update ?? DateTime.Now
                }).ToList();
            }
            return prodSuppliers;
        }
        #endregion

        #region Supplier Offer service methods
        public long SaveSupplierOffer(string SupplierName, string ProductName, decimal PriceOffered, DateTime OfferValidUpto)
        {
            return supplierOfferSvc.SaveSupplierOffer(SupplierName, ProductName, PriceOffered, OfferValidUpto);
        }

        public bool RemoveSupplierOffer(string SupplierName, string ProductName, decimal PriceOffered, DateTime OfferValidUpto)
        {
            return supplierOfferSvc.RemoveSupplierOffer(SupplierName, ProductName, PriceOffered, OfferValidUpto);
        }

        public SupplierOffer GetSupplierOffer(string SupplierName, string ProductName, decimal PriceOffered, DateTime OfferValidUpto)
        {
            return supplierOfferSvc.GetSupplierOffer(SupplierName, ProductName, PriceOffered, OfferValidUpto);
        }

        public List<SupplierOffer> GetSupplierOffers(string SupplierName)
        {
            return supplierOfferSvc.GetSupplierOffers(SupplierName);
        }
        #endregion
    }
}
