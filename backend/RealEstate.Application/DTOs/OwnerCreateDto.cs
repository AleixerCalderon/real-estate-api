using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.DTOs
{
    public class OwnerCreateDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es requerida")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "La dirección debe tener entre 1 y 200 caracteres")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        public DateTime Birthday { get; set; }
    }
}