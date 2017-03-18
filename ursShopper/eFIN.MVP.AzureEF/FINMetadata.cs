using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eFIN.MVP.AzureEF
{
    [MetadataType(typeof(SupplierHelper))]
    public partial class SupplierEntity {
        [Display(Name = "Offers")]
        public string offers { get; set; }
        [Display(Name = "Products")]
        public string products { get; set; }
    }

    public class SupplierHelper
    {
        [Display(Name = "Supplier ID")]
        public long supplier_id { get; set; }
        [Display(Name = "Supplier Domain")]
        public string supplier_domain { get; set; }
        [Display(Name = "Supplier Type")]
        public string supplier_type { get; set; }
        [Display(Name = "Supplier Name")]
        public string supplier_name { get; set; }

        [Display(Name = "Created Time")]
        public DateTime supplier_create { get; set; }
        [Display(Name = "Updated Time")]
        public DateTime supplier_update { get; set; }

     //   [Display(Name = "Supplier Products")]
    }


    [MetadataType(typeof(SupplierProductHelper))]
    public partial class SupplierProductEntity { }

    public class SupplierProductHelper
    {
        //[JsonIgnoreAttribute]
        //public Supplier Supplier { get; set; }
    }

    [MetadataType(typeof(ProductHelper))]
    public partial class ProductEntity { }

    public class ProductHelper
    {
        //[JsonIgnoreAttribute]
        //public virtual ICollection<SupplierProduct> SupplierProducts { get; set; }
    }

    [MetadataType(typeof(MarketerHelper))]
    public partial class MarketerEntity { }

    public class MarketerHelper
    {
        [Display(Name = "Marketer ID")]
        public long marketer_id { get; set; }
        [Display(Name = "Marketer Domain")]
        public string marketer_domain { get; set; }
        [Display(Name = "Marketer Type")]
        public string marketer_type { get; set; }
        [Display(Name = "Marketer Name")]
        public string marketer_name { get; set; }

        [Display(Name = "Created Time")]
        public DateTime marketer_create { get; set; }
        [Display(Name = "Updated Time")]
        public DateTime marketer_update { get; set; }

        //[Display(Name = "Marketer Products")]
        //[JsonIgnoreAttribute]
        //public virtual ICollection<MarketerProduct> MarketerProducts { get; set; }
    }

    [MetadataType(typeof(MarketerProductHelper))]
    public partial class MarketerProductEntity { }

    public class MarketerProductHelper
    {
        //[JsonIgnoreAttribute]
        //public Marketer Marketer { get; set; }
    }

    [MetadataType(typeof(ConsumerHelper))]
    public partial class ConsumerEntity {
        [Display(Name = "Name")]
        public string consumer_name { get { return string.Format("{0} {1} {2}", consumer_fname, consumer_mname, consumer_lname); } }
        [Display(Name = "Marketer Name")]
        public string marketer_name { get; set; }
        [Display(Name = "Offers")]
        public string offers { get; set; }
    }

    public class ConsumerHelper
    {
        [Display(Name = "Consumer ID")]
        public string consumer_id { get; set; }
        [Display(Name = "Email")]
        public string consumer_email { get; set; }
        [Display(Name = "First Name")]
        public string consumer_fname { get; set; }
        [Display(Name = "Middle Name")]
        public string consumer_mname { get; set; }
        [Display(Name = "Last Name")]
        public string consumer_lname { get; set; }
        [Display(Name = "Phone")]
        public string consumer_phone { get; set; }
        [Display(Name = "Status")]
        public string consumer_status { get; set; }
        [Display(Name = "Consumer Handle")]
        public string consumer_handle { get; set; }
    }

    [MetadataType(typeof(ConsumerOfferHelper))]
    public partial class ConsumerOfferEntity { }
    public class ConsumerOfferHelper
    {
        //[JsonIgnoreAttribute]
        //public Consumer Consumer { get; set; }
    }
}

