using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly ILogger<PropertiesController> _logger;

        public PropertiesController(IPropertyService propertyService, ILogger<PropertiesController> logger)
        {
            _propertyService = propertyService;
            _logger = logger;
        }

        /// <summary>
        /// AAB (19 08 2025)
        /// Obtiene todas las propiedades
        /// </summary>
        /// <returns>Lista de propiedades</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<PropertyDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 500)]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<PropertyDto>>>> GetAllProperties()
        {
            _logger.LogInformation("Obteniendo todas las propiedades");
            
            var result = await _propertyService.GetAllPropertiesAsync();
            
            if (!result.Success)
            {
                _logger.LogError("Error al obtener propiedades: {Message}", result.Message);
                return StatusCode(500, result);
            }

            return Ok(result);
        }

        /// <summary>
        /// AAB (19 08 2025)
        /// Obtiene una propiedad por ID
        /// </summary>
        /// <param name="id">ID de la propiedad</param>
        /// <returns>Propiedad encontrada</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<PropertyDto>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 404)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 500)]
        public async Task<ActionResult<ApiResponseDto<PropertyDto>>> GetProperty(string id)
        {
            _logger.LogInformation("Obteniendo propiedad con ID: {Id}", id);
            
            var result = await _propertyService.GetPropertyByIdAsync(id);
            
            if (!result.Success)
            {
                _logger.LogWarning("Propiedad no encontrada: {Id}", id);
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// AAB (20 08 2025)
        /// Busca propiedades con filtros
        /// </summary>
        /// <param name="filter">Filtros de búsqueda</param>
        /// <returns>Propiedades filtradas</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<PropertyDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 400)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 500)]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<PropertyDto>>>> SearchProperties([FromQuery] PropertyFilterDto filter)
        {
            _logger.LogInformation("Buscando propiedades con filtros: {@Filter}", filter);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseDto<object>.ErrorResponse("Parámetros de búsqueda inválidos"));
            }

            var result = await _propertyService.GetFilteredPropertiesAsync(filter);
            
            if (!result.Success)
            {
                _logger.LogError("Error al filtrar propiedades: {Message}", result.Message);
                return StatusCode(500, result);
            }

            return Ok(result);
        }

        /// <summary>
        /// AAB (20 08 2025)
        /// Crea una nueva propiedad
        /// </summary>
        /// <param name="propertyDto">Datos de la propiedad</param>
        /// <returns>Propiedad creada</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseDto<PropertyDto>), 201)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 400)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 500)]
        public async Task<ActionResult<ApiResponseDto<PropertyDto>>> CreateProperty([FromBody] PropertyCreateDto propertyDto)
        {
            _logger.LogInformation("Creando nueva propiedad: {@Property}", propertyDto);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseDto<object>.ErrorResponse("Datos de propiedad inválidos"));
            }

            var result = await _propertyService.CreatePropertyAsync(propertyDto);
            
            if (!result.Success)
            {
                _logger.LogError("Error al crear propiedad: {Message}", result.Message);
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetProperty), new { id = result.Data!.Id }, result);
        }

        /// <summary>
        /// AAB (20 08 2025)
        /// Actualiza una propiedad existente
        /// </summary>
        /// <param name="id">ID de la propiedad</param>
        /// <param name="propertyDto">Datos actualizados</param>
        /// <returns>Propiedad actualizada</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<PropertyDto>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 400)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 404)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 500)]
        public async Task<ActionResult<ApiResponseDto<PropertyDto>>> UpdateProperty(string id, [FromBody] PropertyUpdateDto propertyDto)
        {
            _logger.LogInformation("Actualizando propiedad {Id}: {@Property}", id, propertyDto);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseDto<object>.ErrorResponse("Datos de actualización inválidos"));
            }

            var result = await _propertyService.UpdatePropertyAsync(id, propertyDto);
            
            if (!result.Success)
            {
                if (result.Message.Contains("no encontrada"))
                {
                    return NotFound(result);
                }
                _logger.LogError("Error al actualizar propiedad: {Message}", result.Message);
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// AAB (20 08 2025)
        /// Elimina una propiedad
        /// </summary>
        /// <param name="id">ID de la propiedad</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 404)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 500)]
        public async Task<ActionResult<ApiResponseDto<bool>>> DeleteProperty(string id)
        {
            _logger.LogInformation("Eliminando propiedad: {Id}", id);
            
            var result = await _propertyService.DeletePropertyAsync(id);
            
            if (!result.Success)
            {
                if (result.Message.Contains("no encontrada"))
                {
                    return NotFound(result);
                }
                _logger.LogError("Error al eliminar propiedad: {Message}", result.Message);
                return StatusCode(500, result);
            }

            return Ok(result);
        }
    }
}