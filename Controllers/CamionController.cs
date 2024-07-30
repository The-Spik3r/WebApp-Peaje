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
    public class CamionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CamionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Camion
        public async Task<IActionResult> Index()
        {
            return View(await _context.Camion.ToListAsync());
        }

        // GET: Camion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camion = await _context.Camion
                .FirstOrDefaultAsync(m => m.id == id);
            if (camion == null)
            {
                return NotFound();
            }

            return View(camion);
        }

        // GET: Camion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Camion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,matricula,modelo,marca")] Camion camion)
        {
            if (camion.matricula.Length >= 6)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(camion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Payments");
                }
               
            }
            else
            {
                ModelState.AddModelError("matricula", "La matrícula debe tener al menos 7 caracteres.");
            }
            return View(camion);
        }

        // GET: Camion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camion = await _context.Camion.FindAsync(id);
            if (camion == null)
            {
                return NotFound();
            }
            return View(camion);
        }

        // POST: Camion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,matricula,modelo,marca")] Camion camion)
        {
            if (id != camion.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(camion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CamionExists(camion.id))
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
            return View(camion);
        }

        // GET: Camion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camion = await _context.Camion
                .FirstOrDefaultAsync(m => m.id == id);
            if (camion == null)
            {
                return NotFound();
            }

            return View(camion);
        }

        // POST: Camion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var camion = await _context.Camion.FindAsync(id);
            _context.Camion.Remove(camion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CamionExists(int id)
        {
            return _context.Camion.Any(e => e.id == id);
        }
    }
}
