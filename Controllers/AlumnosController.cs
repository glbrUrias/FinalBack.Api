using System.Collections.Generic;
using KalumNotas.Entities;
using KalumNotas.KalumDBContext;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using KalumNotas.Models;
using AutoMapper;

namespace KalumNotas.Controllers
{
    [Route("/KalumNotas/v1/[controller]")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly KalumNotasDBContext dBContext;
        private readonly ILogger<AlumnosController> logger;
        private readonly IMapper mapper;
        public AlumnosController(IMapper mapper,ILogger<AlumnosController> logger,
        KalumNotasDBContext dBContext)
        {
            this.mapper=mapper;
            this.logger = logger;
            this.dBContext = dBContext;
        }
        //SE USA PARA BUSQUEDA DE VARIOS DATOS, QUE CONCUERDEN COMO UN NOMBRE "FRANCISCO"...TRAE A TODOS LOS FRANCISCOS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlumnoDTO>>>GetAlumnos(string? nombre)//sirve para obtener todos los alumnos o solo uno con el nombre 
        {//esta consulta se hizo doble pporque si no hubiera dos [HttpGet] en dos metodos y caemos en ambiguedad
            
            logger.LogInformation("Iniciando proceso de consulta de alumnos");
            List<Alumno> alumnos=null;
            if(nombre==null)//si no viene nombre entonces nos mostrara todos
            {
                logger.LogDebug("Haciendo consulta de datos sin filtro.");
                alumnos = await dBContext.Alumnos.ToListAsync();
            }
            else//si viene nombre..me mostrara solo el alumno con el nombre
            {
                logger.LogDebug($"Haciendo consultas de datos con el filtro [{nombre}]");
                alumnos = await dBContext.Alumnos.Where(a => a.Nombres.StartsWith($"{nombre}")).ToListAsync();
            }
            if(alumnos == null|| alumnos.Count==0)
            {
                logger.LogWarning("No se encontraron registros de la base de datos");
                return NoContent();
            }
            else 
            {
                logger.LogInformation("Finalizando proceso de busqueda");
                var lista = mapper.Map<List<AlumnoDTO>>(alumnos);

                return lista;
            }
        }
        //SE USA PARA BUSQUEDA COMO ID ...QUE ES DATO UNICO..NO FUNCIONA PARA DATOS QUE SE REPITAN
        [HttpGet("{carne}/{seccion?}", Name = "GetAlumno")] 
        //estos son PARAMETROS PATH tiene que venir fijos sino truena...si se le pone signo ? es opcional el parametro
        //se puede pones hasta seccion=unica.....para que sea siempre asi......y se tiene que pasar fijo como parametros aqui abajao
        public async Task<ActionResult<AlumnoDTO>> Get(int carne,string seccion)//sirve para obtener un alumno por carne 
        {
            logger.LogInformation("Iniciando proceso de busqueda por carne");
            var alumno = await dBContext.Alumnos.FirstOrDefaultAsync(x => x.Carne == carne);
             //var alumno = dBContext.Alumnos.FirstOrDefault(x => x.Carne==carne && x.Nombres.Contains(nombre));
            //para doble consulta se hace como aqui arriba para consultar por nombre y por carne
            //se pone como parametro tambien Delte(int carne,string nombre)
            if(alumno == null)
            {
                logger.LogWarning($"No existe el alumno con el carne {carne}");
                return NotFound();
            }
            else 
            {
                logger.LogInformation("Finalizando proceso de busqueda de alumno por carne");
                return mapper.Map<AlumnoDTO>(alumno);  
            }
        }

        
        [HttpPost]
        public async Task<ActionResult<AlumnoDTO>> Post([FromBody]AlumnoCreateDTO value)//sirve para agregar registros
        {
            logger.LogInformation("Iniciando proceso de aprovisionamiento de alumno");
            var alumno=mapper.Map<Alumno>(value);
            await dBContext.Alumnos.AddAsync(mapper.Map<Alumno>(value));
            await dBContext.SaveChangesAsync();
            var alumnoDTO = mapper.Map<AlumnoDTO>(alumno);
            logger.LogInformation("Finalizando proceso de aprovisionameinto");
            return new CreatedAtRouteResult("GetAlumno",new {carne=alumnoDTO.Carne}, alumnoDTO);// ESTA LA COLOCO EL DE KINAL
            //COMO UNA LLAVE PRIMARIA QUE EL ASIGNA. NO SE AUTO GENERA ("GetAlumno", new {x=value.Carne}, value)
        }

        [HttpPut("{carne}")]
        public async Task<ActionResult> Put(int carne, [FromBody] AlumnoUpdateDTO value)//para modificar registros
        {
            logger.LogInformation($"Iniciando proceso de modificacion de registro con carne {carne}");
            var alumno =await dBContext.Alumnos.FirstOrDefaultAsync(x=>x.Carne==carne);
            alumno.Apellidos=value.Apellidos;
            alumno.Nombres=value.Nombres;
            alumno.Email=value.Email;
            dBContext.Entry(alumno).State= EntityState.Modified;
            logger.LogDebug("Validacion de datos al momento de modifiacr el registro");
            await dBContext.SaveChangesAsync();
            logger.LogInformation("Proceso de modificacion de alumno realizado con exito");
            return NoContent();
            
        }
        [HttpDelete("{carne}")]
        public async Task<ActionResult<AlumnoDTO>> Delete(int carne)
        {
            logger.LogInformation($"Iniciando proceso de eliminacion de registro con carne  {carne}");
            var alumno = await dBContext.Alumnos.FirstOrDefaultAsync(x => x.Carne == carne);
            //var alumno = dBContext.Alumnos.FirstOrDefault(x => x.Carne==carne && x.Nombres.Contains(nombre));
            //para doble consulta se hace como aqui arriba para consultar por nombre y por carne
            //se pone como parametro tambien Delte(int carne,string nombre)
            if(alumno == null)
            {
                logger.LogWarning($"No existen registros con el carne {carne}");
                return NotFound();
            }
            dBContext.Alumnos.Remove(alumno);
            await dBContext.SaveChangesAsync();
            logger.LogInformation($"Finalizando el proceso de eliminacion del registro {alumno}");
            return mapper.Map<AlumnoDTO>(alumno);
        }
    }
}