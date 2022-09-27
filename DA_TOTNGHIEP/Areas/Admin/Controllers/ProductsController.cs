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

    public class ProductsController : Controller
    {
        private readonly DA_TOTNGHIEP2Context _context;

        public ProductsController(DA_TOTNGHIEP2Context context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public IActionResult Index(string SearchName = "", string SearchType = "")
        {
            ViewBag.CountStock = _context.Products.Sum(p => p.Stock).ToString("##,#0");
            ViewBag.CountProduct = (from ok in _context.Products select ok.Id).Count().ToString("##,#0");
            ViewBag.TongDoanhThu = _context.Invoices.Where(inv => inv.StatusId == 4).Sum(inv => inv.Total).ToString("##,#0 ₫");
            ViewBag.SoLuongDaBan = _context.InvoiceDetails.Where(inv => inv.MomoConfirm != "Giao dịch Momo bị huỷ" && inv.Invoice.StatusId == 4).Sum(inv => inv.Quantity).ToString("##,#0");

            var db_DOANTOTNGHIEPContext = _context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse).ToList();
            if (SearchType == "" || SearchType == null)
            {
                if (SearchName != "" && SearchName != null)
                {
                    db_DOANTOTNGHIEPContext = _context.Products.Where(prd => prd.Name.Contains(SearchName) || prd.Sku.Contains(SearchName)).ToList();
                    return View(db_DOANTOTNGHIEPContext);
                }
            }
            if (SearchType == "Apple" && SearchType != "")
            {
                if (SearchName != "" && SearchName != null)
                {
                    db_DOANTOTNGHIEPContext = _context.Products.Where(prd => prd.Name.Contains(SearchName) || prd.Sku.Contains(SearchName) && prd.ProductType.Name.Contains(SearchType)).ToList();
                    return View(db_DOANTOTNGHIEPContext);
                }
                else
                {
                    db_DOANTOTNGHIEPContext = _context.Products.Where(prd =>prd.ProductType.Name.Contains(SearchType)).ToList();
                    return View(db_DOANTOTNGHIEPContext);
                }
            }
            if (SearchType == "SamSung" && SearchType != "")
            {
                if (SearchName != "" && SearchName != null)
                {
                    db_DOANTOTNGHIEPContext = _context.Products.Where(prd => prd.Name.Contains(SearchName) || prd.Sku.Contains(SearchName) && prd.ProductType.Name.Contains(SearchType)).ToList();
                    return View(db_DOANTOTNGHIEPContext);
                }
                else
                {
                    db_DOANTOTNGHIEPContext = _context.Products.Where(prd => prd.ProductType.Name.Contains(SearchType)).ToList();
                    return View(db_DOANTOTNGHIEPContext);
                }
            }
            if (SearchType == "Xiaomi" && SearchType != "")
            {
                if (SearchName != "" && SearchName != null)
                {
                    db_DOANTOTNGHIEPContext = _context.Products.Where(prd => prd.Name.Contains(SearchName) || prd.Sku.Contains(SearchName) && prd.ProductType.Name.Contains(SearchType)).ToList();
                    return View(db_DOANTOTNGHIEPContext);
                }
                else
                {
                    db_DOANTOTNGHIEPContext = _context.Products.Where(prd => prd.ProductType.Name.Contains(SearchType)).ToList();
                    return View(db_DOANTOTNGHIEPContext);
                }
            }
            if (SearchType == "Oppo" && SearchType != "")
            {
                if (SearchName != "" && SearchName != null)
                {
                    db_DOANTOTNGHIEPContext = _context.Products.Where(prd => prd.Name.Contains(SearchName) || prd.Sku.Contains(SearchName) && prd.ProductType.Name.Contains(SearchType)).ToList();
                    return View(db_DOANTOTNGHIEPContext);
                }
                else
                {
                    db_DOANTOTNGHIEPContext = _context.Products.Where(prd => prd.ProductType.Name.Contains(SearchType)).ToList();
                    return View(db_DOANTOTNGHIEPContext);
                }
            }
            if (SearchType == "Nokia" && SearchType != "")
            {
                if (SearchName != "" && SearchName != null)
                {
                    db_DOANTOTNGHIEPContext = _context.Products.Where(prd => prd.Name.Contains(SearchName) || prd.Sku.Contains(SearchName) && prd.ProductType.Name.Contains(SearchType)).ToList();
                    return View(db_DOANTOTNGHIEPContext);
                }
                else
                {
                    db_DOANTOTNGHIEPContext = _context.Products.Where(prd => prd.ProductType.Name.Contains(SearchType)).ToList();
                    return View(db_DOANTOTNGHIEPContext);
                }
            }

            return View(db_DOANTOTNGHIEPContext);
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.ImageProductss)
                .Include(p => p.ProductType)
                .Include(p => p.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }


        [NoDirectAccess]
        public async Task<IActionResult> Create(int id = 0)
        {
            ViewData["ImageProductId"] = new SelectList(_context.ImageProduct, "Id", "CodeImage");
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");
            if (id == 0)
                return View(new Products());

            else
            {
                var imageProduct = await _context.Products.FindAsync(id);
                if (imageProduct == null)
                {
                    return NotFound();
                }

                return View(imageProduct);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Id,Sku,Name,Description,Price,ListedPrice,Stock,ImageProductId,ProductTypeId,WarehouseId,ShipmentId,Status")] Products products)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    //products.Stock = 1;

                    var cc = _context.Warehouse.Include(x => x.Products).FirstOrDefault(x => x.Id == products.WarehouseId);

                    if (products.WarehouseId != null)
                    {

                        products.Stock = cc.Stock;

                    }

                    _context.Add(products);
                    ViewData["ImageProductId"] = new SelectList(_context.ImageProduct, "Id", "CodeImage", products.ImageProductId);
                    ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", products.ProductTypeId);
                    ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", products.WarehouseId);
                    await _context.SaveChangesAsync();
                }
                ViewData["ImageProductId"] = new SelectList(_context.ImageProduct, "Id", "CodeImage", products.ImageProductId);
                ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", products.ProductTypeId);
                ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", products.WarehouseId);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Products.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", products) });
        }


        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id = 0)
        {
            ViewData["ImageProductId"] = new SelectList(_context.ImageProduct, "Id", "CodeImage");
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");
            if (id == 0)
                return View(new Products());

            else
            {
                var products = await _context.Products.FindAsync(id);
                if (products == null)
                {
                    return NotFound();
                }

                return View(products);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sku,Name,Description,Price,ListedPrice,Stock,ImageProductId,ProductTypeId,WarehouseId,ShipmentId,Status")] Products products)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Id))
                    { return NotFound(); }
                    else
                    { throw; }
                }
                ViewData["ImageProductId"] = new SelectList(_context.ImageProduct, "Id", "CodeImage", products.ImageProductId);
                ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", products.ProductTypeId);
                ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", products.WarehouseId);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Products.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", products) });
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Products.ToList()) });
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
