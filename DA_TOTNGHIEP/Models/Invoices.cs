using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DA_TOTNGHIEP.Models
{
    public partial class Invoices
    {
        public Invoices()
        {
            InvoiceDetails = new HashSet<InvoiceDetails>();
        }

        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "Mã HĐ")]
        public string Code { get; set; }
        public int? AccountId { get; set; }
        [Display(Name = "ID Người Dùng")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Display(Name = "Ngày Tạo")]
        public DateTime IssuedDate { get; set; }
        [Display(Name = "Địa Chỉ")]
        public string ShippingAddress { get; set; }
        [Display(Name = "SĐT")]
        public string ShippingPhone { get; set; }
        [Display(Name = "Tổng Tiền")]
        public int Total { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
        [Display(Name = "Họ và Tên")]
        public string Fullname { get; set; }
        [Display(Name = "Trạng Thái")]
        public int? StatusId { get; set; }
        public int? ProductItemsId { get; set; }
        public virtual Accounts Account { get; set; }
        public virtual Products ProductItems { get; set; }
        public virtual ICollection<InvoiceDetails> InvoiceDetails { get; set; }
    }
}
