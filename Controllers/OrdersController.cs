using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Henk5.Data;
using Henk5.Models;
using Microsoft.AspNetCore.Authorization;

namespace Henk5.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Orders.Include(o => o.Owner).Include(o => o.User);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> Index2()
        {
            var applicationDbContext = _context.categories.Include(O => O.Items);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewOrder(int OrderId)
        {
            var applicationDbContext = _context.Orders.Include(O => O.Items).Where(O => O.Id == OrderId);
            return View(await applicationDbContext.ToListAsync());
        }



        // GET: Orders/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Owner)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["OwnerId"] = new SelectList(_context.Owners, "ID", "Discriminator");
            ViewData["UserId"] = new SelectList(_context.Users, "ID", "Discriminator");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Done,TotalPrice,OwnerId,UserId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerId"] = new SelectList(_context.Owners, "ID", "Discriminator", order.OwnerId);
            ViewData["UserId"] = new SelectList(_context.Users, "ID", "Discriminator", order.UserId);
            return View(order);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem1ToOrder(int itemId, int orderId)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == itemId);
            var order = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == orderId);

            if (ModelState.IsValid)
            {
                order.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddItemToOrder(int id)
        {
            var selectedProduct = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (selectedProduct != null)
            {
                var currentOrder = await _context.Orders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync();

                if (currentOrder != null)
                {
                    currentOrder.Items.Add(selectedProduct);
                    currentOrder.TotalPrice += selectedProduct.Price;

                    _context.Orders.Update(currentOrder);
                }
                else
                {
                    var order = new Order
                    {
                        Done = false,
                        TotalPrice = selectedProduct.Price,
                        Items = new List<Item> { selectedProduct }
                    };

                    _context.Orders.Add(order);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index2");
            }

            return RedirectToAction("ProductList");
        }


        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_context.Owners, "ID", "Discriminator", order.OwnerId);
            ViewData["UserId"] = new SelectList(_context.Users, "ID", "Discriminator", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Done,TotalPrice,OwnerId,UserId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["OwnerId"] = new SelectList(_context.Owners, "ID", "Discriminator", order.OwnerId);
            ViewData["UserId"] = new SelectList(_context.Users, "ID", "Discriminator", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Owner)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        private bool OrderExists(int id)
        {
          return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
