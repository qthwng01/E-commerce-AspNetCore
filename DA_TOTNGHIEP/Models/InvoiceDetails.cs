using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DA_TOTNGHIEP.Models
{
    public partial class InvoiceDetails
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public string Payment { get; set; }
        public string? MomoConfirm { get; set; }

        public virtual Invoices Invoice { get; set; }
        public virtual Products Product { get; set; }
    }
}
