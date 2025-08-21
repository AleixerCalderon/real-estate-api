using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.DTOs
{
    public class OwnerUpdateDto
    {
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres")]
        public string? Name { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = "La dirección debe tener entre 1 y 200 caracteres")]
        public string? Address { get; set; }

        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string? Phone { get; set; }

        public DateTime? Birthday { get; set; }
    }
}