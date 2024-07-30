using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Peajes.Data;
using Peajes.Models.payment;
using Peajes.Models.entity;

namespace Peajes.Controllers
{
    public class paymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public paymentsController(ApplicationDbContext context)
        {
            _context = context;
        }
        private bool PaymentExists(int id)
        {
            return _context.payment.Any(e => e.id == id);
        }


        // GET: payments
        public async Task<IActionResult> Index()
        {
            var payments = await _context.payment.ToListAsync();
            var vehiculos = await _context.Vehiculo.ToListAsync();
            var Motos = await _context.Moto.ToListAsync();
            var Camions = await _context.Camion.ToListAsync();
            var Colectivos = await _context.Colectivo.ToListAsync();

            var viewModel = new PaymentsViewModel
            {
                Payments = payments,
                Vehiculos = vehiculos,
                Motos = Motos,
                Camions = Camions,
                Colectivos = Colectivos
            };

            return View(viewModel);
        }

        // GET: payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.payment
                .FirstOrDefaultAsync(m => m.id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: payments/Create
        public IActionResult Create(string matricula = null, int IdVehiculo = 0 )
        {
            var payment = new payment
            {
                matricula = matricula,
                IdVehiculo = IdVehiculo
            };
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,matricula,pagado,IdVehiculo")] payment payment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(payment);
                    await _context.SaveChangesAsync();

                    // Llama al método Delete de VehiculoController para eliminar el vehículo
                    var vehiculoController = new VehiculoController(_context);
                    var deleteResult = await vehiculoController.Delete(payment.IdVehiculo, true);


                    if (deleteResult is OkResult) // Revisa si la eliminación fue exitosa
                    {
                        return RedirectToAction("Index", "Payments");
                    }
                    else
                    {
                        // Maneja el caso donde la eliminación no fue exitosa
                        return NotFound(); // Puedes ajustar esto según el manejo que desees
                    }
                }
                catch (Exception e)
                {
                    // Handle the exception here
                    // Log the error or perform any necessary actions
                    // You can also return a specific error view or message
                    return View("Error", e);
                }
            }
            return View(payment);
        }



        // GET: payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.payment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        // POST: payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,matricula,pagado")] payment payment)
        {
            if (id != payment.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!paymentExists(payment.id))
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
            return View(payment);
        }

        // GET: payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.payment
                .FirstOrDefaultAsync(m => m.id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.payment.FindAsync(id);
            _context.payment.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool paymentExists(int id)
        {
            return _context.payment.Any(e => e.id == id);
        }
    }

    public class PaymentsViewModel
    {
        public List<payment> Payments { get; set; }
        public List<Vehiculo> Vehiculos { get; set; }
        public List<Moto> Motos { get; set; }
        public List <Camion> Camions { get; set; }
        public List<Colectivo> Colectivos { get; set;}
    }
}
