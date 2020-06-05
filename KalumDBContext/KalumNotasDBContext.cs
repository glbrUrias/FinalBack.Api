using KalumNotas.Entities;
using Microsoft.EntityFrameworkCore;

namespace KalumNotas.KalumDBContext
{
    public class KalumNotasDBContext :DbContext
    {
        public KalumNotasDBContext(DbContextOptions<KalumNotasDBContext> options)
            :base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alumno>().HasKey(x=> new{x.Carne});

        }
        public DbSet<Alumno> Alumnos {get;set;}
    }
}