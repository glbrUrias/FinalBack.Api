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
        protected override void OnModelCreating(ModelBuilder modelBuilder)//esto se hace cuando no es auto incrementable
        //la llave primria......si es auto incrementable se coloca solo asi: public DbSet<Alumno> Alumnos {get;set;}
        {
            modelBuilder.Entity<Alumno>().HasKey(x=> new{x.Carne});
            //aqui se coloca cuando hay una relacion de uno a muchos o de una a una como aqui arriba
            //se pone la lleve primaria si no dice AlumnoId, si dice asi como aqui arriba carne
            modelBuilder.Entity<AsignacionAlumno>().HasKey(x=> new{x.AsignacionId});

        }
        public DbSet<Alumno> Alumnos {get;set;}
        //aqui se coloca cuando hay una relacion de uno a muchos o de una a una como aqui arriba
        public DbSet<AsignacionAlumno> AsignacionAlumnos {get;set;}
        public DbSet<DetalleActividad> DetalleActividades {get;set;}
        public DbSet<DetalleNota> DetalleNotas {get;set;}
        //public DbSet<Modulo> Modulos {get;set;}
        //public DbSet<Seminario> Seminarios {get;set;}
    }
}