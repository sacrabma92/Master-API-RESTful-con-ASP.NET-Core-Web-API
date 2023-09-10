using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos.Dtos
{
    public class ActualizaPeliculaDTO
    {
        public int PeliculaId { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        public string RutaImagen { get; set; }
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La duración es obligatoria")]
        public int Duracion { get; set; }
        public enum TipoClasificacion { Siente, Trece, Dieciseis, Dieciocho }
        //public TipoClasificacion Clasificacion { get; set; }
        //public DateTime FechaCreacion { get; set; }
        public int CategoriaId { get; set; }
    }
}
