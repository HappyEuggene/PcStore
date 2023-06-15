using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PcStore.Data;
using PcStore.Models;

namespace PcStore.Controllers
{
    public class ItemSpecsController : Controller
    {
        private readonly Somkin1Context _context;

        public ItemSpecsController(Somkin1Context context)
        {
            _context = context;
        }

        // GET: ItemSpecs
        public async Task<IActionResult> Index()
        {
              return _context.ItemSpecs != null ? 
                          View(await _context.ItemSpecs.ToListAsync()) :
                          Problem("Entity set 'Somkin1Context.ItemSpecs'  is null.");
        }

        // GET: ItemSpecs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ItemSpecs == null)
            {
                return NotFound();
            }

            var itemSpec = await _context.ItemSpecs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemSpec == null)
            {
                return NotFound();
            }

            return View(itemSpec);
        }

        // GET: ItemSpecs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ItemSpecs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Frequency,MemCapacity,MemType,Cores,Power")] ItemSpec itemSpec)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemSpec);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itemSpec);
        }

        // GET: ItemSpecs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ItemSpecs == null)
            {
                return NotFound();
            }

            var itemSpec = await _context.ItemSpecs.FindAsync(id);
            if (itemSpec == null)
            {
                return NotFound();
            }
            return View(itemSpec);
        }

        // POST: ItemSpecs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Frequency,MemCapacity,MemType,Cores,Power")] ItemSpec itemSpec)
        {
            if (id != itemSpec.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemSpec);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemSpecExists(itemSpec.Id))
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
            return View(itemSpec);
        }

        // GET: ItemSpecs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ItemSpecs == null)
            {
                return NotFound();
            }

            var itemSpec = await _context.ItemSpecs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemSpec == null)
            {
                return NotFound();
            }

            return View(itemSpec);
        }

        // POST: ItemSpecs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ItemSpecs == null)
            {
                return Problem("Entity set 'Somkin1Context.ItemSpecs'  is null.");
            }
            var itemSpec = await _context.ItemSpecs.FindAsync(id);
            if (itemSpec != null)
            {
                _context.ItemSpecs.Remove(itemSpec);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemSpecExists(int id)
        {
          return (_context.ItemSpecs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
