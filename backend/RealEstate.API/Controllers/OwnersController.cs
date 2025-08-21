using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerService _ownerService;
        private readonly ILogger<OwnersController> _logger;

        public OwnersController(IOwnerService ownerService, ILogger<OwnersController> logger)
        {
            _ownerService = ownerService;
            _logger = logger;
        }

        /// <summary>
        /// AAB (19 08 2025)
        /// Obtiene todos los propietarios
        /// </summary>
        /// <returns>Lista de propietarios</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<OwnerDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 500)]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<OwnerDto>>>> GetAllOwners()
        {
            _logger.LogInformation("Obteniendo todos los propietarios");
            
            var result = await _ownerService.GetAllOwnersAsync();
            
            if (!result.Success)
            {
                _logger.LogError("Error al obtener propietarios: {Message}", result.Message);
                return StatusCode(500, result);
            }

            return Ok(result);
        }

        /// <summary>
        /// AAB (19 08 2025)
        /// Obtiene un propietario por ID
        /// </summary>
        /// <param name="id">ID del propietario</param>
        /// <returns>Propietario encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<OwnerDto>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 404)]
        public async Task<ActionResult<ApiResponseDto<OwnerDto>>> GetOwner(string id)
        {
            _logger.LogInformation("Obteniendo propietario con ID: {Id}", id);
            
            var result = await _ownerService.GetOwnerByIdAsync(id);
            
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// AAB (19 08 2025)
        /// Crea un nuevo propietario
        /// </summary>
        /// <param name="ownerDto">Datos del propietario</param>
        /// <returns>Propietario creado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseDto<OwnerDto>), 201)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 400)]
        public async Task<ActionResult<ApiResponseDto<OwnerDto>>> CreateOwner([FromBody] OwnerCreateDto ownerDto)
        {
            _logger.LogInformation("Creando nuevo propietario: {@Owner}", ownerDto);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseDto<object>.ErrorResponse("Datos de propietario inválidos"));
            }

            var result = await _ownerService.CreateOwnerAsync(ownerDto);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetOwner), new { id = result.Data!.Id }, result);
        }

        /// <summary>
        /// AAB (19 08 2025)
        /// Actualiza un propietario existente
        /// </summary>
        /// <param name="id">ID del propietario</param>
        /// <param name="ownerDto">Datos actualizados</param>
        /// <returns>Propietario actualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<OwnerDto>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 400)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 404)]
        public async Task<ActionResult<ApiResponseDto<OwnerDto>>> UpdateOwner(string id, [FromBody] OwnerUpdateDto ownerDto)
        {
            _logger.LogInformation("Actualizando propietario {Id}: {@Owner}", id, ownerDto);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseDto<object>.ErrorResponse("Datos de actualización inválidos"));
            }

            var result = await _ownerService.UpdateOwnerAsync(id, ownerDto);
            
            if (!result.Success)
            {
                if (result.Message.Contains("no encontrado"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// AAB (20 08 2025)
        /// Elimina un propietario
        /// </summary>
        /// <param name="id">ID del propietario</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), 404)]
        public async Task<ActionResult<ApiResponseDto<bool>>> DeleteOwner(string id)
        {
            _logger.LogInformation("Eliminando propietario: {Id}", id);
            
            var result = await _ownerService.DeleteOwnerAsync(id);
            
            if (!result.Success)
            {
                if (result.Message.Contains("no encontrado"))
                {
                    return NotFound(result);
                }
                return StatusCode(500, result);
            }

            return Ok(result);
        }
    }
}