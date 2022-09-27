using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA_TOTNGHIEP.Models
{
    public partial class ImageProduct
    {
        public ImageProduct()
        {
            Productss = new HashSet<Products>();
        }

        public int Id { get; set; }
        [Display(Name = "Mã hình ảnh")]
        public string CodeImage { get; set; }
        [Display(Name = "Hình ảnh chính")]
        public string? ImageName { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [Display(Name = "Hình ảnh 1")]
        public string? ImageName1 { get; set; }
        [NotMapped]
        public IFormFile ImageFile1 { get; set; }
        [Display(Name = "Hình ảnh 2")]
        public string? ImageName2 { get; set; }
        [NotMapped]
        public IFormFile ImageFile2 { get; set; }
        [Display(Name = "Hình ảnh 3")]
        public string? ImageName3 { get; set; }
        [NotMapped]
        public IFormFile ImageFile3 { get; set; }
        [Display(Name = "Hình ảnh 4")]
        public string? ImageName4 { get; set; }
        [NotMapped]
        public IFormFile ImageFile4 { get; set; }
        public int? ProductsId { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }


        public virtual Products Products { get; set; }
        public virtual ICollection<Products> Productss { get; set; }
    }
}
