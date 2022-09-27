using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DA_TOTNGHIEP.Data;
using DA_TOTNGHIEP.Models;
using static DA_TOTNGHIEP.Helper;
using DA_TOTNGHIEP.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace DA_TOTNGHIEP.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin,Sale")]
    public class StatiscalInvoicesController : Controller
    {       
        private readonly DA_TOTNGHIEP2Context _context;
        public StatiscalInvoicesController(DA_TOTNGHIEP2Context context)
        {
            _context = context;
        }

        public IActionResult Index(DateTime fromDate,DateTime toDate)
        {
            ViewBag.CountStock = _context.Products.Sum(p => p.Stock).ToString("##,#0");
            ViewBag.CountProduct = (from ok in _context.Products select ok.Id).Count().ToString("##,#0");
            ViewBag.TongDoanhThu = _context.Invoices.Where(inv => inv.StatusId == 4).Sum(inv => inv.Total).ToString("##,#0 ₫");
            ViewBag.SoLuongDaBan = _context.InvoiceDetails.Where(inv => inv.MomoConfirm != "Giao dịch Momo bị huỷ" && inv.Invoice.StatusId == 4).Sum(inv => inv.Quantity).ToString("##,#0");

            var countInvoice = _context.Invoices.Where( x => x.StatusId != 3 && x.IssuedDate.Date >= fromDate && x.IssuedDate.Date <= toDate).Count();
            ViewBag.countInvoices = countInvoice;
            var countUnApprove = _context.Invoices.Where(x => x.StatusId == 0 && x.IssuedDate.Date >= fromDate && x.IssuedDate.Date <= toDate).Count();
            ViewBag.countUnApprove = countUnApprove;
            var countShip = _context.Invoices.Where(x => x.StatusId == 1 && x.IssuedDate.Date >= fromDate && x.IssuedDate.Date <= toDate).Count();
            ViewBag.countShip = countShip;
            var countCancel = _context.Invoices.Where(x => x.StatusId == 2 && x.IssuedDate.Date >= fromDate && x.IssuedDate.Date <= toDate).Count();
            ViewBag.countCancel = countCancel;
            var countApproved = _context.Invoices.Where(x => x.StatusId == 4 && x.IssuedDate.Date >= fromDate && x.IssuedDate.Date <= toDate).Count();
            ViewBag.countApproved = countApproved;
            var doanhThu = _context.Invoices.Where(x => x.StatusId == 4 && x.IssuedDate.Date >= fromDate && x.IssuedDate.Date <= toDate).Select(x => x.Total).Sum();
            ViewBag.doanhThu = doanhThu;

            /*var today = DateTime.Today;
            var total = _context.Invoices.Where(x => x.IssuedDate.Date >= fromDate && x.IssuedDate.Date <= toDate && x.StatusId == 4)
                .Select(x => x.Total).Sum().ToString("##,# ₫");
            ViewBag.total = total;*/
            return View();
        }
    }
}
