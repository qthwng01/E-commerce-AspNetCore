using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DA_TOTNGHIEP.Models
{
    public partial class ProductTypes
    {
        public ProductTypes()
        {
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }
        [Display(Name = "Tên loại SP")]
        public string Name { get; set; }
        public string Color { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<Products> Products { get; set; }
    }

    public class ProductTypesChartModel
    {
        [JsonProperty(PropertyName = "NameList")]
        public List<string> NameList { get; set; }

        [JsonProperty(PropertyName = "ColorList")]
        public List<string> ColorList { get; set; }
    }
}
