using System.Collections.Generic;
using KalumNotas.Entities;
using KalumNotas.KalumDBContext;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
        public ActionResult<Alumno> Get(int carne)
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

    }
}