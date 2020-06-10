using System.Collections.Generic;
using KalumNotas.Entities;
using KalumNotas.KalumDBContext;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
        public ActionResult<IEnumerable<Alumno>> GetAlumnos(string? nombre)//sirve para obtener todos los alumnos o solo uno con el nombre 
        {//esta consulta se hizo doble pporque si no hubiera dos [HttpGet] en dos metodos y caemos en ambiguedad
            List<Alumno> alumnos=null;
            if(nombre==null)//si no viene nombre entonces nos mostrara todos
            {
                alumnos = dBContext.Alumnos.ToList();
            }
            else//si viene nombre..me mostrara solo el alumno con el nombre
            {
                alumnos = dBContext.Alumnos.Where(a => a.Nombres.StartsWith($"{nombre}")).ToList();
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
        [HttpGet("{carne}", Name = "GetAlumno")]
        public ActionResult<Alumno> Get(int carne)//sirve para obtener un alumno por carne 
        {
            var alumno = dBContext.Alumnos.FirstOrDefault(x => x.Carne == carne);
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
        public ActionResult<Alumno> Post([FromBody]Alumno value)//sirve para agregar registros
        {
            dBContext.Alumnos.Add(value);
            dBContext.SaveChanges();
            return new CreatedAtRouteResult("GetAlumno",new {x=value.Carne}, value);// ESTA LA COLOCO EL DE KINAL
            //COMO UNA LLAVE PRIMARIA QUE EL ASIGNA. NO SE AUTO GENERA ("GetAlumno", new {x=value.Carne}, value)
        }

        [HttpPut("{carne}")]
        public ActionResult Put(int carne, [FromBody] Alumno value)//para modificar registros
        {
            if(carne != value.Carne)//si no es el mismo carne
            {
                return BadRequest();//muestra ERROR 400 la informacion dada no coincide
            }
            dBContext.Entry(value).State= EntityState.Modified;
            dBContext.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{carne}")]
        public ActionResult<Alumno> Delete(int carne)
        {
            var alumno = dBContext.Alumnos.FirstOrDefault(x => x.Carne == carne);
            //var alumno = dBContext.Alumnos.FirstOrDefault(x => x.Carne==carne && x.Nombres.Contains(nombre));
            //para doble consulta se hace como aqui arriba para consultar por nombre y por carne
            //se pone como parametro tambien Delte(int carne,string nombre)
            if(alumno == null)
            {
                return NotFound();
            }
            dBContext.Alumnos.Remove(alumno);
            dBContext.SaveChanges();
            return alumno;
        }
    }
}