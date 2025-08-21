using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IOwnerRepository _ownerRepository;

        public PropertyService(IPropertyRepository propertyRepository, IOwnerRepository ownerRepository)
        {
            _propertyRepository = propertyRepository;
            _ownerRepository = ownerRepository;
        }

        public async Task<ApiResponseDto<IEnumerable<PropertyDto>>> GetAllPropertiesAsync()
        {
            try
            {
                var properties = await _propertyRepository.GetAllAsync();
                var propertyDtos = new List<PropertyDto>();

                foreach (var property in properties)
                {
                    var owner = await _ownerRepository.GetByIdAsync(property.IdOwner);
                    propertyDtos.Add(MapToDto(property, owner));
                }

                return ApiResponseDto<IEnumerable<PropertyDto>>.SuccessResponse(propertyDtos, "Propiedades obtenidas exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<PropertyDto>>.ErrorResponse($"Error al obtener las propiedades: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PropertyDto>> GetPropertyByIdAsync(string id)
        {
            try
            {
                var property = await _propertyRepository.GetByIdAsync(id);
                if (property == null)
                {
                    return ApiResponseDto<PropertyDto>.ErrorResponse("Propiedad no encontrada");
                }

                var owner = await _ownerRepository.GetByIdAsync(property.IdOwner);
                var propertyDto = MapToDto(property, owner);

                return ApiResponseDto<PropertyDto>.SuccessResponse(propertyDto, "Propiedad obtenida exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PropertyDto>.ErrorResponse($"Error al obtener la propiedad: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<PropertyDto>>> GetFilteredPropertiesAsync(PropertyFilterDto filter)
        {
            try
            {
                var (properties, total) = await _propertyRepository.GetFilteredAsync(filter);
                var propertyDtos = new List<PropertyDto>();

                foreach (var property in properties)
                {
                    var owner = await _ownerRepository.GetByIdAsync(property.IdOwner);
                    propertyDtos.Add(MapToDto(property, owner));
                }

                return ApiResponseDto<IEnumerable<PropertyDto>>.PagedResponse(
                    propertyDtos, 
                    total, 
                    filter.Page, 
                    filter.PageSize, 
                    "Propiedades filtradas obtenidas exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<PropertyDto>>.ErrorResponse($"Error al filtrar las propiedades: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PropertyDto>> CreatePropertyAsync(PropertyCreateDto propertyDto)
        {
            try
            {
                // Verificar que el propietario existe AAB (20 08 2025)
                var ownerExists = await _ownerRepository.ExistsAsync(propertyDto.IdOwner);
                if (!ownerExists)
                {
                    return ApiResponseDto<PropertyDto>.ErrorResponse("El propietario especificado no existe");
                }

                var property = new Property
                {
                    Name = propertyDto.Name,
                    Address = propertyDto.Address,
                    Price = propertyDto.Price,
                    IdOwner = propertyDto.IdOwner,
                    Image = propertyDto.Image,
                    Year = propertyDto.Year,
                    CodeInternal = GenerateCodeInternal()
                };

                var createdProperty = await _propertyRepository.CreateAsync(property);
                var owner = await _ownerRepository.GetByIdAsync(createdProperty.IdOwner);
                var responseDto = MapToDto(createdProperty, owner);

                return ApiResponseDto<PropertyDto>.SuccessResponse(responseDto, "Propiedad creada exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PropertyDto>.ErrorResponse($"Error al crear la propiedad: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PropertyDto>> UpdatePropertyAsync(string id, PropertyUpdateDto propertyDto)
        {
            try
            {
                var existingProperty = await _propertyRepository.GetByIdAsync(id);
                if (existingProperty == null)
                {
                    return ApiResponseDto<PropertyDto>.ErrorResponse("Propiedad no encontrada");
                }
                
                if (!string.IsNullOrEmpty(propertyDto.Name))
                    existingProperty.Name = propertyDto.Name;
                if (!string.IsNullOrEmpty(propertyDto.Address))
                    existingProperty.Address = propertyDto.Address;
                if (propertyDto.Price.HasValue)
                    existingProperty.Price = propertyDto.Price.Value;
                if (!string.IsNullOrEmpty(propertyDto.Image))
                    existingProperty.Image = propertyDto.Image;
                if (propertyDto.Year.HasValue)
                    existingProperty.Year = propertyDto.Year.Value;

                var updatedProperty = await _propertyRepository.UpdateAsync(id, existingProperty);
                if (updatedProperty == null)
                {
                    return ApiResponseDto<PropertyDto>.ErrorResponse("Error al actualizar la propiedad");
                }

                var owner = await _ownerRepository.GetByIdAsync(updatedProperty.IdOwner);
                var responseDto = MapToDto(updatedProperty, owner);

                return ApiResponseDto<PropertyDto>.SuccessResponse(responseDto, "Propiedad actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PropertyDto>.ErrorResponse($"Error al actualizar la propiedad: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> DeletePropertyAsync(string id)
        {
            try
            {
                var exists = await _propertyRepository.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponseDto<bool>.ErrorResponse("Propiedad no encontrada");
                }

                var deleted = await _propertyRepository.DeleteAsync(id);
                if (!deleted)
                {
                    return ApiResponseDto<bool>.ErrorResponse("Error al eliminar la propiedad");
                }

                return ApiResponseDto<bool>.SuccessResponse(true, "Propiedad eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error al eliminar la propiedad: {ex.Message}");
            }
        }

        private PropertyDto MapToDto(Property property, Owner? owner)
        {
            return new PropertyDto
            {
                Id = property.Id,
                IdOwner = property.IdOwner,
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                Image = property.Image,
                Year = property.Year,
                CodeInternal = property.CodeInternal,
                OwnerName = owner?.Name ?? "Propietario no encontrado"
            };
        }

        private int GenerateCodeInternal()
        {
            return new Random().Next(100000, 999999);
        }
    }
}