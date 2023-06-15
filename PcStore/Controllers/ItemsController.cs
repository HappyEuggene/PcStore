using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PcStore.Data;
using PcStore.Models;
using PcStore.Utilities;

namespace PcStore.Controllers
{
    public class ItemsController : Controller
    {
        private readonly Somkin1Context _context;
        private readonly UserManager<User> _userManager;

        public ItemsController(Somkin1Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var somkin1Context = _context.Items.Include(i => i.ItemSpec).Include(i => i.ItemType);
            return View(await somkin1Context.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemSpec)
                .Include(i => i.ItemType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["ItemSpecId"] = new SelectList(_context.ItemSpecs, "Id", "Frequency");
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "Manufacture");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,ItemSpecId,ItemTypeId")] Item item)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                item.UserId = userId;
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemSpecId"] = new SelectList(_context.ItemSpecs, "Id", "Id", item.ItemSpecId);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "Id", item.ItemTypeId);
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["ItemSpecId"] = new SelectList(_context.ItemSpecs, "Id", "Id", item.ItemSpecId);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "Id", item.ItemTypeId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ItemSpecId,ItemTypeId")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId  = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    item.UserId = userId;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            ViewData["ItemSpecId"] = new SelectList(_context.ItemSpecs, "Id", "Id", item.ItemSpecId);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "Id", item.ItemTypeId);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemSpec)
                .Include(i => i.ItemType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'Somkin1Context.Items'  is null.");
            }
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return (_context.Items?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public IActionResult Import()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (fileExcel != null)
            {
                using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                {
                    await fileExcel.CopyToAsync(stream);
                    using (var workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                    {
                        foreach (var worksheet in workBook.Worksheets)
                        {
                            foreach (var row in worksheet.RowsUsed())
                            {
                                try
                                {
                                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                                    var itemSpec = await _context.ItemSpecs.Where(x => x.Id == Convert.ToInt32(row.Cell(3).Value)).FirstOrDefaultAsync();
                                    var itemType = await _context.ItemTypes.Where(x => x.Id == Convert.ToInt32(row.Cell(4).Value)).FirstOrDefaultAsync();
                                    var item = new Item();
                                    item.Name = row.Cell(1).Value.ToString();
                                    item.Price = double.Parse(row.Cell(2).Value.ToString());
                                    item.ItemSpec = itemSpec;
                                    item.ItemType = itemType;
                                    item.UserId = userId;
                                    _context.Items.Add(item);
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                            }
                        }
                    }
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var items = _context.Items.Where(i=>i.UserId == userId).Include(i => i.ItemSpec).Include(i => i.ItemType).ToList();
                var worksheet = workbook.Worksheets.Add("All your products");
                worksheet.Cell("A1").Value = "Name";
                worksheet.Cell("B1").Value = "Price";
                worksheet.Cell("C1").Value = "ItemSpecId";
                worksheet.Cell("D1").Value = "ItemTypeId";
                int row = 2;
                foreach (var item in items)
                {
                    worksheet.Cell(row, 1).Value = item.Name;
                    worksheet.Cell(row, 2).Value = item.Price;
                    worksheet.Cell(row, 3).Value = item.ItemSpecId;
                    worksheet.Cell(row, 4).Value = item.ItemTypeId;
                    row++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"Items{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> BecomeSeller()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            result = await _userManager.AddToRoleAsync(user, Roles.Seller);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
