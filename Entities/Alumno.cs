using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KalumNotas.Helpers;

namespace KalumNotas.Entities
{
    public class Alumno 
    {
        //[NumeroCarne]
        public int Carne {get;set;}
        public int NoExpediente {get;set;}
        [Required]
        public string Apellidos {get;set;}
        [Required]
        public string Nombres {get;set;}
        [EmailAddress]
        public string Email{get;set;}

      
    }
}