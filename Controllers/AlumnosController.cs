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
        public ActionResult<IEnumerable<Alumno>> Get()
        {
            var alumnos = dBContext.Alumnos.ToList();
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
        public ActionResult<Alumno> Get(int carne)//sirve para obtener todos los alumnos...mostrar 
        {
            var alumno = dBContext.Alumnos.FirstOrDefault(x => x.Carne == carne);
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