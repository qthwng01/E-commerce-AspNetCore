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
using AspNetCoreHero.ToastNotification.Abstractions;
using DA_TOTNGHIEP.Services;

namespace DA_TOTNGHIEP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class InvoicesController : Controller
    {
        private readonly DA_TOTNGHIEP2Context _context;
        private readonly INotyfService _notyf;
        private readonly IMailService mailService;

        public InvoicesController(DA_TOTNGHIEP2Context context, INotyfService notyf, IMailService mailService)
        {
            _context = context;
            _notyf = notyf;
            this.mailService = mailService;
        }
        // GET: Admin/Invoices
        public IActionResult Index(DateTime fromDate, DateTime toDate, string SearchString = "", string Status = "")
        {
            ViewBag.CountStock = _context.Products.Sum(p => p.Stock).ToString("##,#0");
            ViewBag.CountProduct = (from ok in _context.Products select ok.Id).Count().ToString("##,#0");
            ViewBag.TongDoanhThu = _context.Invoices.Where(inv => inv.StatusId == 4).Sum(inv => inv.Total).ToString("##,#0 ₫");
            ViewBag.SoLuongDaBan = _context.InvoiceDetails.Where(inv => inv.MomoConfirm != "Giao dịch Momo bị huỷ" && inv.Invoice.StatusId == 4).Sum(inv => inv.Quantity).ToString("##,#0");
            List<Invoices> invoices;

            if (Status == "" || Status == null)
            {
                if (SearchString != "" && SearchString != null)
                {
                    if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        invoices = _context.Invoices.Where(inv => inv.StatusId != 0 && inv.IssuedDate.Date >= fromDate && inv.IssuedDate.Date <= toDate && (inv.Code.Contains(SearchString) || inv.ShippingPhone.Contains(SearchString) || inv.ShippingAddress.Contains(SearchString))).OrderByDescending(i => i.IssuedDate).ToList();
                        return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices, _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                    }
                    else
                    {
                        invoices = _context.Invoices.Where(inv => inv.StatusId != 0 && (inv.Code.Contains(SearchString) || inv.ShippingPhone.Contains(SearchString) || inv.ShippingAddress.Contains(SearchString))).ToList();
                        return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices, _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));

                    }
                }   
            }
            if (Status == "Đang giao hàng" && Status != null)
            {
                if (SearchString != "" && SearchString != null)
                {
                    if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        invoices = _context.Invoices.Where(inv => inv.StatusId == 1 && inv.IssuedDate.Date >= fromDate && inv.IssuedDate.Date <= toDate && (inv.Code.Contains(SearchString) || inv.ShippingPhone.Contains(SearchString) || inv.ShippingAddress.Contains(SearchString))).OrderByDescending(i => i.IssuedDate).ToList();
                        return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices, _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                    }
                    else
                    {
                        invoices = _context.Invoices.Where(inv => inv.StatusId == 1 && (inv.Code.Contains(SearchString) || inv.ShippingPhone.Contains(SearchString) || inv.ShippingAddress.Contains(SearchString))).OrderByDescending(i => i.IssuedDate).ToList();
                        return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices, _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                    }
                }
                else
                {
                    invoices = _context.Invoices.Where(inv => inv.StatusId == 1).ToList();
                    return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices, _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                }
            }
            if (Status == "Huỷ đơn" && Status != null)
            {
                if (SearchString != "" && SearchString != null)
                {
                    if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        invoices = _context.Invoices.Where(inv => inv.StatusId == 2 && inv.IssuedDate.Date >= fromDate && inv.IssuedDate.Date <= toDate && (inv.Code.Contains(SearchString) || inv.ShippingPhone.Contains(SearchString) || inv.ShippingAddress.Contains(SearchString))).OrderByDescending(i => i.IssuedDate).ToList();
                        return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices, _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                    }
                    else
                    {
                        invoices = _context.Invoices.Where(inv => inv.StatusId == 2 && (inv.Code.Contains(SearchString) || inv.ShippingPhone.Contains(SearchString) || inv.ShippingAddress.Contains(SearchString))).OrderByDescending(i => i.IssuedDate).ToList();
                        return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices, _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                    }
                }
                else
                {
                    invoices = _context.Invoices.Where(inv => inv.StatusId == 2).ToList();
                    return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices, _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                }
            }
            if (Status == "Giao dịch không hoàn thành" && Status != null)
            {
                if (SearchString != "" && SearchString != null)
                {
                    if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        invoices = _context.Invoices.Where(inv => inv.StatusId == 3 && inv.IssuedDate.Date >= fromDate && inv.IssuedDate.Date <= toDate && (inv.Code.Contains(SearchString) || inv.ShippingPhone.Contains(SearchString) || inv.ShippingAddress.Contains(SearchString))).OrderByDescending(i => i.IssuedDate).ToList();
                        return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices, _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                    }
                    else
                    {
                        invoices = _context.Invoices.Where(inv => inv.StatusId == 3 && (inv.Code.Contains(SearchString) || inv.ShippingPhone.Contains(SearchString) || inv.ShippingAddress.Contains(SearchString))).OrderByDescending(i => i.IssuedDate).ToList();
                        return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices, _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                    }
                }
                else
                {
                    invoices = _context.Invoices.Where(inv => inv.StatusId == 3).ToList();
                    return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices, _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                }
            }
            if (Status == "Đã giao" && Status != null)
            {
                if (SearchString != "" && SearchString != null)
                {
                    if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        invoices = _context.Invoices.Where(inv => inv.StatusId == 4 && inv.IssuedDate.Date >= fromDate && inv.IssuedDate.Date <= toDate && (inv.Code.Contains(SearchString) || inv.ShippingPhone.Contains(SearchString) || inv.ShippingAddress.Contains(SearchString))).OrderByDescending(i => i.IssuedDate).ToList();
                        return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices, _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                    }
                    else
                    {
                        invoices = _context.Invoices.Where(inv => inv.StatusId == 4 && (inv.Code.Contains(SearchString) || inv.ShippingPhone.Contains(SearchString) || inv.ShippingAddress.Contains(SearchString))).OrderByDescending(i => i.IssuedDate).ToList();
                        return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices, _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                    }
                }
                else
                {
                    invoices = _context.Invoices.Where(inv => inv.StatusId == 4).ToList();
                    return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices, _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                }
            }

            if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
            {
                return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(_context.Invoices.Include(i => i.Account).Include(i => i.ProductItems)
                            .Include(i => i.User).Include(i => i.InvoiceDetails).Where(i => i.StatusId != 0 && i.IssuedDate.Date >= fromDate && i.IssuedDate.Date <= toDate).OrderByDescending(i => i.IssuedDate).ToList()
                            , _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
            }
            else if (fromDate.ToString("dd/MM/yyyy") == "01/01/0001" && toDate.ToString("dd/MM/yyyy") == "01/01/0001")
            { 
                return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(_context.Invoices.Include(i => i.Account).Include(i => i.ProductItems).Include(i => i.User).Include(i => i.InvoiceDetails).Where(i => i.StatusId != 0).OrderByDescending(i => i.IssuedDate).ToList(), _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList())); 
            }    

            return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(_context.Invoices.Include(i => i.Account).Include(i => i.ProductItems).Include(i => i.User).Include(i => i.InvoiceDetails).Where(i => i.StatusId != 0).OrderByDescending(i => i.IssuedDate).ToList(), _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
        }


        public IActionResult Pending(DateTime fromDate, DateTime toDate, string SearchString = "", string Status = "")
        {
            ViewBag.CountStock = _context.Products.Sum(p => p.Stock).ToString("##,#0");
            ViewBag.CountProduct = (from ok in _context.Products select ok.Id).Count().ToString("##,#0");
            ViewBag.TongDoanhThu = _context.Invoices.Sum(inv => inv.Total).ToString("##,#0 ₫");
            ViewBag.SoLuongDaBan = _context.InvoiceDetails.Sum(inv => inv.Quantity).ToString("##,#0");
            List<Invoices> invoices;
            if (SearchString != "" && SearchString != null)
            {
                if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    invoices = _context.Invoices.Where(i => i.StatusId == 0 && i.IssuedDate.Date >= fromDate && i.IssuedDate.Date <= toDate && (i.Code.Contains(SearchString) || i.ShippingAddress.Contains(SearchString) || i.ShippingPhone.Contains(SearchString))).ToList();
                    return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices.OrderByDescending(i => i.IssuedDate), _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                }
                else {
                invoices = _context.Invoices.Where(i => i.StatusId == 0 && (i.Code.Contains(SearchString) || i.ShippingAddress.Contains(SearchString) || i.ShippingPhone.Contains(SearchString))).ToList();
                return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices.OrderByDescending(i => i.IssuedDate), _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
                }
            }
            if (Status == "Chờ duyệt" && Status != null)
            {
                invoices = _context.Invoices.Where(inv => inv.StatusId == 0).ToList<Invoices>();
                return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(invoices.OrderByDescending(i => i.IssuedDate), _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
            }

            if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
            {
                return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(_context.Invoices.Include(i => i.Account).Include(i => i.ProductItems)
                            .Include(i => i.User).Include(i => i.InvoiceDetails).Where(i => i.StatusId == 0 && i.IssuedDate.Date >= fromDate && i.IssuedDate.Date <= toDate).OrderByDescending(i => i.IssuedDate).ToList()
                            , _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
            }
            else if (fromDate.ToString("dd/MM/yyyy") == "01/01/0001" && toDate.ToString("dd/MM/yyyy") == "01/01/0001")
            {
                return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(_context.Invoices.Include(i => i.Account).Include(i => i.ProductItems).Include(i => i.User).Include(i => i.InvoiceDetails).Where(i => i.StatusId == 0).OrderByDescending(i => i.IssuedDate).ToList(), _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
            }

            return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(_context.Invoices.Include(i => i.Account).Include(i => i.ProductItems).Include(i => i.User).Include(i => i.InvoiceDetails).Where(i => i.StatusId == 0).OrderByDescending(i => i.IssuedDate).ToList(), _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
        }

        // GET: Admin/Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.InvoiceDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Admin/Invoices/Create
        public async Task<IActionResult> Create(int id = 0)
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password");
            ViewData["ProductItemsId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            if (id == 0)
                return View(new Invoices());

            else
            {
                var invoices = await _context.Invoices.FindAsync(id);
                if (invoices == null)
                {
                    return NotFound();
                }

                return View(invoices);
            }
        }

        // POST: Admin/Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Id,Code,IssuedDate,ShippingAddress,ShippingPhone,Total,Status,Email,Fullname,StatusId,ProductItemsId,Payment")] Invoices invoices)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    _context.Add(invoices);
                    ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", invoices.AccountId);
                    ViewData["ProductItemsId"] = new SelectList(_context.Products, "Id", "Id", invoices.ProductItemsId);
                    ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", invoices.UserId);
                    await _context.SaveChangesAsync();
                }

                ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", invoices.AccountId);
                ViewData["ProductItemsId"] = new SelectList(_context.Products, "Id", "Id", invoices.ProductItemsId);
                ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", invoices.UserId);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Invoices.ToList()) });
                //return RedirectToAction(nameof(Index));
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", invoices) });
        }


        [NoDirectAccess]
        // GET: Admin/Invoices/Edit/5
        public async Task<IActionResult> Edit(int id = 0)
        {


            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password");
            ViewData["ProductItemsId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");


            ViewBag.Trangthai = new List<SelectListItem>
            {
            new SelectListItem { Text = "Đang giao hàng", Value = "1" },
            new SelectListItem { Text = "Hủy đơn", Value = "2" },
            new SelectListItem { Text = "Đã giao", Value = "4" }
            };

            if (id == 0)
                return View(new Invoices());

            else
            {
                var invoices = await _context.Invoices.FindAsync(id);
                if (invoices == null)
                {
                    return NotFound();
                }

                return View(invoices);
            }
        }

        // POST: Admin/Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,AccountId,UserId,IssuedDate,ShippingAddress,ShippingPhone,Total,Status,Email,Fullname,StatusId,ProductItemsId")] Invoices invoices,Products products)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (invoices.StatusId == 2 && products.Id != 0)
                    {
                        var getInvoiceDetails = _context.InvoiceDetails.Include(x => x.Invoice).AsNoTracking().FirstOrDefault(x => x.InvoiceId == invoices.Id);
                        if (getInvoiceDetails.InvoiceId == invoices.Id)
                        {
                            if (getInvoiceDetails.ProductId != 0)
                            {
                                var kq = _context.Products.Include(x => x.InvoiceDetails).AsNoTracking().FirstOrDefault(x => x.Id == getInvoiceDetails.ProductId);
                                if (kq.Id == getInvoiceDetails.ProductId)
                                {
                                    kq.Stock += getInvoiceDetails.Quantity;
                                    _notyf.Warning("Số lượng huỷ đã được cộng lại tồn kho", 5);
                                }
                                _context.Products.Update(kq);
                            }
                        }
                    }
                    _context.Update(invoices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoicesExists(invoices.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", invoices.AccountId);
                ViewData["ProductItemsId"] = new SelectList(_context.Products, "Id", "Id", invoices.ProductItemsId);
                ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", invoices.UserId);
                /*return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Invoices.ToList()) });*/
                return RedirectToAction(nameof(Index));
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", invoices) });
        }


        [NoDirectAccess]
        // GET: Admin/Invoices/Edit/5
        public async Task<IActionResult> EditPending(int id = 0)
        {


            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password");
            ViewData["ProductItemsId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");


            ViewBag.Trangthai = new List<SelectListItem>
            {
            new SelectListItem { Text = "Chờ duyệt", Value = "0" },
            new SelectListItem { Text = "Đang giao hàng", Value = "1" },
            new SelectListItem { Text = "Hủy đơn", Value = "2" },
            new SelectListItem { Text = "Đã giao", Value = "4" }
            };

            if (id == 0)
                return View(new Invoices());

            else
            {
                var invoices = await _context.Invoices.FindAsync(id);
                if (invoices == null)
                {
                    return NotFound();
                }

                return View(invoices);
            }
        }

        // POST: Admin/Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPending(int id, [Bind("Id,Code,AccountId,UserId,IssuedDate,ShippingAddress,ShippingPhone,Total,Status,Email,Fullname,StatusId,ProductItemsId")] Invoices invoices,Products products)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (invoices.StatusId == 2 && products.Id != 0)
                    {
                        var getInvoiceDetails = _context.InvoiceDetails.Include(x => x.Invoice).AsNoTracking().FirstOrDefault(x => x.InvoiceId == invoices.Id);
                        if (getInvoiceDetails.InvoiceId == invoices.Id)
                        {
                            if (getInvoiceDetails.ProductId != 0)
                            {
                                var kq = _context.Products.Include(x => x.InvoiceDetails).AsNoTracking().FirstOrDefault(x => x.Id == getInvoiceDetails.ProductId);
                                if (kq.Id == getInvoiceDetails.ProductId)
                                {
                                    kq.Stock += getInvoiceDetails.Quantity;
                                    _notyf.Warning("Số lượng huỷ đã được cộng lại tồn kho", 5);
                                }
                                _context.Products.Update(kq);
                            }
                        }
                    }
                    _context.Update(invoices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoicesExists(invoices.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", invoices.AccountId);
                ViewData["ProductItemsId"] = new SelectList(_context.Products, "Id", "Id", invoices.ProductItemsId);
                ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", invoices.UserId);
                /*return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Invoices.ToList()) });*/
                return RedirectToAction(nameof(Pending));
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "EditPending", invoices) });
        }
        [AllowAnonymous]
        [NoDirectAccess]
        // GET: Admin/Invoices/Edit/5
        public async Task<IActionResult> EditPendingHome(int id = 0)
        {


            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password");
            ViewData["ProductItemsId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");


            ViewBag.Trangthai = new List<SelectListItem>
            {
            new SelectListItem { Text = "Chờ duyệt", Value = "0" },
            new SelectListItem { Text = "Đang giao hàng", Value = "1" },
            new SelectListItem { Text = "Hủy đơn", Value = "2" },
            new SelectListItem { Text = "Đã giao", Value = "4" }
            };

            if (id == 0)
                return View(new Invoices());

            else
            {
                var invoices = await _context.Invoices.FindAsync(id);
                if (invoices == null)
                {
                    return NotFound();
                }

                return View(invoices);
            }
        }

        // POST: Admin/Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPendingHome(int id, [Bind("Id,Code,AccountId,UserId,IssuedDate,ShippingAddress,ShippingPhone,Total,Status,Email,Fullname,StatusId,ProductItemsId")] Invoices invoices, Products products)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (invoices.StatusId == 2 && products.Id != 0)
                    {
                        var getInvoiceDetails = _context.InvoiceDetails.Include(x => x.Invoice).AsNoTracking().FirstOrDefault(x => x.InvoiceId == invoices.Id);
                        if (getInvoiceDetails.InvoiceId == invoices.Id)
                        {
                            if (getInvoiceDetails.ProductId != 0)
                            {
                                var kq = _context.Products.Include(x => x.InvoiceDetails).AsNoTracking().FirstOrDefault(x => x.Id == getInvoiceDetails.ProductId);
                                if (kq.Id == getInvoiceDetails.ProductId)
                                {
                                    kq.Stock += getInvoiceDetails.Quantity;
                                    _notyf.Warning("Hủy đơn hàng thành công", 5);
                                }
                                _context.Products.Update(kq);
                            }
                        }
                    }
                    _context.Update(invoices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoicesExists(invoices.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", invoices.AccountId);
                ViewData["ProductItemsId"] = new SelectList(_context.Products, "Id", "Id", invoices.ProductItemsId);
                ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", invoices.UserId);
                /*return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Invoices.ToList()) });*/
                return RedirectToAction("MyOrder","Home",new { area = ""});
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "EditPending", invoices) });
        }
        // GET: Admin/Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoices = await _context.Invoices
                .Include(i => i.Account)
                .Include(i => i.ProductItems)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoices == null)
            {
                return NotFound();
            }

            return View(invoices);
        }

        // POST: Admin/Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoices = await _context.Invoices.FindAsync(id);
            _context.Invoices.Remove(invoices);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoicesExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }

        public IActionResult SendWelcomeMail(int? id)
        {
            ViewBag.Fullname = _context.Invoices.Include(x => x.User).Where(x => x.Id == id).Select(x => x.Fullname).FirstOrDefault();
            ViewBag.Email = _context.Invoices.Include(x => x.User).Where(x => x.Id == id).Select(x => x.Email).FirstOrDefault();
            ViewBag.Code = _context.Invoices.Include(x => x.User).Where(x => x.Id == id).Select(x => x.Code).FirstOrDefault();
            return View(new Invoices());

        }

        [HttpPost]
        public async Task<IActionResult> SendWelcomeMail([FromForm] Invoices invoices)
        {
            try
            {
                await mailService.SendWelcomeEmailAsync(invoices);
                _notyf.Success("Đã gửi hoá đơn qua email khách hàng thành công!", 5);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
