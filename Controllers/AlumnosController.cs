using System.Collections.Generic;
using KalumNotas.Entities;
using KalumNotas.KalumDBContext;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KalumNotas.Controllers
{
    [Route("/KalumNotas/v1/[controller]")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly KalumNotasDBContext dBContext;
        public AlumnosController(KalumNotasDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alumno>>>GetAlumnos(string? nombre)//sirve para obtener todos los alumnos o solo uno con el nombre 
        {//esta consulta se hizo doble pporque si no hubiera dos [HttpGet] en dos metodos y caemos en ambiguedad
            List<Alumno> alumnos=null;
            if(nombre==null)//si no viene nombre entonces nos mostrara todos
            {
                alumnos = await dBContext.Alumnos.ToListAsync();
            }
            else//si viene nombre..me mostrara solo el alumno con el nombre
            {
                alumnos = await dBContext.Alumnos.Where(a => a.Nombres.StartsWith($"{nombre}")).ToListAsync();
            }
            if(alumnos == null)
            {
                return NoContent();
            }
            else 
            {
                return alumnos;
            }
        }
        [HttpGet("{carne}/{seccion?}", Name = "GetAlumno")] 
        //estos son PARAMETROS PATH tiene que venir fijos sino truena...si se le pone signo ? es opcional el parametro
        //se puede pones hasta seccion=unica.....para que sea siempre asi......y se tiene que pasar fijo como parametros aqui abajao
        public async Task<ActionResult<Alumno>> Get(int carne,string seccion)//sirve para obtener un alumno por carne 
        {
            var alumno = await dBContext.Alumnos.FirstOrDefaultAsync(x => x.Carne == carne);
             //var alumno = dBContext.Alumnos.FirstOrDefault(x => x.Carne==carne && x.Nombres.Contains(nombre));
            //para doble consulta se hace como aqui arriba para consultar por nombre y por carne
            //se pone como parametro tambien Delte(int carne,string nombre)
            if(alumno == null)
            {
                return NotFound();
            }
            else 
            {
                return alumno;
            }
        }

        
        [HttpPost]
        public async Task<ActionResult<Alumno>> Post([FromBody]Alumno value)//sirve para agregar registros
        {
            await dBContext.Alumnos.AddAsync(value);
            await dBContext.SaveChangesAsync();
            return new CreatedAtRouteResult("GetAlumno",new {x=value.Carne}, value);// ESTA LA COLOCO EL DE KINAL
            //COMO UNA LLAVE PRIMARIA QUE EL ASIGNA. NO SE AUTO GENERA ("GetAlumno", new {x=value.Carne}, value)
        }

        [HttpPut("{carne}")]
        public async Task<ActionResult> Put(int carne, [FromBody] Alumno value)//para modificar registros
        {
            if(carne != value.Carne)//si no es el mismo carne
            {
                return BadRequest();//muestra ERROR 400 la informacion dada no coincide
            }
            dBContext.Entry(value).State= EntityState.Modified;
            await dBContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{carne}")]
        public async Task<ActionResult<Alumno>> Delete(int carne)
        {
            var alumno = await dBContext.Alumnos.FirstOrDefaultAsync(x => x.Carne == carne);
            //var alumno = dBContext.Alumnos.FirstOrDefault(x => x.Carne==carne && x.Nombres.Contains(nombre));
            //para doble consulta se hace como aqui arriba para consultar por nombre y por carne
            //se pone como parametro tambien Delte(int carne,string nombre)
            if(alumno == null)
            {
                return NotFound();
            }
            dBContext.Alumnos.Remove(alumno);
            await dBContext.SaveChangesAsync();
            return alumno;
        }
    }
}