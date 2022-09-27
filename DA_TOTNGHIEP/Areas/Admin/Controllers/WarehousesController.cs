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

    public class WarehousesController : Controller
    {
        private readonly DA_TOTNGHIEP2Context _context;

        public WarehousesController(DA_TOTNGHIEP2Context context)
        {
            _context = context;
        }

        // GET: Admin/Warehouses
        public async Task<IActionResult> Index()
        {
            ViewBag.CountStock = _context.Products.Sum(p => p.Stock).ToString("##,#0");
            ViewBag.CountProduct = (from ok in _context.Products select ok.Id).Count().ToString("##,#0");
            ViewBag.TongDoanhThu = _context.Invoices.Where(inv => inv.StatusId == 4).Sum(inv => inv.Total).ToString("##,#0 ₫");
            ViewBag.SoLuongDaBan = _context.InvoiceDetails.Where(inv => inv.MomoConfirm != "Giao dịch Momo bị huỷ" && inv.Invoice.StatusId == 4).Sum(inv => inv.Quantity).ToString("##,#0");

            var db_DOANTOTNGHIEPContext = _context.Warehouse.Include(w => w.ImportWarehouse).Include(w => w.Product);
            return View(await db_DOANTOTNGHIEPContext.ToListAsync());
        }

        // GET: Admin/Warehouses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouse
                .Include(w => w.ImportWarehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Create(int id = 0)
        {
            ViewData["ImportWarehouseId"] = new SelectList(_context.ImportWarehouse, "Id", "Code");
            if (id == 0)
                return View(new Warehouse());

            else
            {
                var warehouse = await _context.Warehouse.FindAsync(id);
                if (warehouse == null)
                {
                    return NotFound();
                }

                return View(warehouse);
            }
        }

        // POST: Admin/Warehouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Id,Name,Stock,ImportWarehouseId,ProductId")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var cc = _context.ImportWarehouse.FirstOrDefault(x => x.Id == warehouse.ImportWarehouseId);
                    if (warehouse.ImportWarehouseId != null)
                    {
                        warehouse.Stock = cc.Stock;
                        //warehouse.OnStock = cc.Stock;
                        warehouse.ShipmentName = cc.Shipment;
                    }

                    _context.Add(warehouse);
                    ViewData["ImportWarehouseId"] = new SelectList(_context.ImportWarehouse, "Id", "Code", warehouse.ImportWarehouseId);
                    await _context.SaveChangesAsync();
                }
                ViewData["ImportWarehouseId"] = new SelectList(_context.ImportWarehouse, "Id", "Code", warehouse.ImportWarehouseId);
                return RedirectToAction("Index");
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", warehouse) });
        }
        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id = 0)
        {
            ViewData["ImportWarehouseId"] = new SelectList(_context.ImportWarehouse, "Id", "Code");
            if (id == 0)
                return View(new Warehouse());

            else
            {
                var warehouse = await _context.Warehouse.FindAsync(id);
                if (warehouse == null)
                {
                    return NotFound();
                }

                return View(warehouse);
            }
        }

        // POST: Admin/Warehouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Stock,ImportWarehouseId,ProductId")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var cc = _context.ImportWarehouse.FirstOrDefault(x => x.Id == warehouse.ImportWarehouseId);
                    if (warehouse.ImportWarehouseId != null)
                    {
                        warehouse.Stock = cc.Stock;
                        /*warehouse.ShipmentName = cc.Shipment;*/
                    }

                    _context.Update(warehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseExists(warehouse.Id))
                    { return NotFound(); }
                    else
                    { throw; }
                }
                ViewData["ImportWarehouseId"] = new SelectList(_context.ImportWarehouse, "Id", "Code", warehouse.ImportWarehouseId);
                return RedirectToAction("Index");
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", warehouse) });
        }

        // GET: Admin/Warehouses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouse
                .Include(w => w.ImportWarehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // POST: Admin/Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouse = await _context.Warehouse.FindAsync(id);
            _context.Warehouse.Remove(warehouse);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool WarehouseExists(int id)
        {
            return _context.Warehouse.Any(e => e.Id == id);
        }
    }
}
