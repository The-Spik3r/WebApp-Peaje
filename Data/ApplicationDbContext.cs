using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Peajes.Models.entity;
using Peajes.Models.role;
using Peajes.Models.precios;
using Peajes.Models;
using Peajes.Models.payment;

namespace Peajes.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Peajes.Models.entity.Vehiculo> Vehiculo { get; set; }
        public DbSet<Peajes.Models.entity.Camion> Camion { get; set; }
        public DbSet<Peajes.Models.entity.Moto> Moto { get; set; }
        public DbSet<Peajes.Models.entity.Colectivo> Colectivo { get; set; }
        public DbSet<Peajes.Models.role.Persona> Persona { get; set; }
        public DbSet<Peajes.Models.role.Empleado> Empleado { get; set; }
        public DbSet<Peajes.Models.role.Supervisor> Supervisor { get; set; }
        public DbSet<Peajes.Models.precios.Precio> Precio { get; set; }
        public DbSet<Peajes.Models.Tarifa> Tarifa { get; set; }
        public DbSet<Peajes.Models.payment.payment> payment { get; set; }
    }
}
