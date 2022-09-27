using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DA_TOTNGHIEP.Models
{
    public partial class Products
    {
        public Products()
        {
            Carts = new HashSet<Carts>();
            Comment = new HashSet<Comment>();
            ImageProduct = new HashSet<ImageProduct>();
            /*ImportWarehouse = new HashSet<ImportWarehouse>();*/
            InvoiceDetails = new HashSet<InvoiceDetails>();
            Invoices = new HashSet<Invoices>();
            WarehouseNavigation = new HashSet<Warehouse>();
        }

        /*public int Id { get; set; }
        [Display(Name = "SKU")]
        public string Sku { get; set; }
        [Display(Name = "Tên SP")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Giá bán")]
        public int Price { get; set; }
        [Display(Name = "Giá gốc")]
        public int ListedPrice { get; set; }
        [Display(Name = "Số lượng")]
        public int Stock { get; set; }
        [Display(Name = "Loại SP")]
        public int? ProductTypeId { get; set; }
        [Display(Name = "Hình ảnh SP")]
        public int? ImageProductId { get; set; }
        [Display(Name = "Số kho")]
        public int? WarehouseId { get; set; }
        [Display(Name = "Số lô")]
        public int? ShipmentId { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        public virtual ImageProduct ImageProductss { get; set; }
        [Display(Name = "Loại SP")]
        public virtual ProductTypes ProductType { get; set; }
        [Display(Name = "Số lô")]
        public virtual WarehouseDetails Shipments { get; set; }
        [Display(Name = "Kho")]
        public virtual Warehouse Warehouse { get; set; }
        public virtual ICollection<Carts> Carts { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<ImageProduct> ImageProduct { get; set; }
        public virtual ICollection<ImportWarehouse> ImportWarehouse { get; set; }
        public virtual ICollection<InvoiceDetails> InvoiceDetails { get; set; }
        public virtual ICollection<Invoices> Invoices { get; set; }*/

        public int Id { get; set; }
        [Display(Name = "SKU")]
        public string Sku { get; set; }
        [Display(Name = "Tên SP")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Giá bán")]
        public int Price { get; set; }
        [Display(Name = "Giá gốc")]
        public int ListedPrice { get; set; }
        [Display(Name = "SL tồn kho")]
        public int Stock { get; set; }
        [Display(Name = "Loại SP")]
        public int? ProductTypeId { get; set; }
        [Display(Name = "Hình ảnh SP")]
        public int? ImageProductId { get; set; }
        [Display(Name = "Kho")]
        public int? WarehouseId { get; set; }
        [Display(Name = "Lô")]
        public int? ShipmentId { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }


        [Display(Name = "Hình ảnh SP")]
        public virtual ImageProduct ImageProductss { get; set; }
        [Display(Name = "Loại SP")]
        public virtual ProductTypes ProductType { get; set; }
        [Display(Name = "Kho")]
        public virtual Warehouse Warehouse { get; set; }
        public virtual ICollection<Carts> Carts { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<ImageProduct> ImageProduct { get; set; }
        public virtual ICollection<InvoiceDetails> InvoiceDetails { get; set; }
        public virtual ICollection<Invoices> Invoices { get; set; }
        public virtual ICollection<Warehouse> WarehouseNavigation { get; set; }


    }

}
