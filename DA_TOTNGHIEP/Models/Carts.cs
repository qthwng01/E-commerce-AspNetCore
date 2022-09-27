using System;
using System.Collections.Generic;

namespace DA_TOTNGHIEP.Models
{
    public partial class Carts
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Products Product { get; set; }
    }
}
