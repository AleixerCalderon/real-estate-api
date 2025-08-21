using RealEstate.Application.DTOs;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<ApiResponseDto<IEnumerable<PropertyDto>>> GetAllPropertiesAsync();
        Task<ApiResponseDto<PropertyDto>> GetPropertyByIdAsync(string id);
        Task<ApiResponseDto<IEnumerable<PropertyDto>>> GetFilteredPropertiesAsync(PropertyFilterDto filter);
        Task<ApiResponseDto<PropertyDto>> CreatePropertyAsync(PropertyCreateDto propertyDto);
        Task<ApiResponseDto<PropertyDto>> UpdatePropertyAsync(string id, PropertyUpdateDto propertyDto);
        Task<ApiResponseDto<bool>> DeletePropertyAsync(string id);
    }
}