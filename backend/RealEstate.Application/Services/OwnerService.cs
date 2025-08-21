using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;

        public OwnerService(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public async Task<ApiResponseDto<IEnumerable<OwnerDto>>> GetAllOwnersAsync()
        {
            try
            {
                var owners = await _ownerRepository.GetAllAsync();
                var ownerDtos = owners.Select(MapToDto);

                return ApiResponseDto<IEnumerable<OwnerDto>>.SuccessResponse(ownerDtos, "Propietarios obtenidos exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<OwnerDto>>.ErrorResponse($"Error al obtener los propietarios: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<OwnerDto>> GetOwnerByIdAsync(string id)
        {
            try
            {
                var owner = await _ownerRepository.GetByIdAsync(id);
                if (owner == null)
                {
                    return ApiResponseDto<OwnerDto>.ErrorResponse("Propietario no encontrado");
                }

                var ownerDto = MapToDto(owner);
                return ApiResponseDto<OwnerDto>.SuccessResponse(ownerDto, "Propietario obtenido exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<OwnerDto>.ErrorResponse($"Error al obtener el propietario: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<OwnerDto>> CreateOwnerAsync(OwnerCreateDto ownerDto)
        {
            try
            {
                var owner = new Owner
                {
                    Name = ownerDto.Name,
                    Address = ownerDto.Address,
                    Phone = ownerDto.Phone,
                    Birthday = ownerDto.Birthday
                };

                var createdOwner = await _ownerRepository.CreateAsync(owner);
                var responseDto = MapToDto(createdOwner);

                return ApiResponseDto<OwnerDto>.SuccessResponse(responseDto, "Propietario creado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<OwnerDto>.ErrorResponse($"Error al crear el propietario: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<OwnerDto>> UpdateOwnerAsync(string id, OwnerUpdateDto ownerDto)
        {
            try
            {
                var existingOwner = await _ownerRepository.GetByIdAsync(id);
                if (existingOwner == null)
                {
                    return ApiResponseDto<OwnerDto>.ErrorResponse("Propietario no encontrado");
                }

                if (!string.IsNullOrEmpty(ownerDto.Name))
                    existingOwner.Name = ownerDto.Name;
                if (!string.IsNullOrEmpty(ownerDto.Address))
                    existingOwner.Address = ownerDto.Address;
                if (!string.IsNullOrEmpty(ownerDto.Phone))
                    existingOwner.Phone = ownerDto.Phone;
                if (ownerDto.Birthday.HasValue)
                    existingOwner.Birthday = ownerDto.Birthday.Value;

                var updatedOwner = await _ownerRepository.UpdateAsync(id, existingOwner);
                if (updatedOwner == null)
                {
                    return ApiResponseDto<OwnerDto>.ErrorResponse("Error al actualizar el propietario");
                }

                var responseDto = MapToDto(updatedOwner);
                return ApiResponseDto<OwnerDto>.SuccessResponse(responseDto, "Propietario actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<OwnerDto>.ErrorResponse($"Error al actualizar el propietario: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> DeleteOwnerAsync(string id)
        {
            try
            {
                var exists = await _ownerRepository.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponseDto<bool>.ErrorResponse("Propietario no encontrado");
                }

                var deleted = await _ownerRepository.DeleteAsync(id);
                if (!deleted)
                {
                    return ApiResponseDto<bool>.ErrorResponse("Error al eliminar el propietario");
                }

                return ApiResponseDto<bool>.SuccessResponse(true, "Propietario eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error al eliminar el propietario: {ex.Message}");
            }
        }

        private OwnerDto MapToDto(Owner owner)
        {
            return new OwnerDto
            {
                Id = owner.Id,
                Name = owner.Name,
                Address = owner.Address,
                Phone = owner.Phone,
                Birthday = owner.Birthday
            };
        }
    }
}