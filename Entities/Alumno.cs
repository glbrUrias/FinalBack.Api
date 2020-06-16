using System.ComponentModel.DataAnnotations;

namespace KalumNotas.Entities
{
    public class Alumno
    {
         public int Carne {get;set;}
        //[Required]
        public int NoExpediente {get;set;}
        //[Required]
        public string Apellidos {get;set;}
        //[Required]
        public string Nombres {get;set;}
        //[EmailAddress]
        public string Email{get;set;}
        
    }
}