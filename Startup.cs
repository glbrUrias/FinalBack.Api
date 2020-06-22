using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using KalumNotas.KalumDBContext;
using Microsoft.Extensions.DependencyInjection;
using KalumNotas.Entities;
using KalumNotas.Models;

namespace KalumNotas
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(configuration => {
            configuration.CreateMap<Alumno,AlumnoDTO>().ConstructUsing(Alumno => new AlumnoDTO{FullName = $"{Alumno.Apellidos} {Alumno.Nombres}"});
            configuration.CreateMap<AlumnoCreateDTO,Alumno>();//SE COLOCAN LOS QUE QUERRAMOS QUE SE MUESTREN 
            configuration.CreateMap<AlumnoUpdateDTO,Alumno>();


            configuration.CreateMap<AsignacionAlumno,AsignacionAlumnoDTO>().ConstructUsing(AsignacionAlumno => new AsignacionAlumnoDTO{AsignacionId = AsignacionAlumno.AsignacionId});
            configuration.CreateMap<AsignacionCreateDTO,AsignacionAlumno>();//SE COLOCAN LOS QUE QUERRAMOS QUE SE MUESTREN
            configuration.CreateMap<AsignacionUpdateDTO,AsignacionAlumno>();
            
    //SE COLOCAN LOS QUE QUERRAMOS QUE SE MUESTREN
            configuration.CreateMap<DetalleActividad,DetalleActividadDTO>().ConstructUsing(DetalleActividad => new DetalleActividadDTO{
                DetalleActividadId = DetalleActividad.DetalleActividadId,
                SeminarioId=DetalleActividad.SeminarioId,NombreActividad= DetalleActividad.NombreActividad,
                NotaActividad=DetalleActividad.NotaActividad, FechaCreacion=DetalleActividad.FechaCreacion,
                FechaEntrega=DetalleActividad.FechaEntrega, FechaPostergacion=DetalleActividad.FechaPostergacion,
                Estado=DetalleActividad.Estado});
            configuration.CreateMap<DetalleActividadCreateDTO,DetalleActividad>();
            configuration.CreateMap<DetalleActividadUpdateDTO,DetalleActividad>();

/*
            configuration.CreateMap<AsignacionAlumno,AsignacionAlumnoDTO>().ConstructUsing(AsignacionAlumno => new AsignacionAlumnoDTO{AsignacionId = AsignacionAlumno.AsignacionId});
            configuration.CreateMap<AsignacionCreateDTO,AsignacionAlumno>();
            configuration.CreateMap<AsignacionUpdateDTO,AsignacionAlumno>();

            configuration.CreateMap<AsignacionAlumno,AsignacionAlumnoDTO>().ConstructUsing(AsignacionAlumno => new AsignacionAlumnoDTO{AsignacionId = AsignacionAlumno.AsignacionId});
            configuration.CreateMap<AsignacionCreateDTO,AsignacionAlumno>();
            configuration.CreateMap<AsignacionUpdateDTO,AsignacionAlumno>();

            configuration.CreateMap<AsignacionAlumno,AsignacionAlumnoDTO>().ConstructUsing(AsignacionAlumno => new AsignacionAlumnoDTO{AsignacionId = AsignacionAlumno.AsignacionId});
            configuration.CreateMap<AsignacionCreateDTO,AsignacionAlumno>();
            configuration.CreateMap<AsignacionUpdateDTO,AsignacionAlumno>();
*/
            },
            typeof(Startup));
            
            services.AddDbContext<KalumNotasDBContext>(options => 
                options.UseSqlServer(Configuration
                    .GetConnectionString("DefaultConnectionString"))
            );
            //services.AddControllers()..AddNewtonsoftJson(options =>
            //options.SerializerSettings.ReferenceLoopHandling = 
            //Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}