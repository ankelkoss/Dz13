using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcP1.Data;
using MvcP1.Models;

namespace MvcP1.Controllers
{
    public class ContactsController : Controller
    {
        private readonly AppDbContext _context;

        public ContactsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contacts.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactsModel = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactsModel == null)
            {
                return NotFound();
            }

            return View(contactsModel);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Phone,PhoneAlt,Email,DescShort,Id,CreatedAt,UpdatedAt")] ContactsModel contactsModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactsModel);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactsModel = await _context.Contacts.FindAsync(id);
            if (contactsModel == null)
            {
                return NotFound();
            }
            return View(contactsModel);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Phone,PhoneAlt,Email,DescShort,Id,CreatedAt,UpdatedAt")] ContactsModel contactsModel)
        {
            if (id != contactsModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactsModelExists(contactsModel.Id))
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
            return View(contactsModel);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactsModel = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactsModel == null)
            {
                return NotFound();
            }

            return View(contactsModel);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactsModel = await _context.Contacts.FindAsync(id);
            if (contactsModel != null)
            {
                _context.Contacts.Remove(contactsModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactsModelExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
