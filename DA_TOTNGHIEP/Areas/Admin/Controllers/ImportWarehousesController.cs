using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DA_TOTNGHIEP.Models;
using static DA_TOTNGHIEP.Helper;
using DA_TOTNGHIEP;
using DA_TOTNGHIEP.Data;
using Microsoft.AspNetCore.Authorization;

namespace DA_TOTNGHIEP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Sale")]

    public class ImportWarehousesController : Controller
    {
        private readonly DA_TOTNGHIEP2Context _context;

        public ImportWarehousesController(DA_TOTNGHIEP2Context context)
        {
            _context = context;
        }

        // GET: Admin/ImportWarehouses
        public IActionResult Index(DateTime fromDate, DateTime toDate, string SearchString = "")
        {
            ViewBag.CountStock = _context.Products.Sum(p => p.Stock).ToString("##,#0");
            ViewBag.CountProduct = (from ok in _context.Products select ok.Id).Count().ToString("##,#0");
            ViewBag.TongDoanhThu = _context.Invoices.Where(inv => inv.StatusId == 4).Sum(inv => inv.Total).ToString("##,#0 ₫");
            ViewBag.SoLuongDaBan = _context.InvoiceDetails.Where(inv => inv.MomoConfirm != "Giao dịch Momo bị huỷ" && inv.Invoice.StatusId == 4).Sum(inv => inv.Quantity).ToString("##,#0");

            if (SearchString != "" && SearchString != null)
            {
                if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    return View(_context.ImportWarehouse.Include(i => i.Warehouse).Where(i => i.CreatedAt.Date >= fromDate && i.CreatedAt.Date <= toDate && (i.Code.Contains(SearchString) || i.Supplier.Contains(SearchString) ||
                                                                                i.Shipment.Contains(SearchString) || i.ProductName.Contains(SearchString))).ToList());
                }
                else 
                { 
                    return View(_context.ImportWarehouse.Include(i => i.Warehouse).Where(i => i.Code.Contains(SearchString) || i.Supplier.Contains(SearchString) ||
                                                                                i.Shipment.Contains(SearchString) || i.ProductName.Contains(SearchString)).ToList());
                }
            }

            if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
            {
                return View(_context.ImportWarehouse.Include(i => i.Warehouse).Where(i => i.CreatedAt.Date >= fromDate && i.CreatedAt.Date <= toDate).ToList());
            }
            else if (fromDate.ToString("dd/MM/yyyy") == "01/01/0001" && toDate.ToString("dd/MM/yyyy") == "01/01/0001")
            {
                return View(_context.ImportWarehouse.Include(i => i.Warehouse).ToList());
            }

            var db_DOANTOTNGHIEPContext = _context.ImportWarehouse.Include(i => i.Warehouse);
            return View(db_DOANTOTNGHIEPContext);
        }

        // GET: Admin/ImportWarehouses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var importWarehouse = await _context.ImportWarehouse
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (importWarehouse == null)
            {
                return NotFound();
            }

            return View(importWarehouse);
        }

        // GET: Admin/ImportWarehouses/Create
        [NoDirectAccess]
        public async Task<IActionResult> Create(int id = 0)
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");
            if (id == 0)
                return View(new ImportWarehouse());

            else
            {
                var importWarehouse = await _context.ImportWarehouse.FindAsync(id);
                if (importWarehouse == null)
                {
                    return NotFound();
                }

                return View(importWarehouse);
            }
        }

        // POST: Admin/ImportWarehouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Id,Code,SupplierCode,Supplier,ProductName,Stock,WarehouseId,ProductId,Shipment,CreatedAt")] ImportWarehouse importWarehouse)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    _context.Add(importWarehouse);
                    ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", importWarehouse.WarehouseId);
                    await _context.SaveChangesAsync();
                }
                ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", importWarehouse.WarehouseId);
                return RedirectToAction("Index");
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", importWarehouse) });
        }

        // GET: Admin/ImportWarehouses/Edit/5
        [NoDirectAccess]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == 0)
                return View(new ImportWarehouse());

            else
            {
                var importWarehouse = await _context.ImportWarehouse.FindAsync(id);
                if (importWarehouse == null)
                {
                    return NotFound();
                }
                ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", importWarehouse.WarehouseId);
                return View(importWarehouse);
            }
        }

        // POST: Admin/ImportWarehouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,SupplierCode,Supplier,ProductName,Stock,WarehouseId,ProductId,Shipment,CreatedAt")] ImportWarehouse importWarehouse)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(importWarehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImportWarehouseExists(importWarehouse.Id))
                    { return NotFound(); }
                    else
                    { throw; }
                }
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", importWarehouse.WarehouseId);
                return RedirectToAction("Index");
            }   
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", importWarehouse) });
        }

       
        // POST: Admin/ImportWarehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var importWarehouse = await _context.ImportWarehouse.FindAsync(id);
            _context.ImportWarehouse.Remove(importWarehouse);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ImportWarehouseExists(int id)
        {
            return _context.ImportWarehouse.Any(e => e.Id == id);
        }
    }
}
