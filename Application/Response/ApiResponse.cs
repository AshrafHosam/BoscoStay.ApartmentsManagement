using Application.Contracts.Helpers;
using FluentValidation.Results;
using System.Net;

namespace Application.Response
{
    public class ApiResponse<T> : BaseResponse<T>, ICacheableResponse
    {
        public int StatusCode
        {
            get
            {
                return (int)HttpStatusCode;
            }
        }
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;
        public bool IsSuccessStatusCode
        {
            get
            {
                return (StatusCode >= 200) && (StatusCode <= 299);
            }
        }

        public bool IsCacheable { get; set; } = false;

        public static ApiResponse<T> GetApiResponseFromInvalidValidationResult(ValidationResult validationResult)
        {
            return new ApiResponse<T>()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(a => a.ErrorMessage).ToList()
            };
        }

        public static ApiResponse<T> GetNotFoundApiResponse(List<string> errors = null)
        {
            if (errors is null)
                return new ApiResponse<T>()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                };

            return new ApiResponse<T>()
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                Errors = errors.Any() ? errors : new List<string>()
            };
        }

        public static ApiResponse<T> GetNotFoundApiResponse(string error)
        {
            return new ApiResponse<T>()
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                Errors = new List<string> { error }
            };
        }

        public static ApiResponse<T> GetBadRequestApiResponse(List<string> errors = null)
        {
            if (errors is null)
                return new ApiResponse<T>()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };

            return new ApiResponse<T>()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Errors = errors.Any() ? errors : new List<string>()
            };
        }

        public static ApiResponse<T> GetBadRequestApiResponse(string error)
        {
            return new ApiResponse<T>()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Errors = new List<string> { error }
            };
        }

        public static ApiResponse<T> GetNoContentApiResponse()
        {
            return new ApiResponse<T>
            {
                HttpStatusCode = HttpStatusCode.NoContent
            };
        }

        public static ApiResponse<T> GetSuccessApiResponse(T data)
        {
            return new ApiResponse<T>
            {
                Data = data
            };
        }

        public static ApiResponse<T> GetSuccessCacheableApiResponse(T data)
        {
            return new ApiResponse<T>
            {
                Data = data,
                IsCacheable = true
            };
        }

        public static ApiResponse<T> GetConflictApiResponse(T data)
        {
            return new ApiResponse<T>
            {
                Data = data,
                HttpStatusCode = HttpStatusCode.Conflict
            };
        }

        public static ApiResponse<T> GetAcceptedApiResponse()
        {
            return new ApiResponse<T>
            {
                HttpStatusCode = HttpStatusCode.Accepted
            };
        }

        public static ApiResponse<T> GetCreatedApiResponse()
        {
            return new ApiResponse<T>
            {
                HttpStatusCode = HttpStatusCode.Created
            };
        }
    }
}
