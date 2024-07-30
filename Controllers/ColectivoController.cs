using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Peajes.Data;
using Peajes.Models.entity;

namespace Peajes.Controllers
{
    public class ColectivoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ColectivoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Colectivo
        public async Task<IActionResult> Index()
        {
            return View(await _context.Colectivo.ToListAsync());
        }

        // GET: Colectivo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colectivo = await _context.Colectivo
                .FirstOrDefaultAsync(m => m.id == id);
            if (colectivo == null)
            {
                return NotFound();
            }

            return View(colectivo);
        }

        // GET: Colectivo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Colectivo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,matricula,marca,modelo,pisos")] Colectivo colectivo)
        {
            if(colectivo.matricula.Length >= 6 || colectivo.pisos.ToString() != null)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(colectivo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Payments");
                }
            }
            else
            {
                ModelState.AddModelError("matricula", "La matricula debe tener al menos 6 caracteres o los pisos no puede estar vacio");
            }
          
            return View(colectivo);
        }

        // GET: Colectivo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colectivo = await _context.Colectivo.FindAsync(id);
            if (colectivo == null)
            {
                return NotFound();
            }
            return View(colectivo);
        }

        // POST: Colectivo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,matricula,marca,modelo,pisos")] Colectivo colectivo)
        {
            if (id != colectivo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colectivo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColectivoExists(colectivo.id))
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
            return View(colectivo);
        }

        // GET: Colectivo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colectivo = await _context.Colectivo
                .FirstOrDefaultAsync(m => m.id == id);
            if (colectivo == null)
            {
                return NotFound();
            }

            return View(colectivo);
        }

        // POST: Colectivo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var colectivo = await _context.Colectivo.FindAsync(id);
            _context.Colectivo.Remove(colectivo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColectivoExists(int id)
        {
            return _context.Colectivo.Any(e => e.id == id);
        }
    }
}
