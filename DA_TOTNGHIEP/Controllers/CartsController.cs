using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DA_TOTNGHIEP.Models;
using DA_TOTNGHIEP.Data;
using DA_TOTNGHIEP.PaymentLibrary;
using Newtonsoft.Json.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace DA_TOTNGHIEP.Controllers
{
    public class CartsController : Controller
    {
        private readonly DA_TOTNGHIEP2Context _context;
        private readonly INotyfService _notyf;

        public CartsController(DA_TOTNGHIEP2Context context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        // GET: Carts
        public IActionResult Index()
        {
            if (!CheckStock(User.Identity.Name))
            {
                //ViewBag.ErrorMessage = "Sản phẩm hết hàng. Vui lòng kiểm tra hoặc liên hệ nhân viên!";
                ViewBag.Account = _context.Users.Where(a => a.UserName == User.Identity.Name).FirstOrDefault();
                ViewBag.CartsTotal = _context.Carts.Sum(c => c.Quantity * c.Product.Price);
            }

            //var data = _context.Carts.Include(c => c.Product).Include(c => c.User);
            var getUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            //ViewBag tính toán
            


            //Kiểm tra tài khoản đã login chưa. Nếu chưa login thì giỏ hàng = null
            if (getUser == null)
            {
                var carts = new List<Carts>();
                return View(carts);
            }
            else
            {
                //Nếu có User thì hiển thị Cart theo Id của User đó
                ViewBag.CountCart = _context.Carts.Include(c => c.Product).Where(c => c.UserId == getUser.Id).Sum(c => c.Quantity);
                var getCartByUserId = _context.Carts.Include(c => c.Product.ImageProductss).Where(c => c.UserId == getUser.Id);
                ViewBag.getTotalByUserId = _context.Carts.Where(c => c.UserId == getUser.Id).Sum(c => c.Quantity * c.Product.Price);
                return View(getCartByUserId);
            }
            //return View(await data.ToListAsync());
        }

        //GET
        public IActionResult Add(int id)
        {
            return Add(id, 1);
        }

        //POST
        [HttpPost]
        public IActionResult Add(int id, int quantity = 1)
        {

            //Bắt buộc đăng nhập để thực hiện Action
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect($"/Identity/Account/Login?Returnurl=/Carts/Add/{id}");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            //Kiểm tra trong giỏ hàng có hay chưa
            Carts cart = _context.Carts.FirstOrDefault(c => c.UserId == user.Id && c.ProductId == id);

            if (cart == null)
            {
                cart = new Carts
                {
                    UserId = user.Id,
                    ProductId = id,
                    Quantity = quantity
                };
                _context.Carts.Add(cart);
            }
            else
            {
                cart.Quantity += quantity;
                _context.Carts.Update(cart);
            }
            _context.SaveChanges();
            _notyf.Success("Sản phẩm đã thêm vào giỏ hàng", 3);
            return RedirectToAction("Details", "Home", new { id = id});
        }

        //GET
        public IActionResult AddNow(int id)
        {
            return AddNow(id, 1);
        }
        [HttpPost]
        public IActionResult AddNow(int id, int quantity = 1)
        {

            //Bắt buộc đăng nhập để thực hiện Action
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect($"/Identity/Account/Login?Returnurl=/Carts/Add/{id}");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            //Kiểm tra trong giỏ hàng có hay chưa
            Carts cart = _context.Carts.FirstOrDefault(c => c.UserId == user.Id && c.ProductId == id);

            if (cart == null)
            {
                cart = new Carts
                {
                    UserId = user.Id,
                    ProductId = id,
                    Quantity = quantity
                };
                _context.Carts.Add(cart);
            }
            else
            {
                cart.Quantity += quantity;
                _context.Carts.Update(cart);
            }
            _context.SaveChanges();
            _notyf.Success("Sản phẩm đã thêm vào giỏ hàng", 3);
            return RedirectToAction("Index", "Carts");
        }

        //GET
        public IActionResult UpdateCart(int id)
        {
            return UpdateCart(id, 1);
        }

        //POST
        [HttpPost]
        public IActionResult UpdateCart(int id, int quantity = 1)
        {
            /*if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect($"/Identity/Account/Login?Returnurl=/Carts/Add/{id}");
            }*/
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            Carts cart = _context.Carts.FirstOrDefault(c => c.UserId == user.Id && c.ProductId == id);

            if (cart == null)
            {
                cart = new Carts
                {
                    UserId = user.Id,
                    ProductId = id,
                    Quantity = quantity
                };
                _context.Carts.Add(cart);
            }
            else
            {
                cart.Quantity += quantity;
                _context.Carts.Update(cart);
            }
            _context.SaveChanges();
            _notyf.Success("Sản phẩm đã tăng thêm", 3);
            return RedirectToAction("Index", "Carts");
        }

        //GET
        public IActionResult Remove(int id)
        {
            return Remove(id, 1);
        }

        //POST
        [HttpPost]
        public IActionResult Remove(int id, int quantity)
        {
            /*if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect($"/Identity/Account/Login?Returnurl=/Carts/Remove/{id}");
            }*/
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            Carts cart = _context.Carts.FirstOrDefault(c => c.UserId == user.Id && c.ProductId == id);

            if (cart == null)
            {
                cart = new Carts
                {
                    UserId = user.Id,
                    ProductId = id,
                    Quantity = quantity
                };
                _context.Carts.Remove(cart);
            }
            else
            {
                cart.Quantity -= quantity;
                _context.Carts.Update(cart);
            }
            _context.SaveChanges();
            _notyf.Warning("Đã giảm số lượng", 3);
            return RedirectToAction("Index", "Carts");
        }

        //GET
        public IActionResult Pay()
        {
            var getUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.getTotalByUserId = _context.Carts.Where(c => c.UserId == getUser.Id).Sum(c => c.Quantity * c.Product.Price);
            ViewBag.EmailUser = _context.Carts.Where(c => c.UserId == getUser.Id).Select(c => getUser.Email).FirstOrDefault().ToString();
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            //ViewBag.Account = _context.Users.Where(a => a.UserName == user.Id).FirstOrDefault();
            ViewBag.CartTotal = _context.Carts.Sum(c => c.Quantity * c.Product.Price);
            var doAnContext = _context.Carts.Include(c => c.User).Include(c => c.Product.ImageProductss);

            //Kiểm tra tài khoản đã login chưa. Nếu chưa login thì giỏ hàng = null
            if (getUser == null)
            {
                var carts = new List<Carts>();
                return View(carts);
            }
            else
            {
                //Nếu có User thì hiển thị Cart theo Id của User đó
                ViewBag.CountCart = _context.Carts.Include(c => c.Product).Where(c => c.UserId == getUser.Id).Sum(c => c.Quantity);
                /*var getCartByUserId = _context.Carts.Include(c => c.Product.ImageProductss).Where(c => c.UserId == getUser.Id);
                ViewBag.getTotalByUserId = _context.Carts.Where(c => c.UserId == getUser.Id).Sum(c => c.Quantity * c.Product.Price);*/
                return View(Tuple.Create<Invoices, IEnumerable<Carts>>(new Invoices(), _context.Carts.Include(c => c.Product.ImageProductss).Where(c => c.UserId == getUser.Id).ToList()));
            }

            return View(Tuple.Create<Invoices, IEnumerable<Carts>>(new Invoices(), _context.Carts.Include(c => c.Product.ImageProductss).Where(c => c.UserId == getUser.Id).ToList()));
        }

        //POST
        [HttpPost]
        public IActionResult Pay([Bind(Prefix = "Item1")] Invoices invoice)
        {
            var username = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var getUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (!CheckStock(User.Identity.Name))
            {
                /*var getUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();*/
                var getCartByUserId = _context.Carts.Include(c => c.Product.ImageProductss).Where(c => c.UserId == getUser.Id);
                ViewBag.ErrorMessage = "Sản phẩm hết hàng. Vui lòng kiểm tra hoặc liên hệ nhân viên!";
                ViewBag.Account = _context.Users.Where(a => a.UserName == User.Identity.Name).FirstOrDefault();
                ViewBag.CartsTotal = _context.Carts.Where(u => u.UserId == getUser.Id).Sum(c => c.Quantity * c.Product.Price);
                return RedirectToAction("Index", "Carts");
            }

            //thêm hóa đơn

            DateTime now = DateTime.Now;
            invoice.Code = "CT" + now.ToString("yyMMddhhmmss");
            invoice.UserId = _context.Users.FirstOrDefault(a => a.UserName == User.Identity.Name).Id;
            invoice.IssuedDate = now;
            invoice.Total = _context.Carts.Where(c => c.UserId == getUser.Id).Sum(c => c.Quantity * c.Product.Price);
            invoice.StatusId = 0;

            _context.Add(invoice);
            _context.SaveChanges();

            // thêm chi tiết hóa đơn
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            List<Carts> carts = _context.Carts.Include(c => c.Product).Include(c => c.User)
                .Where(c => c.UserId == user.Id).ToList();

            foreach (Carts c in carts)
            {
                InvoiceDetails invoiceDetail = new InvoiceDetails();
                invoiceDetail.InvoiceId = invoice.Id;
                invoiceDetail.ProductId = c.ProductId;
                invoiceDetail.Quantity = c.Quantity;
                invoiceDetail.UnitPrice = c.Product.Price;
                invoiceDetail.Payment = "Thanh toán khi nhận hàng";
                _context.Add(invoiceDetail);
            }

            _context.SaveChanges();
            //trừ sl tồn kho và xóa giỏ hàng
            foreach (Carts c in carts)
            {
                c.Product.Stock -= c.Quantity;
                _context.Carts.Remove(c);
            }
            _context.SaveChanges();

            return RedirectToAction("Ordersuccess", "Carts");
        }


        //GET
        /*public IActionResult PayMomo()
        {
            var getUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.getTotalByUserId = _context.Carts.Where(c => c.UserId == getUser.Id).Sum(c => c.Quantity * c.Product.Price);
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            //ViewBag.Account = _context.Users.Where(a => a.UserName == user.Id).FirstOrDefault();
            ViewBag.CartTotal = _context.Carts.Sum(c => c.Quantity * c.Product.Price);
            var doAnContext = _context.Carts.Include(c => c.User).Include(c => c.Product);
            return View(Tuple.Create<Invoices, IEnumerable<Carts>>(new Invoices(), _context.Carts.Include(c => c.User).Include(c => c.Product).ToList()));
        }*/

        //POST
        [HttpPost]
        public IActionResult PayMomo([Bind(Prefix = "Item1")] Invoices invoice, Products product)
        {
            var username = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var getUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (!CheckStock(User.Identity.Name))
            {
                /*var getUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();*/
                var getCartByUserId = _context.Carts.Include(c => c.Product.ImageProductss).Where(c => c.UserId == getUser.Id);
                ViewBag.ErrorMessage = "Sản phẩm hết hàng. Vui lòng kiểm tra hoặc liên hệ nhân viên!";
                ViewBag.Account = _context.Users.Where(a => a.UserName == User.Identity.Name).FirstOrDefault();
                ViewBag.CartsTotal = _context.Carts.Where(u => u.UserId == getUser.Id).Sum(c => c.Quantity * c.Product.Price);
                return RedirectToAction("Index", "Carts");
            }

            //thêm hóa đơn
            DateTime now = DateTime.Now;
            invoice.Code = "CT" + now.ToString("yyMMddhhmmss");
            invoice.UserId = _context.Users.FirstOrDefault(a => a.UserName == User.Identity.Name).Id;
            invoice.IssuedDate = now;
            invoice.Total = _context.Carts.Where(u => u.UserId == getUser.Id).Sum(c => c.Quantity * c.Product.Price);
            invoice.StatusId = 0;

            _context.Add(invoice);
            _context.SaveChanges();


            // thêm chi tiết hóa đơn
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            List<Carts> carts = _context.Carts.Include(c => c.Product).Include(c => c.User)
                .Where(c => c.UserId == user.Id).ToList();

            foreach (Carts c in carts)
            {
                InvoiceDetails invoiceDetail = new InvoiceDetails();
                invoiceDetail.InvoiceId = invoice.Id;
                invoiceDetail.ProductId = c.ProductId;
                invoiceDetail.Quantity = c.Quantity;
                invoiceDetail.UnitPrice = c.Product.Price;
                invoiceDetail.Payment = "Giao dịch qua Momo";
                invoiceDetail.MomoConfirm = "Giao dịch thành công";
                _context.Add(invoiceDetail);
            }
            _context.SaveChanges();

            return RedirectToRoute(new { action = "Payment", controller = "Carts" });

        }

        public IActionResult ConfirmPaymentClient()
        {
            string errorCode = (Request.Query["errorCode"]);
            string orderId = (Request.Query["orderId"]);
            if (errorCode == "49")
            {
                List<InvoiceDetails> invoiceDetails = _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product)
                    .Where(i => i.Invoice.Code == orderId)
                    .ToList();
                foreach(InvoiceDetails inv in invoiceDetails)
                {
                    inv.MomoConfirm = "Giao dịch Momo bị huỷ";
                    inv.Invoice.StatusId = 3;
                    inv.UnitPrice = 0;
                }
                _notyf.Warning("Giao dịch qua Momo đã bị huỷ. Quý khách hãy thử thanh toán lại", 5);
                _context.SaveChanges();
            }
            if(errorCode == "0")
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                List<Carts> carts = _context.Carts.Include(c => c.Product).Include(c => c.User)
                    .Where(c => c.UserId == user.Id).ToList();
                //trừ sl tồn kho và xóa giỏ hàng
                foreach (Carts c in carts)
                {
                    c.Product.Stock -= c.Quantity;
                    _context.Carts.Remove(c);
                }
                _context.SaveChanges();
                return RedirectToAction("Ordersuccess", "Carts");
            }
            return RedirectToAction("Index", "Carts");
        }

        public IActionResult Ordersuccess()
        {
            //Bắt buộc đăng nhập để thực hiện Action
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect($"/Identity/Account/Login?Returnurl=/Home/Index");
            }

            var getUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            //Kiểm tra tài khoản đã login chưa. Nếu chưa login thì invoice = null
            if (getUser != null)
            {
                var lstInvoices = _context.Invoices.OrderByDescending(inv => inv.Id).Where(c => c.UserId == getUser.Id).Take(1);
                return View(lstInvoices);
            }
            return View();
        }

        private bool CheckStock(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            List<Carts> carts = _context.Carts.Include(c => c.Product).Include(c => c.User)
                .Where(c => c.User.UserName == username).ToList();
            foreach (Carts c in carts)
            {
                if (c.Product.Stock < c.Quantity)
                {
                    return false;
                }
            }
            return true;
        }


        // GET: Carts/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.User)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }*/

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.User)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            //return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll") });
            _notyf.Error("Bạn đã xoá sản phẩm khỏi giỏ hàng", 3);
            return RedirectToAction(nameof(Index));
        }

        private bool CartsExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }

        //Cổng thanh toán Momo
        public ActionResult Payment()
        {
            var getUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "YOUR CODE";
            string accessKey = "YOUR ACCESS KEY
            string serectkey = "YOUR SERECT KEY";
            string orderInfo = "Giao dịch thanh toán MoMo C&T Shop";
            string returnUrl = "https://localhost:5001/Carts/ConfirmPaymentClient";
            string notifyurl = "https://9322-116-102-185-51.ap.ngrok.io/Carts/OrderSuccess"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            ViewBag.Amount = _context.Carts.Where(c => c.UserId == getUser.Id).Sum(c => c.Quantity * c.Product.Price);
            string amount = ViewBag.Amount.ToString();
            DateTime now = DateTime.Now;
            ViewBag.OrderId = "CT" + now.ToString("yyMMddhhmmss");
            string orderid = ViewBag.OrderId.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());

        }

        //Khi thanh toán xong ở cổng thanh toán Momo, Momo sẽ trả về một số thông tin, trong đó có errorCode để check thông tin thanh toán
        //errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
        //Tham khảo bảng mã lỗi tại: https://developers.momo.vn/#/docs/aio/?id=b%e1%ba%a3ng-m%c3%a3-l%e1%bb%97i

    }
}
