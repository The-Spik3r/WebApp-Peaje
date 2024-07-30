using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Peajes.Data;
using Peajes.Models.entity;

namespace Peajes.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));

        public VehiculoController(ApplicationDbContext context  )
        {
            _context = context;
        }

        // GET: Vehiculo
        [Authorize(Roles = "Empleado, Supervisor")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vehiculo.ToListAsync());
        }

        // GET: Vehiculo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculo
                .FirstOrDefaultAsync(m => m.id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // GET: Vehiculo/Create
        public async Task<IActionResult> Create()
        {
            // Obtener la tarifa antes de mostrar la vista
            var tarifa = await _context.Tarifa.FirstOrDefaultAsync(t => t.TipoVehiculo == "Vehiculo");
            

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado, Supervisor")]
        public async Task<IActionResult> Create([Bind("id,matricula,modelo,marca")] Vehiculo vehiculo)
        {
            try
            {
                log.Debug("Creando vehiculo");

                // Obtener la tarifa antes de cualquier validación
                var tarifa = await _context.Tarifa.FirstOrDefaultAsync(t => t.TipoVehiculo == "Vehiculo");

                if (tarifa == null)
                {
                    // Si no se encuentra la tarifa, agrega un mensaje de error al ModelState
                    ModelState.AddModelError("tarifa", "No se encontró una tarifa para este tipo de vehículo.");
                    return View(vehiculo);
                }

                // Continuar con las validaciones del modelo
                if (ModelState.IsValid)
                {
                    if (vehiculo.matricula.Length >= 7)
                    {
                        // Si la matrícula cumple con el requisito, asigna la tarifa encontrada al ViewBag
                        log.Debug("La matrícula cumple con el requisito de longitud.");

                        try
                        {
                            CreateTxt createTxt = new CreateTxt();
                            log.Debug("Llamando al método CrearArchivo de CreateTxt.");
                            createTxt.Create();
                            log.Debug("Archivo creado exitosamente.");
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error al crear el archivo", ex);
                            ModelState.AddModelError("archivo", "Hubo un problema al crear el archivo.");
                            return View(vehiculo);
                        }

                        ViewBag.TarifaEncontrada = tarifa.precio;
                        log.DebugFormat("Subiendo a la base de datos el vehiculo con matricula: '{0}', su modelo y marca son '{1}','{2}'", vehiculo.matricula, vehiculo.modelo, vehiculo.marca);
                        _context.Add(vehiculo);
                        await _context.SaveChangesAsync();
                        ModelState.AddModelError("matricula", "Vehiculo registrado con éxito");
                        return RedirectToAction("Index", "Payments");
                    }
                    else
                    {
                        // Si la matrícula no cumple con el requisito, agrega un mensaje de error al ModelState
                        ModelState.AddModelError("matricula", "La matrícula debe tener al menos 7 caracteres.");
                    }
                }

                // Si el modelo no es válido por otros motivos, retorna la vista con los errores
                return View(vehiculo);
            }
            catch (Exception ex)
            {
                log.Error("Error al crear el vehículo", ex);
                Console.WriteLine($"Error al crear el vehículo: {ex.Message}");
                // Considera retornar una vista de error o manejar la excepción de manera adecuada
                return View("Error"); // Asegúrate de tener una vista de Error o maneja esto de otra manera
            }
        }




        // GET: Vehiculo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculo.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }
            return View(vehiculo);
        }

        // POST: Vehiculo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,matricula,modelo,marca")] Vehiculo vehiculo)
        {
            if (id != vehiculo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehiculoExists(vehiculo.id))
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
            return View(vehiculo);
        }

        // GET: Vehiculo/Delete/5
        public async Task<IActionResult> Delete(int? id, bool fromAnotherController = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculo.FirstOrDefaultAsync(m => m.id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            if (fromAnotherController)
            {
                _context.Vehiculo.Remove(vehiculo);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return View(vehiculo);
        }


        // POST: Vehiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehiculo = await _context.Vehiculo.FindAsync(id);
            _context.Vehiculo.Remove(vehiculo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Payments");
        }

        private bool VehiculoExists(int id)
        {
            return _context.Vehiculo.Any(e => e.id == id);
        }
    }
}
