using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcP1.Data;
using MvcP1.Models;

namespace MvcP1.Controllers
{
    public class NoteController : Controller
    {
        private readonly AppDbContext _context;

        public NoteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Note
        public async Task<IActionResult> Index()
        {
            return View(await _context.Notes.ToListAsync());
        }

        // GET: Note/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noteModel = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (noteModel == null)
            {
                return NotFound();
            }

            return View(noteModel);
        }

        // GET: Note/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Note/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteModel noteModel, string tagsText)
        {
            noteModel.Tags = ParseTags(tagsText);

            if (!ModelState.IsValid)
                return View(noteModel);

            _context.Add(noteModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Note/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noteModel = await _context.Notes.FindAsync(id);
            if (noteModel == null)
            {
                return NotFound();
            }
            return View(noteModel);
        }

        // POST: Note/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoteModel noteModel, string tagsText)
        {
            if (id != noteModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    noteModel.Tags = ParseTags(tagsText);
                    _context.Update(noteModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteModelExists(noteModel.Id))
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
            return View(noteModel);
        }

        // GET: Note/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noteModel = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (noteModel == null)
            {
                return NotFound();
            }

            return View(noteModel);
        }

        // POST: Note/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var noteModel = await _context.Notes.FindAsync(id);
            if (noteModel != null)
            {
                _context.Notes.Remove(noteModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteModelExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }

        private static List<string> ParseTags(string? input)
        {
            return (input ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(x => x.Length > 0)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}
