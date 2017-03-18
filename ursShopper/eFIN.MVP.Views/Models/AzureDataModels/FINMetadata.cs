using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eFIN.MVP.AzureEF;

namespace eFIN.MVP.Views.Models.AzureDataModels
{
    [MetadataType(typeof(SupplierEntityHelper))]
    public partial class SupplierEntity { }

    public class SupplierEntityHelper
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

        [Display(Name = "Supplier ProductEntities")]
        [JsonIgnoreAttribute]
        public virtual ICollection<SupplierProductEntity> SupplierProductEntities { get; set; }
    }


    [MetadataType(typeof(SupplierEntityProductHelper))]
    public partial class SupplierProductEntity{ }

    public class SupplierEntityProductHelper
    {
        [JsonIgnoreAttribute]
        public SupplierEntity SupplierEntity { get; set; }
    }

    [MetadataType(typeof(ProductHelper))]
    public partial class ProductEntity { }

    public class ProductHelper
    {
        [JsonIgnoreAttribute]
        public virtual ICollection<SupplierProductEntity> SupplierProductEntities { get; set; }
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

        [Display(Name = "Marketer ProductEntities")]
        [JsonIgnoreAttribute]
        public virtual ICollection<MarketerProductEntity> MarketerProductEntities { get; set; }
    }

    [MetadataType(typeof(MarketerProductHelper))]
    public partial class MarketerProductEntity { }

    public class MarketerProductHelper
    {
        [JsonIgnoreAttribute]
        public MarketerEntity MarketerEntity { get; set; }
    }

    [MetadataType(typeof(ConsumerEntityHelper))]
    public partial class ConsumerEntity {
        [Display(Name = "Name")]
        public string consumer_name { get { return string.Format("{0} {1} {2}", consumer_fname, consumer_mname, consumer_lname); } }
    }

    public class ConsumerEntityHelper
    {
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
    }

    [MetadataType(typeof(ConsumerEntityOfferHelper))]
    public partial class ConsumerOfferEntity { }
    public class ConsumerEntityOfferHelper
    {
        [JsonIgnoreAttribute]
        public ConsumerEntity ConsumerEntity { get; set; }
    }
}

