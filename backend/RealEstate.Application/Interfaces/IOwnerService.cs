using RealEstate.Application.DTOs;

namespace RealEstate.Application.Interfaces
{
     public interface IOwnerService
    {
        Task<ApiResponseDto<IEnumerable<OwnerDto>>> GetAllOwnersAsync();
        Task<ApiResponseDto<OwnerDto>> GetOwnerByIdAsync(string id);
        Task<ApiResponseDto<OwnerDto>> CreateOwnerAsync(OwnerCreateDto ownerDto);
        Task<ApiResponseDto<OwnerDto>> UpdateOwnerAsync(string id, OwnerUpdateDto ownerDto);
        Task<ApiResponseDto<bool>> DeleteOwnerAsync(string id);
    }

}