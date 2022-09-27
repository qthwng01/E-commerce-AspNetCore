using DA_TOTNGHIEP.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DA_TOTNGHIEP.Helper;

namespace DA_TOTNGHIEP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Sale")]
    public class DashboardController : Controller
    {
        private readonly DA_TOTNGHIEP2Context _context;
        public DashboardController(DA_TOTNGHIEP2Context context)
        {
            _context = context;
        }

        
        public IActionResult Index(DateTime fromDate, DateTime toDate)
        {
            ViewBag.CountStock = _context.Products.Sum(p => p.Stock).ToString("##,#0");
            ViewBag.CountProduct = (from ok in _context.Products select ok.Id).Count().ToString("##,#0");
            ViewBag.TongDoanhThu = _context.Invoices.Where(inv => inv.StatusId == 4).Sum(inv => inv.Total).ToString("##,#0 ₫");
            ViewBag.SoLuongDaBan = _context.InvoiceDetails.Where(inv => inv.MomoConfirm != "Giao dịch Momo bị huỷ" && inv.Invoice.StatusId == 4).Sum(inv => inv.Quantity).ToString("##,#0");

            var total6 = _context.Invoices.Where(x => x.IssuedDate.Date == DateTime.Today.AddDays(-6) && x.StatusId == 4)
               .Select(x => x.Total).Sum();
            var total5 = _context.Invoices.Where(x => x.IssuedDate.Date == DateTime.Today.AddDays(-5) && x.StatusId == 4)
               .Select(x => x.Total).Sum();
            var total4 = _context.Invoices.Where(x => x.IssuedDate.Date == DateTime.Today.AddDays(-4) && x.StatusId == 4)
               .Select(x => x.Total).Sum();
            var total3 = _context.Invoices.Where(x => x.IssuedDate.Date == DateTime.Today.AddDays(-3) && x.StatusId == 4)
               .Select(x => x.Total).Sum();
            var total2 = _context.Invoices.Where(x => x.IssuedDate.Date == DateTime.Today.AddDays(-2) && x.StatusId == 4)
               .Select(x => x.Total).Sum();
            var total1 = _context.Invoices.Where(x => x.IssuedDate.Date == DateTime.Today.AddDays(-1) && x.StatusId == 4)
               .Select(x => x.Total).Sum();
            var totalToday = _context.Invoices.Where(x => x.IssuedDate.Date == DateTime.Today && x.StatusId == 4)
               .Select(x => x.Total).Sum();

            var dateTime6 = DateTime.Today.AddDays(-6).ToShortDateString().ToString();
            var dateTime5 = DateTime.Today.AddDays(-5).ToShortDateString().ToString();
            var dateTime4 = DateTime.Today.AddDays(-4).ToShortDateString().ToString();
            var dateTime3 = DateTime.Today.AddDays(-3).ToShortDateString().ToString();
            var dateTime2 = DateTime.Today.AddDays(-2).ToShortDateString().ToString();
            var dateTime1 = DateTime.Today.AddDays(-1).ToShortDateString().ToString();
            var dateTimeToday = DateTime.Today.ToShortDateString().ToString();

            var countTotal6 = total6.ToString();
            var countTotal5 = total5.ToString();
            var countTotal4 = total4.ToString();
            var countTotal3 = total3.ToString();
            var countTotal2 = total2.ToString();
            var countTotal1 = total1.ToString();
            var countTotalToday = totalToday.ToString();

            ViewBag.dateTime6 = dateTime6;
            ViewBag.dateTime5 = dateTime5;
            ViewBag.dateTime4 = dateTime4;
            ViewBag.dateTime3 = dateTime3;
            ViewBag.dateTime2 = dateTime2;
            ViewBag.dateTime1 = dateTime1;
            ViewBag.dateTimeToday = dateTimeToday;

            ViewBag.countTotal6 = countTotal6;
            ViewBag.countTotal5 = countTotal5;
            ViewBag.countTotal4 = countTotal4;
            ViewBag.countTotal3 = countTotal3;
            ViewBag.countTotal2 = countTotal2;
            ViewBag.countTotal1 = countTotal1;
            ViewBag.countTotalToday = countTotalToday;

            var total = _context.Invoices.Where(x => x.IssuedDate.Date >= fromDate && x.IssuedDate.Date <= toDate && x.StatusId == 4)
                .Select(x => x.Total).Sum().ToString();
            ViewBag.total = total;
            ViewBag.fromDate = fromDate.Date.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.Date.ToString("dd/MM/yyyy");

            return View();
        }
        
        /*[HttpGet]
        public IActionResult GetChart()
        {
            var date = _context.ProductTypes.Select(x => x.CreatedAt).Distinct().ToList();
            var status = _context.ProductTypes
                .Where(p => p.Status == true)
                .GroupBy(j => j.CreatedAt)
                .Select(group => new
                {
                    CreatedAt = group.Key,
                    Count = group.Count()
                });
            var countStatus = status.Select(a => a.Count).ToArray();
            var notstatus = _context.ProductTypes
                .Where(p => p.Status == false)
                .GroupBy(j => j.CreatedAt)
                .Select(group => new
                {
                    CreatedAt = group.Key,
                    Count = group.Count()
                });
            var countnotstatus = notstatus.Select(a => a.Count).ToArray();

            return new JsonResult(new { myStatus = countStatus, myNotStatus = notstatus, myDate = date });
        }*/

        /*[HttpGet]
        public IActionResult ChartInvoices()
        {
            var Approved = _context.Invoices
                .Where(i => i.StatusId == 0)
                .GroupBy(j => j.IssuedDate)
                .Select(group => new
                {
                    IssuedDate = group.Key,
                    Count = group.Count()
                });


            var countApproved = Approved.Select(a => a.Count).ToArray();
            
            var invCanceled = _context.Invoices
                .Where(i => i.StatusId == 2)                                                    
                .GroupBy(j => j.IssuedDate)
                .Select(group => new
                {
                    IssuedDate = group.Key,
                    Count = group.Count()
                });

            var countinvCanceled = Approved.Select(a => a.Count).ToArray();


            *//*float ok = (float) (countinvCanceled.Count() / (float)(countApproved.Count() + (float)countinvCanceled.Count()) * 100);
            float ko = (float) (countApproved.Count() / (float)(countApproved.Count() + (float)countinvCanceled.Count()) * 100);*//*
            
            return new JsonResult(new { jsonApproved = countApproved, jsoninvCanceld = countinvCanceled });
        }*/
    }
}
