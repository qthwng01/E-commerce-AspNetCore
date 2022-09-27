using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DA_TOTNGHIEP.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using DA_TOTNGHIEP.Data;
using Microsoft.AspNetCore.Authorization;

namespace DA_TOTNGHIEP.Controllers
{
    public class CommentsController : Controller
    {
        private readonly DA_TOTNGHIEP2Context _context;
        private readonly INotyfService _notyf;

        public CommentsController(DA_TOTNGHIEP2Context context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        // GET: Admin/Comments
        public async Task<IActionResult> Index()
        {
            var dA_TOTNGHIEPContext = _context.Comment.Include(c => c.Account).Include(c => c.Product);
            return View(await dA_TOTNGHIEPContext.ToListAsync());
        }

        // GET: Admin/Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Admin/Comments/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            return View();
        }

        // POST: Admin/Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,ContentComment,Rating,ProductId,AccountId,CreatedAt,ModifyAt")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                int? productId = comment.ProductId;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                _notyf.Success("Bình luận thành công !!!", 5);
                return RedirectToAction("Details", "Home", new { id = productId });
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", comment.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", comment.ProductId);

            return View(comment);
        }

        // GET: Admin/Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", comment.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", comment.ProductId);
            return View(comment);
        }

        // POST: Admin/Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,ContentComment,Rating,ProductId,AccountId,CreatedAt,ModifyAt")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", comment.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", comment.ProductId);
            return View(comment);
        }
        [Authorize(Roles = "Admin,Sale")]
        // GET: Admin/Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }
        [Authorize(Roles = "Admin,Sale")]
        // POST: Admin/Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            _notyf.Success("Xóa bình luận thành công !!!", 5);
            return RedirectToAction("Details", "Home", new { id = comment.ProductId});
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }
    }
}
