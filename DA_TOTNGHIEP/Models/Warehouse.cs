using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DA_TOTNGHIEP.Models
{
    public partial class Warehouse
    {
        public Warehouse()
        {
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }
        [Display(Name = "Tên kho")]
        public string Name { get; set; }
        [Display(Name = "Tên lô")]
        public string ShipmentName { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        [Display(Name = "Số lượng nhập vào")]
        public int Stock { get; set; }
        [Display(Name = "Số lượng tồn kho")]
        public int? OnStock { get; set; }
        [Display(Name = "Mã phiếu nhập kho")]
        public int? ImportWarehouseId { get; set; }
        [Display(Name = "Sản phẩm")]
        public int? ProductId { get; set; }

        [Display(Name = "Mã phiếu nhập kho")]
        public virtual ImportWarehouse ImportWarehouse { get; set; }
        public virtual Products Product { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}
