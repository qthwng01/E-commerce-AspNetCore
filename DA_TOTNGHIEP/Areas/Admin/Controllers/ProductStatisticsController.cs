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
    public class ProductStatisticsController : Controller
    {       
        private readonly DA_TOTNGHIEP2Context _context;
        public ProductStatisticsController(DA_TOTNGHIEP2Context context)
        {
            _context = context;
        }

        public IActionResult Index(string SearchString = "",string sortStockAscending = "", string sortStockDescending = "", string sortProductAscending = "", string sortProductDescending = "")
        {
            ViewBag.CountStock = _context.Products.Sum(p => p.Stock).ToString("##,#0");
            ViewBag.CountProduct = (from ok in _context.Products select ok.Id).Count().ToString("##,#0");
            ViewBag.TongDoanhThu = _context.Invoices.Where(inv => inv.StatusId == 4).Sum(inv => inv.Total).ToString("##,#0 ₫");
            ViewBag.SoLuongDaBan = _context.InvoiceDetails.Where(inv => inv.MomoConfirm != "Giao dịch Momo bị huỷ" && inv.Invoice.StatusId == 4).Sum(inv => inv.Quantity).ToString("##,#0");

            var db_DOANTOTNGHIEPContext = Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(_context.Invoices.Include(i => i.Account).Include(i => i.ProductItems).Include(i => i.User).Include(i => i.InvoiceDetails).ToList(), _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).Include(i => i.Product.ImageProductss).ToList());

            /*if (SearchString != "" || SearchString != null)
            {
                ViewBag.SearchString = SearchString;
                return View(Tuple.Create<IEnumerable<Invoices>,
                                                       IEnumerable<InvoiceDetails>>(
                                                        _context.Invoices.Include(i => i.Account).Include(i => i.ProductItems).Include(i => i.User).Include(i => i.InvoiceDetails).ToList(),
                                                        _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).Where(i => i.Product.Sku.Contains(SearchString) || i.Product.Name.Contains(SearchString)).ToList()));
            }   */ 

            //Sắp xếp SLTK theo thứ tự tăng - giảm
            ViewData["StockAscending"] = string.IsNullOrEmpty(sortStockAscending) ? "ascending" : "";
            switch (sortStockAscending)
            {
                case "ascending":
                   
                    break;
            }
            ViewData["StockDescending"] = string.IsNullOrEmpty(sortStockDescending) ? "descending" : "";
            switch (sortStockDescending)
            {
                case "descending":
                   
                    break;
            }

            //Sắp xếp SPBC theo thứ tự tăng - giảm
            ViewData["ProductAscending"] = string.IsNullOrEmpty(sortProductAscending) ? "ascending" : "";
            switch (sortProductAscending)
            {
                case "ascending":

                    break;
            }
            ViewData["ProductDescending"] = string.IsNullOrEmpty(sortProductDescending) ? "descending" : "";
            switch (sortProductDescending)
            {
                case "descending":

                    break;
            }

            return View(db_DOANTOTNGHIEPContext);
        }
    }
}
