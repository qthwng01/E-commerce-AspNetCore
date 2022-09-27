using System;
using System.Collections.Generic;

namespace DA_TOTNGHIEP.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string ContentComment { get; set; }
        public int? Rating { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int? ProductId { get; set; }
        public int? AccountId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifyAt { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Products Product { get; set; }
    }
}
