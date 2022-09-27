using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DA_TOTNGHIEP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using static DA_TOTNGHIEP.Helper;
using DA_TOTNGHIEP;
using DA_TOTNGHIEP.Data;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace DA_TOTNGHIEP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Sale")]
    public class ImageProductsController : Controller
    {
        private readonly DA_TOTNGHIEP2Context _context;
        private readonly IWebHostEnvironment _OK;
        private readonly INotyfService _notyf;

        public ImageProductsController(DA_TOTNGHIEP2Context context, IWebHostEnvironment OK, INotyfService notyf)
        {
            _context = context;
            _OK = OK;
            _notyf = notyf;
        }

        // GET: Admin/ImageProducts
        public async Task<IActionResult> Index()
        {
            ViewBag.CountStock = _context.Products.Sum(p => p.Stock).ToString("##,#0");
            ViewBag.CountProduct = (from ok in _context.Products select ok.Id).Count().ToString("##,#0");
            ViewBag.TongDoanhThu = _context.Invoices.Where(inv => inv.StatusId == 4).Sum(inv => inv.Total).ToString("##,#0 ₫");
            ViewBag.SoLuongDaBan = _context.InvoiceDetails.Where(inv => inv.MomoConfirm != "Giao dịch Momo bị huỷ" && inv.Invoice.StatusId == 4).Sum(inv => inv.Quantity).ToString("##,#0");

            var db_DOANTOTNGHIEPContext = _context.ImageProduct.Include(i => i.Products);
            return View(await db_DOANTOTNGHIEPContext.ToListAsync());
        }

        // GET: Admin/ImageProducts/Details/5
        public async Task<IActionResult> DetailImages(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageProduct = await _context.ImageProduct
                .Include(i => i.Products)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (imageProduct == null)
            {
                return NotFound();
            }
            return View(imageProduct);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Create(int id = 0)
        {
            ViewData["ProductsID"] = new SelectList(_context.Products, "Id", "Name");
            if (id == 0)
                return View(new ImageProduct());
            else
            {
                var imageProduct = await _context.ImageProduct.FindAsync(id);
                if (imageProduct == null)
                {
                    return NotFound();
                }
                return View(imageProduct);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Id,CodeImage,ImageName,ImageFile,ImageName1,ImageFile1,ImageName2,ImageFile2,ImageName3,ImageFile3,ImageName4,ImageFile4,ProductsId,Status")] ImageProduct imageProduct, IFormFile ImageFile, IFormFile ImageFile1, IFormFile ImageFile2, IFormFile ImageFile3, IFormFile ImageFile4)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    //ImageName
                    _context.Add(imageProduct);
                    ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", imageProduct.ProductsId);
                    await _context.SaveChangesAsync();
                    if (imageProduct.ImageFile != null)
                    {
                        var filename = imageProduct.CodeImage + "img" + imageProduct.Id.ToString() + Path.GetExtension(imageProduct.ImageFile.FileName);
                        var uploadpath = Path.Combine(_OK.WebRootPath, "assetsAdmin", "imgProducts");
                        var filepath = Path.Combine(uploadpath, filename);
                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            imageProduct.ImageFile.CopyTo(fs);
                            fs.Flush();
                        }
                        imageProduct.ImageName = filename;
                        _context.ImageProduct.Update(imageProduct);
                        ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", imageProduct.ProductsId);
                        await _context.SaveChangesAsync();
                    }
                    //ImageName 1
                    if (imageProduct.ImageFile1 != null)
                    {
                        var filename = imageProduct.CodeImage + "img1" + imageProduct.Id.ToString() + Path.GetExtension(imageProduct.ImageFile1.FileName);
                        var uploadpath = Path.Combine(_OK.WebRootPath, "assetsAdmin", "imgProducts");
                        var filepath = Path.Combine(uploadpath, filename);
                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            imageProduct.ImageFile1.CopyTo(fs);
                            fs.Flush();
                        }
                        imageProduct.ImageName1 = filename;
                        _context.ImageProduct.Update(imageProduct);
                        ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", imageProduct.ProductsId);
                        await _context.SaveChangesAsync();
                    }
                    //ImageName 2
                    if (imageProduct.ImageFile2 != null)
                    {
                        var filename = imageProduct.CodeImage + "img2" + imageProduct.Id.ToString() + Path.GetExtension(imageProduct.ImageFile2.FileName);
                        var uploadpath = Path.Combine(_OK.WebRootPath, "assetsAdmin", "imgProducts");
                        var filepath = Path.Combine(uploadpath, filename);
                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            imageProduct.ImageFile2.CopyTo(fs);
                            fs.Flush();
                        }
                        imageProduct.ImageName2 = filename;
                        _context.ImageProduct.Update(imageProduct);
                        ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", imageProduct.ProductsId);
                        await _context.SaveChangesAsync();
                    }
                    //ImageName 3
                    if (imageProduct.ImageFile3 != null)
                    {
                        var filename = imageProduct.CodeImage + "img3" + imageProduct.Id.ToString() + Path.GetExtension(imageProduct.ImageFile3.FileName);
                        var uploadpath = Path.Combine(_OK.WebRootPath, "assetsAdmin", "imgProducts");
                        var filepath = Path.Combine(uploadpath, filename);
                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            imageProduct.ImageFile3.CopyTo(fs);
                            fs.Flush();
                        }
                        imageProduct.ImageName3 = filename;
                        _context.ImageProduct.Update(imageProduct);
                        ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", imageProduct.ProductsId);
                        await _context.SaveChangesAsync();
                    }
                    //ImageName 4
                    if (imageProduct.ImageFile4 != null)
                    {
                        var filename = imageProduct.CodeImage + "img4" + imageProduct.Id.ToString() + Path.GetExtension(imageProduct.ImageFile4.FileName);
                        var uploadpath = Path.Combine(_OK.WebRootPath, "assetsAdmin", "imgProducts");
                        var filepath = Path.Combine(uploadpath, filename);
                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            imageProduct.ImageFile4.CopyTo(fs);
                            fs.Flush();
                        }
                        imageProduct.ImageName4 = filename;
                        _context.ImageProduct.Update(imageProduct);
                        ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", imageProduct.ProductsId);
                        await _context.SaveChangesAsync();
                    }
                }
                ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", imageProduct.ProductsId);
                _notyf.Success("Thêm hình ảnh thành công !!!", 5);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.ImageProduct.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", imageProduct) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id = 0)
        {
            ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name");
            if (id == 0)
                return View(new ImageProduct());

            else
            {
                var imageProduct = await _context.ImageProduct.FindAsync(id);
                if (imageProduct == null)
                {
                    return NotFound();
                }

                return View(imageProduct);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CodeImage,ImageName,ImageFile,ImageName1,ImageFile1,ImageName2,ImageFile2,ImageName3,ImageFile3,ImageName4,ImageFile4,ProductsId,Status")] ImageProduct imageProduct, IFormFile ImageFile, IFormFile ImageFile1, IFormFile ImageFile2, IFormFile ImageFile3, IFormFile ImageFile4)
        {
            if (ModelState.IsValid)
            {
                //ImageName
                if (imageProduct.ImageFile != null)
                {
                    Random rnd = new Random();
                    int num = rnd.Next(1, 999);
                    var filename = imageProduct.CodeImage + "imgeditmain" + imageProduct.Id.ToString() + num + Path.GetExtension(imageProduct.ImageFile.FileName);
                    var uploadpath = Path.Combine(_OK.WebRootPath, "assetsAdmin", "imgProducts");
                    var filepath = Path.Combine(uploadpath, filename);
                    using (FileStream fs = System.IO.File.Create(filepath))
                    {
                        imageProduct.ImageFile.CopyTo(fs);
                        fs.Flush();
                    }
                    imageProduct.ImageName = filename;
                    _context.Entry(imageProduct).State = EntityState.Modified;
                    ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", imageProduct.ProductsId);
                    await _context.SaveChangesAsync();
                }
                //ImageName 1
                if (imageProduct.ImageFile1 != null)
                {
                    Random rnd = new Random();
                    int num = rnd.Next(1, 999);
                    var filename = imageProduct.CodeImage + "imgedit1" + imageProduct.Id.ToString() + num + Path.GetExtension(imageProduct.ImageFile1.FileName);
                    var uploadpath = Path.Combine(_OK.WebRootPath, "assetsAdmin", "imgProducts");
                    var filepath = Path.Combine(uploadpath, filename);
                    using (FileStream fs = System.IO.File.Create(filepath))
                    {
                        imageProduct.ImageFile1.CopyTo(fs);
                        fs.Flush();
                    }
                    imageProduct.ImageName1 = filename;
                    _context.Entry(imageProduct).State = EntityState.Modified;
                    ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", imageProduct.ProductsId);
                    await _context.SaveChangesAsync();
                }
                //ImageName 2
                if (imageProduct.ImageFile2 != null)
                {
                    Random rnd = new Random();
                    int num = rnd.Next(1, 999);
                    var filename = imageProduct.CodeImage + "imgedit2" + imageProduct.Id.ToString() + num + Path.GetExtension(imageProduct.ImageFile2.FileName);
                    var uploadpath = Path.Combine(_OK.WebRootPath, "assetsAdmin", "imgProducts");
                    var filepath = Path.Combine(uploadpath, filename);
                    using (FileStream fs = System.IO.File.Create(filepath))
                    {
                        imageProduct.ImageFile2.CopyTo(fs);
                        fs.Flush();
                    }
                    imageProduct.ImageName2 = filename;
                    _context.Entry(imageProduct).State = EntityState.Modified;
                    ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", imageProduct.ProductsId);
                    await _context.SaveChangesAsync();
                }
                //ImageName 3
                if (imageProduct.ImageFile3 != null)
                {
                    Random rnd = new Random();
                    int num = rnd.Next(1, 999);
                    var filename = imageProduct.CodeImage + "imgedit3" + imageProduct.Id.ToString() + num + Path.GetExtension(imageProduct.ImageFile3.FileName);
                    var uploadpath = Path.Combine(_OK.WebRootPath, "assetsAdmin", "imgProducts");
                    var filepath = Path.Combine(uploadpath, filename);
                    using (FileStream fs = System.IO.File.Create(filepath))
                    {
                        imageProduct.ImageFile3.CopyTo(fs);
                        fs.Flush();
                    }
                    imageProduct.ImageName3 = filename;
                    _context.Entry(imageProduct).State = EntityState.Modified;
                    ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", imageProduct.ProductsId);
                    await _context.SaveChangesAsync();
                }
                //ImageName 4
                if (imageProduct.ImageFile4 != null)
                {
                    Random rnd = new Random();
                    int num = rnd.Next(1, 999);
                    var filename = imageProduct.CodeImage + "imgedit4" + imageProduct.Id.ToString() + num + Path.GetExtension(imageProduct.ImageFile4.FileName);
                    var uploadpath = Path.Combine(_OK.WebRootPath, "assetsAdmin", "imgProducts");
                    var filepath = Path.Combine(uploadpath, filename);
                    using (FileStream fs = System.IO.File.Create(filepath))
                    {
                        imageProduct.ImageFile4.CopyTo(fs);
                        fs.Flush();
                    }
                    imageProduct.ImageName4 = filename;
                    _context.Entry(imageProduct).State = EntityState.Modified;
                    ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", imageProduct.ProductsId);
                    await _context.SaveChangesAsync();
                }
                try
                {
                    _context.Update(imageProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageProductExists(imageProduct.Id))
                    { return NotFound(); }
                    else
                    { throw; }
                }
                ViewData["ProductsId"] = new SelectList(_context.Products, "Id", "Name", imageProduct.ProductsId);
                _notyf.Success("Sửa hình ảnh thành công !!!", 5);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.ImageProduct.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", imageProduct) });
        }


        // POST: Admin/ImageProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imageProduct = await _context.ImageProduct.FindAsync(id);
            _context.ImageProduct.Remove(imageProduct);
            await _context.SaveChangesAsync();
            _notyf.Success("Xóa hình ảnh thành công !!!", 5);
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.ImageProduct.ToList()) });
        }

        private bool ImageProductExists(int id)
        {
            return _context.ImageProduct.Any(e => e.Id == id);
        }
    }
}
