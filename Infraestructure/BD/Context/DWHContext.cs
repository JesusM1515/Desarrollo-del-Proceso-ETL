using Domain.Entities.DWH;
using Domain.Entities.DWH.Dimensions;
using Domain.Entities.DWH.Facts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.BD.Context
{
    public class DWHContext : DbContext
    {
        public DWHContext(DbContextOptions<DWHContext> dhwContext) : base(dhwContext)
        {

        }

        public DbSet<Dim_Tiempo> Dim_Tiempo { get; set; }
        public DbSet<Dim_Sentimiento> Dim_Sentimiento { get; set; }
        public DbSet<Dim_Producto> Dim_Producto { get; set; }
        public DbSet<Dim_FuentesDatos> Dim_FuentesDatos { get; set; }
        public DbSet<Dim_Clientes> Dim_Clientes { get; set; }
        public DbSet<Fact_Opiniones> Fact_Opiniones { get; set; }
        public DbSet<StFact_Opiniones> StFact_Opiniones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                base.OnModelCreating(modelBuilder);
        }
    }
}
