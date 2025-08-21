namespace RealEstate.Application.DTOs
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public static ApiResponseDto<T> SuccessResponse(T data, string message = "Operación exitosa")
        {
            return new ApiResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponseDto<T> ErrorResponse(string message)
        {
            return new ApiResponseDto<T>
            {
                Success = false,
                Message = message
            };
        }

        public static ApiResponseDto<T> PagedResponse(T data, int total, int page, int pageSize, string message = "Datos obtenidos exitosamente")
        {
            return new ApiResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Total = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}