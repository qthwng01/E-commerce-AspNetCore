using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DA_TOTNGHIEP.Models
{
    public partial class ImportWarehouse
    {
        public ImportWarehouse()
        {
            Warehouse = new HashSet<Warehouse>();
        }

        public int Id { get; set; }
        [Display(Name = "Mã phiếu nhập")]
        public string Code { get; set; }
        [Display(Name = "Mã nhà cung cấp")]
        public int SupplierCode { get; set; }
        [Display(Name = "Tên nhà cung cấp")]
        public string Supplier { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }
        [Display(Name = "Tên lô")]
        public string Shipment { get; set; }
        [Display(Name = "Số lượng")]
        public int Stock { get; set; }
        [Display(Name = "Kho")]
        public int? WarehouseId { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /*public virtual Products Product { get; set; }
        public virtual WarehouseDetails Shipments { get; set; }*/
        public virtual ICollection<Warehouse> Warehouse { get; set; }
    }
}
