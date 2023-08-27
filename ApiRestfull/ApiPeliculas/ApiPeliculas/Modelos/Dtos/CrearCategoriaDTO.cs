using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class CrearCategoriaDTO
    {
        // Esta validacion es importante sino se creara vacia el nombre de categoria
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El numero maximo de caracteres es de 100")]
        public string Nombre { get; set; }
    }
}
