using AspNetCoreHero.ToastNotification.Abstractions;
using DA_TOTNGHIEP.Data;
using DA_TOTNGHIEP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DA_TOTNGHIEP.Controllers
{
    public class HomeController : Controller
    {
        private readonly DA_TOTNGHIEP2Context _context;
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyf;

        public HomeController(ILogger<HomeController> logger, DA_TOTNGHIEP2Context context, INotyfService notyf)
        {
            _logger = logger;
            _context = context;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index()
        {
            var getUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            //Kiểm tra tài khoản đã login chưa. Nếu chưa login thì giỏ hàng = null
            if (getUser == null)
            {
                var carts = new List<Carts>();
            }
            else
            {
                //Nếu có User thì hiển thị Cart theo Id của User đó
                ViewBag.CountCart = _context.Carts.Include(c => c.Product).Where(c => c.UserId == getUser.Id).Sum(c => c.Quantity);
            }

            return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>, IEnumerable<InvoiceDetails>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList(), _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).ToList()));
        }
        public async Task<IActionResult> MyOrder()
        {
            
            var getUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(_context.Invoices.Include(invd => invd.InvoiceDetails).Include(invd => invd.User)
                                                                                        .Where(c => c.UserId == getUser.Id).OrderByDescending(c => c.IssuedDate).ToList(),
                                                                                    _context.InvoiceDetails.Include(c => c.Invoice).Include(c => c.Product).ToList()));
        }
        public async Task<IActionResult> MyRank()
        {

            var getUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>(_context.Invoices.Include(invd => invd.InvoiceDetails).Include(invd => invd.User)
                                                                                        .Where(c => c.UserId == getUser.Id).OrderByDescending(c => c.IssuedDate).ToList(),
                                                                                    _context.InvoiceDetails.Include(c => c.Invoice).Include(c => c.Product).Where(c => c.Invoice.UserId == getUser.Id).ToList()));
            /*return View(Tuple.Create<IEnumerable<Invoices>, IEnumerable<InvoiceDetails>>
                (_context.Invoices.Include(i => i.Account).Include(i => i.ProductItems).Include(i => i.User).Include(i => i.InvoiceDetails).ToList(),
                _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).Include(i => i.Product.ImageProductss).ToList()));*/
        }
        public async Task<IActionResult> SearchOrder(string searchOrder = "")
        {
            var getUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            //Kiểm tra tài khoản đã login chưa. Nếu chưa login thì giỏ hàng = null
            if (getUser == null)
            {
                var carts = new List<Carts>();
            }
            else
            {
                //Nếu có User thì hiển thị Cart theo Id của User đó
                ViewBag.CountCart = _context.Carts.Include(c => c.Product).Where(c => c.UserId == getUser.Id).Sum(c => c.Quantity);
            }

            //SearchOrder
            List<InvoiceDetails> productOrder;
            if (searchOrder != null && searchOrder != null)
            {
                ViewBag.KeyOrder = searchOrder;
                productOrder = _context.InvoiceDetails.Include(invd => invd.Invoice).Include(invd => invd.Product).Where(invd => invd.Invoice.Code.Contains(searchOrder)).ToList();
                return View(productOrder);
            }
            return View();
        }
        public async Task<IActionResult> Products(string search = "")
        {
            if (search != "" && search != null)
            {
                ViewBag.KeySearch = search;
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.Name.Contains(search) || p.ProductType.Name.Contains(search)).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList())/*await product.ToListAsync()*/);
        }
        public async Task<IActionResult> AllProduct(string price = "", string brand = "", string sortOrderAscending = "", string sortOrderDescending = "")
        {
            var products = Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList());

            //Lọc theo hãng
            if (brand == "Apple" && brand != null)
            {
                products = Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Apple").ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList());
            }
            if (brand == "SamSung" && brand != null)
            {
                products = Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "SamSung").ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList());
            }
            if (brand == "Xiaomi" && brand != null)
            {
                products = Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Xiaomi").ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList());
            }
            if (brand == "Oppo" && brand != null)
            {
                products = Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Oppo").ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList());
            }
            if (brand == "Nokia" && brand != null)
            {
                products = Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Nokia").ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList());
            }
            //Lọc theo giá
            if (price == "Dưới 5 triệu" && price != null)
            {
                products = Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.Price <= 5000000 && p.Name.Contains(brand) || p.Price <= 5000000 && p.ProductType.Name.Contains(brand)).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList());
            }
            if (price == "5 - 10 triệu" && price != null)
            {
                products = Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.Price >= 5000000 && p.Price <= 10000000 && p.Name.Contains(brand) || p.Price >= 5000000 && p.Price <= 10000000 && p.ProductType.Name.Contains(brand)).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList());
            }
            if (price == "10 - 15 triệu" && price != null)
            {
                products = Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.Price >= 10000000 && p.Price <= 15000000 && p.Name.Contains(brand) || p.Price >= 10000000 && p.Price <= 15000000 && p.ProductType.Name.Contains(brand)).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList());
            }
            if (price == "15 - 20 triệu" && price != null)
            {
                products = Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.Price >= 15000000 && p.Price <= 20000000 && p.Name.Contains(brand) || p.Price >= 15000000 && p.Price <= 20000000 && p.ProductType.Name.Contains(brand)).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList());
            }
            if (price == "Trên 20 triệu" && price != null)
            {
                products = Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.Price >= 20000000 && p.Name.Contains(brand) || p.Price >= 20000000 && p.ProductType.Name.Contains(brand)).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList());
            }
            //Sắp xếp theo thứ tự tăng - giảm
            ViewData["PriceAscending"] = string.IsNullOrEmpty(sortOrderAscending) ? "ascending" : "";
            switch (sortOrderAscending)
            {
                case "ascending":
                    products = Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                        .OrderBy(p => p.Price).ToList(),
                                                                        _context.Comment.Include(c => c.Product).ToList());
                    break;
                    /*default:
                        return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                            .OrderBy(p => p.Price).ToList(),
                                                                            _context.Comment.Include(c => c.Product).ToList()));
                        break;*/
            }
            ViewData["PriceDescending"] = string.IsNullOrEmpty(sortOrderDescending) ? "descending" : "";
            switch (sortOrderDescending)
            {
                case "descending":
                    products = Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                        .OrderByDescending(p => p.Price).ToList(),
                                                                        _context.Comment.Include(c => c.Product).ToList());
                    break;
                    /*default:
                        return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                            .OrderByDescending(p => p.Price).ToList(),
                                                                            _context.Comment.Include(c => c.Product).ToList()));
                        break;*/
            }
            return View(products);
        }
        public async Task<IActionResult> ProductTrending()
        {
            var trending = from c in _context.Comment join p in _context.Products on c.ProductId equals p.Id select new { comment = c, product = p };
            var products = trending.Select(p => p.product);
            return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .ToList(),
                                                                                    _context.Comment.Include(c => c.Product)
                                                                                    .Where(c => c.Product.Id == c.ProductId).OrderBy(c => c.Rating)
                                                                                    .ToList()));
        }
        public async Task<IActionResult> Apple(string price = "", string sortOrderAscending = "", string sortOrderDescending = "")
        {
            //Lọc theo giá
            if (price == "Dưới 5 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Apple" && p.Price <= 5000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "5 - 10 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Apple" && p.Price >= 5000000 && p.Price <= 10000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "10 - 15 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Apple" && p.Price >= 10000000 && p.Price <= 15000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "15 - 20 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Apple" && p.Price >= 15000000 && p.Price <= 20000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "Trên 20 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Apple" && p.Price >= 20000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            //Sắp xếp theo thứ tự tăng dần
            ViewData["PriceAscending"] = string.IsNullOrEmpty(sortOrderAscending) ? "ascending" : "";
            switch (sortOrderAscending)
            {
                case "ascending":
                    return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                        .Where(p => p.ProductType.Name == "Apple").OrderBy(p => p.Price).ToList(),
                                                                        _context.Comment.Include(c => c.Product).ToList()));
                    break;
                    /*default:
                        return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                            .OrderBy(p => p.Price).ToList(),
                                                                            _context.Comment.Include(c => c.Product).ToList()));
                        break;*/
            }
            //Sắp xếp theo thứ tự giảm dần
            ViewData["PriceDescending"] = string.IsNullOrEmpty(sortOrderDescending) ? "descending" : "";
            switch (sortOrderDescending)
            {
                case "descending":
                    return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                        .Where(p => p.ProductType.Name == "Apple").OrderByDescending(p => p.Price).ToList(),
                                                                        _context.Comment.Include(c => c.Product).ToList()));
                    break;
                    /*default:
                        return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                            .OrderByDescending(p => p.Price).ToList(),
                                                                            _context.Comment.Include(c => c.Product).ToList()));
                        break;*/
            }
            return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Apple").ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
        }
        public async Task<IActionResult> SamSung(string price = "", string sortOrderAscending = "", string sortOrderDescending = "")
        {
            //Lọc theo giá
            if (price == "Dưới 5 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "SamSung" && p.Price <= 5000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "5 - 10 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "SamSung" && p.Price >= 5000000 && p.Price <= 10000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "10 - 15 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "SamSung" && p.Price >= 10000000 && p.Price <= 15000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "15 - 20 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "SamSung" && p.Price >= 15000000 && p.Price <= 20000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "Trên 20 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "SamSung" && p.Price >= 20000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            //Sắp xếp theo thứ tự tăng dần
            ViewData["PriceAscending"] = string.IsNullOrEmpty(sortOrderAscending) ? "ascending" : "";
            switch (sortOrderAscending)
            {
                case "ascending":
                    return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                        .Where(p => p.ProductType.Name == "SamSung").OrderBy(p => p.Price).ToList(),
                                                                        _context.Comment.Include(c => c.Product).ToList()));
                    break;
                    /*default:
                        return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                            .OrderBy(p => p.Price).ToList(),
                                                                            _context.Comment.Include(c => c.Product).ToList()));
                        break;*/
            }
            //Sắp xếp theo thứ tự giảm dần
            ViewData["PriceDescending"] = string.IsNullOrEmpty(sortOrderDescending) ? "descending" : "";
            switch (sortOrderDescending)
            {
                case "descending":
                    return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                        .Where(p => p.ProductType.Name == "SamSung").OrderByDescending(p => p.Price).ToList(),
                                                                        _context.Comment.Include(c => c.Product).ToList()));
                    break;
                    /*default:
                        return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                            .OrderByDescending(p => p.Price).ToList(),
                                                                            _context.Comment.Include(c => c.Product).ToList()));
                        break;*/
            }
            return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "SamSung").ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
        }
        public async Task<IActionResult> Xiaomi(string price = "", string sortOrderAscending = "", string sortOrderDescending = "")
        {
            //Lọc theo giá
            if (price == "Dưới 5 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Xiaomi" && p.Price <= 5000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "5 - 10 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Xiaomi" && p.Price >= 5000000 && p.Price <= 10000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "10 - 15 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Xiaomi" && p.Price >= 10000000 && p.Price <= 15000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "15 - 20 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Xiaomi" && p.Price >= 15000000 && p.Price <= 20000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "Trên 20 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Xiaomi" && p.Price >= 20000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            //Sắp xếp theo thứ tự tăng dần
            ViewData["PriceAscending"] = string.IsNullOrEmpty(sortOrderAscending) ? "ascending" : "";
            switch (sortOrderAscending)
            {
                case "ascending":
                    return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                        .Where(p => p.ProductType.Name == "Xiaomi").OrderBy(p => p.Price).ToList(),
                                                                        _context.Comment.Include(c => c.Product).ToList()));
                    break;
                    /*default:
                        return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                            .OrderBy(p => p.Price).ToList(),
                                                                            _context.Comment.Include(c => c.Product).ToList()));
                        break;*/
            }
            //Sắp xếp theo thứ tự giảm dần
            ViewData["PriceDescending"] = string.IsNullOrEmpty(sortOrderDescending) ? "descending" : "";
            switch (sortOrderDescending)
            {
                case "descending":
                    return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                        .Where(p => p.ProductType.Name == "Xiaomi").OrderByDescending(p => p.Price).ToList(),
                                                                        _context.Comment.Include(c => c.Product).ToList()));
                    break;
                    /*default:
                        return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                            .OrderByDescending(p => p.Price).ToList(),
                                                                            _context.Comment.Include(c => c.Product).ToList()));
                        break;*/
            }
            return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Xiaomi").ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
        }
        public async Task<IActionResult> Oppo(string price = "", string sortOrderAscending = "", string sortOrderDescending = "")
        {
            //Lọc theo giá
            if (price == "Dưới 5 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Oppo" && p.Price <= 5000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "5 - 10 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Oppo" && p.Price >= 5000000 && p.Price <= 10000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "10 - 15 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Oppo" && p.Price >= 10000000 && p.Price <= 15000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "15 - 20 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Oppo" && p.Price >= 15000000 && p.Price <= 20000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "Trên 20 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Oppo" && p.Price >= 20000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            //Sắp xếp theo thứ tự tăng dần
            ViewData["PriceAscending"] = string.IsNullOrEmpty(sortOrderAscending) ? "ascending" : "";
            switch (sortOrderAscending)
            {
                case "ascending":
                    return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                        .Where(p => p.ProductType.Name == "Oppo").OrderBy(p => p.Price).ToList(),
                                                                        _context.Comment.Include(c => c.Product).ToList()));
                    break;
                    /*default:
                        return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                            .OrderBy(p => p.Price).ToList(),
                                                                            _context.Comment.Include(c => c.Product).ToList()));
                        break;*/
            }
            //Sắp xếp theo thứ tự giảm dần
            ViewData["PriceDescending"] = string.IsNullOrEmpty(sortOrderDescending) ? "descending" : "";
            switch (sortOrderDescending)
            {
                case "descending":
                    return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                        .Where(p => p.ProductType.Name == "Oppo").OrderByDescending(p => p.Price).ToList(),
                                                                        _context.Comment.Include(c => c.Product).ToList()));
                    break;
                    /*default:
                        return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                            .OrderByDescending(p => p.Price).ToList(),
                                                                            _context.Comment.Include(c => c.Product).ToList()));
                        break;*/
            }
            return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Oppo").ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
        }
        public async Task<IActionResult> Nokia(string price = "", string sortOrderAscending = "", string sortOrderDescending = "")
        {
            //Lọc theo giá
            if (price == "Dưới 5 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Nokia" && p.Price <= 5000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "5 - 10 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Nokia" && p.Price >= 5000000 && p.Price <= 10000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "10 - 15 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Nokia" && p.Price >= 10000000 && p.Price <= 15000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "15 - 20 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Nokia" && p.Price >= 15000000 && p.Price <= 20000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            if (price == "Trên 20 triệu" && price != null)
            {
                return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Nokia" && p.Price >= 20000000).ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
            }
            //Sắp xếp theo thứ tự tăng dần
            ViewData["PriceAscending"] = string.IsNullOrEmpty(sortOrderAscending) ? "ascending" : "";
            switch (sortOrderAscending)
            {
                case "ascending":
                    return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                        .Where(p => p.ProductType.Name == "Nokia").OrderBy(p => p.Price).ToList(),
                                                                        _context.Comment.Include(c => c.Product).ToList()));
                    break;
                    /*default:
                        return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                            .OrderBy(p => p.Price).ToList(),
                                                                            _context.Comment.Include(c => c.Product).ToList()));
                        break;*/
            }
            //Sắp xếp theo thứ tự giảm dần
            ViewData["PriceDescending"] = string.IsNullOrEmpty(sortOrderDescending) ? "descending" : "";
            switch (sortOrderDescending)
            {
                case "descending":
                    return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                        .Where(p => p.ProductType.Name == "Nokia").OrderByDescending(p => p.Price).ToList(),
                                                                        _context.Comment.Include(c => c.Product).ToList()));
                    break;
                    /*default:
                        return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                            .OrderByDescending(p => p.Price).ToList(),
                                                                            _context.Comment.Include(c => c.Product).ToList()));
                        break;*/
            }
            return View(Tuple.Create<IEnumerable<Products>, IEnumerable<Comment>>(_context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                                                                                    .Where(p => p.ProductType.Name == "Nokia").ToList(),
                                                                                    _context.Comment.Include(c => c.Product).ToList()));
        }

        public async Task<IActionResult> Details(int? id)
        {
            /*if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products
                .Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }*/

            //Số lượng giỏ hàng
            var getUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            //Kiểm tra tài khoản đã login chưa. Nếu chưa login thì giỏ hàng = null
            if (getUser == null)
            {
                var carts = new List<Carts>();
            }
            else
            {
                //Nếu có User thì hiển thị Cart theo Id của User đó
                ViewBag.CountCart = _context.Carts.Include(c => c.Product).Where(c => c.UserId == getUser.Id).Sum(c => c.Quantity);
            }

            //Details
            if (id == null)
            {
                return BadRequest();
            }
            Products product = await _context.Products.Include(p => p.ImageProductss).Include(p => p.ProductType).Include(p => p.Warehouse).FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return BadRequest();
            }
            ViewBag.ProductId = id.Value;

            var comments = _context.Comment.Where(d => d.ProductId.Equals(id.Value)).ToList();
            ViewBag.Comments = comments;

            var ratings = _context.Comment.Where(d => d.ProductId.Equals(id.Value)).ToList();
            if (ratings.Count() > 0)
            {
                var ratingSum = ratings.Sum(d => d.Rating.Value);
                ViewBag.RatingSum = ratingSum;
                var ratingCount = ratings.Count();
                ViewBag.RatingCount = ratingCount;
            }
            else
            {
                ViewBag.RatingSum = 0;
                ViewBag.RatingCount = 0;
            }
            return View(product);
        }


        [HttpGet]
        public IActionResult ChartInvoices()
        {
            var unApproved = _context.Invoices
                .Where(i => i.StatusId == 0)
                .GroupBy(j => j.IssuedDate)
                .Select(group => new
                {
                    IssuedDate = group.Key,
                    Count = group.Count()
                });

            var countUnApproved = unApproved
                 .Select(a => a.Count).ToArray();


            var Approved = _context.Invoices
                .Where(i => i.StatusId == 4)
                .GroupBy(j => j.IssuedDate)
                .Select(group => new
                {
                    IssuedDate = group.Key,
                    Count = group.Count()
                });

            var countApproved = Approved
                .Select(a => a.Count).ToArray();

            /*float ok = (float) (countinvCanceled.Count() / (float)(countApproved.Count() + (float)countinvCanceled.Count()) * 100);
            float ko = (float) (countApproved.Count() / (float)(countApproved.Count() + (float)countinvCanceled.Count()) * 100);*/
            if(countUnApproved == new int[0])
            {
                return new JsonResult(new { jsonUnApproved = new int[0], jsonApproved = countApproved.Count() });
            }
           
            return new JsonResult(new { jsonUnApproved = countUnApproved.Count(), jsonApproved = countApproved.Count() });
        }

        [HttpGet]
        public IActionResult ChartTotal(DateTime fromDate, DateTime toDate)
        {
            /*if(fromDate.ToString() != "") { 
            var totalFormDate = _context.Invoices.Where(x => x.IssuedDate.Date == fromDate && x.StatusId == 4)
               .Select(x => x.Total).Sum();
                var ok = fromDate.ToString();
                ViewBag.FromDate = ok.ToString();
                var countTotalFromDate = totalFormDate.ToString();

                return new JsonResult(new
                {
                    jsonDateTime = ok,
                    jsoncountToTalFromDate = countTotalFromDate,
                });
            }*/
            var totalDate = _context.Invoices.Where(x => x.IssuedDate.Date == fromDate && x.StatusId == 4)
               .Select(x => x.Total).Sum();
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

            var dateTimeDate = fromDate.ToShortDateString().ToString();
            var dateTime6 = DateTime.Today.AddDays(-6).ToShortDateString().ToString();
            var dateTime5 = DateTime.Today.AddDays(-5).ToShortDateString().ToString();
            var dateTime4 = DateTime.Today.AddDays(-4).ToShortDateString().ToString();
            var dateTime3 = DateTime.Today.AddDays(-3).ToShortDateString().ToString();
            var dateTime2 = DateTime.Today.AddDays(-2).ToShortDateString().ToString();
            var dateTime1 = DateTime.Today.AddDays(-1).ToShortDateString().ToString();
            var dateTimeToday = DateTime.Today.ToShortDateString().ToString();

            var countTotalDate = totalDate.ToString();
            var countTotal6 = total6.ToString();
            var countTotal5 = total5.ToString();
            var countTotal4 = total4.ToString();
            var countTotal3 = total3.ToString();
            var countTotal2 = total2.ToString();
            var countTotal1 = total1.ToString();
            var countTotalToday = totalToday.ToString();

            var jsonResult = new JsonResult(new {jsoncountTotal6 = countTotal6,jsoncountTotal5 = countTotal5,jsoncountTotal4 = countTotal4, jsoncountTotal3 = countTotal3, jsoncountTotal2 = countTotal2, jsoncountTotal1 = countTotal1, jsoncountTotalToday = countTotalToday,
                                        jsonDateTime6 = dateTime6,jsonDateTime5 = dateTime5,jsonDateTime4 = dateTime4,jsonDateTime3 = dateTime3 ,jsonDateTime2 = dateTime2,jsonDateTime1 = dateTime1,jsonDateTimeToday = dateTimeToday,
                                       });
            return new JsonResult(new {jsoncountTotalDate = countTotalDate,jsoncountTotal6 = countTotal6,jsoncountTotal5 = countTotal5,jsoncountTotal4 = countTotal4, jsoncountTotal3 = countTotal3, jsoncountTotal2 = countTotal2, jsoncountTotal1 = countTotal1, jsoncountTotalToday = countTotalToday,
                                        jsonDateTimeDate = dateTimeDate,jsonDateTime6 = dateTime6,jsonDateTime5 = dateTime5,jsonDateTime4 = dateTime4,jsonDateTime3 = dateTime3 ,jsonDateTime2 = dateTime2,jsonDateTime1 = dateTime1,jsonDateTimeToday = dateTimeToday,
                                       });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
