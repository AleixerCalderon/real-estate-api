using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.DTOs
{
    public class PropertyUpdateDto
    {
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres")]
        public string? Name { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = "La dirección debe tener entre 1 y 200 caracteres")]
        public string? Address { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal? Price { get; set; }

        [Url(ErrorMessage = "Debe ser una URL válida")]
        public string? Image { get; set; }

        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        public int? Year { get; set; }
    }
}