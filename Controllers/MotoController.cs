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
    public class MotoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MotoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Moto
        public async Task<IActionResult> Index()
        {
            return View(await _context.Moto.ToListAsync());
        }

        // GET: Moto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moto = await _context.Moto
                .FirstOrDefaultAsync(m => m.id == id);
            if (moto == null)
            {
                return NotFound();
            }

            return View(moto);
        }

        // GET: Moto/Create
        public async Task<IActionResult> Create()
        {
            // Obtener la tarifa antes de mostrar la vista
            var tarifa = await _context.Tarifa.FirstOrDefaultAsync(t => t.TipoVehiculo == "Moto");

            if (tarifa == null)
            {
                // Si no se encuentra la tarifa, agrega un mensaje de error al ModelState
                ModelState.AddModelError("tarifa", "No se encontró una tarifa para este tipo de vehículo.");
                // Puedes manejar esto de una manera que tenga sentido para tu aplicación
                // Por ejemplo, redirigir a una página de error o mostrar un mensaje en la vista
            }
            else
            {
                // Pasar la tarifa a la vista usando ViewBag
                ViewBag.TarifaEncontrada = tarifa.precio;
            }

            return View();
        }
        // POST: Moto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,matricula,cilindraje,modelo,marca")] Moto moto)
        {
            if (moto.matricula.Length >= 6)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(moto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Payments");
                }
            }
            else
            {
                ModelState.AddModelError("matricula", "La matrícula debe tener al menos 6 caracteres");
            }
            
            return View(moto);
        }

        // GET: Moto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moto = await _context.Moto.FindAsync(id);
            if (moto == null)
            {
                return NotFound();
            }
            return View(moto);
        }

        // POST: Moto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,matricula,cilindraje,modelo,marca")] Moto moto)
        {
            if (id != moto.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MotoExists(moto.id))
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
            return View(moto);
        }

        // GET: Moto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moto = await _context.Moto
                .FirstOrDefaultAsync(m => m.id == id);
            if (moto == null)
            {
                return NotFound();
            }

            return View(moto);
        }

        // POST: Moto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var moto = await _context.Moto.FindAsync(id);
            _context.Moto.Remove(moto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MotoExists(int id)
        {
            return _context.Moto.Any(e => e.id == id);
        }
    }
}
