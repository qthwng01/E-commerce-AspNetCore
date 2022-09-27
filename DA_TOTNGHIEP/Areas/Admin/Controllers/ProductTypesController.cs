using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DA_TOTNGHIEP.Models;
using static DA_TOTNGHIEP.Helper;
using DA_TOTNGHIEP.Data;
using Microsoft.AspNetCore.Authorization;

namespace DA_TOTNGHIEP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Sale")]

    public class ProductTypesController : Controller
    {
        private readonly DA_TOTNGHIEP2Context _context;
        public ProductTypesController(DA_TOTNGHIEP2Context context)
        {
            _context = context;
        }
        
        // GET: Admin/ProductTypes
        public async Task<IActionResult> Index()
        {
            ViewBag.CountStock = _context.Products.Sum(p => p.Stock).ToString("##,#0");
            ViewBag.CountProduct = (from ok in _context.Products select ok.Id).Count().ToString("##,#0");
            ViewBag.TongDoanhThu = _context.Invoices.Where(inv => inv.StatusId == 4).Sum(inv => inv.Total).ToString("##,#0 ₫");
            ViewBag.SoLuongDaBan = _context.InvoiceDetails.Where(inv => inv.MomoConfirm != "Giao dịch Momo bị huỷ" && inv.Invoice.StatusId == 4).Sum(inv => inv.Quantity).ToString("##,#0");

            return View(await _context.ProductTypes.ToListAsync());
        }
        
        [NoDirectAccess]
        public async Task<IActionResult> Create(int id = 0)
        {
            if (id == 0)
                return View(new ProductTypes());

            else
            {
                var productTypes = await _context.ProductTypes.FindAsync(id);
                if (productTypes == null)
                {
                    return NotFound();
                }

                return View(productTypes);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Id,Name,Color,Status,CreatedAt")] ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    _context.Add(productTypes);
                    await _context.SaveChangesAsync();
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.ProductTypes.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", productTypes) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id = 0)
        {
            if (id == 0)
                return View(new ProductTypes());
            else
            {
                var productTypes = await _context.ProductTypes.FindAsync(id);
                if (productTypes == null)
                {
                    return NotFound();
                }
                return View(productTypes);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Color,Status,CreatedAt")] ProductTypes productTypes)
        {
                //Update
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(productTypes);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductTypesExists(productTypes.Id))
                        { return NotFound(); }
                        else
                        { throw; }
                    }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.ProductTypes.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", productTypes) });
        }

        // GET: Admin/ProductTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTypes = await _context.ProductTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productTypes == null)
            {
                return NotFound();
            }

            return View(productTypes);
        }

        // POST: Admin/ProductTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productTypes = await _context.ProductTypes.FindAsync(id);
            _context.ProductTypes.Remove(productTypes);
            await _context.SaveChangesAsync();
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.ProductTypes.ToList()) });
        }

        private bool ProductTypesExists(int id)
        {
            return _context.ProductTypes.Any(e => e.Id == id);
        }
    }
}
