namespace Api.Services.Models.Api
{
    public class Response<T> : BaseResponse
    {
        /// <summary>
        /// Information about the Response object 
        /// </summary>
        public T  Result { get; set; } = default!;
        /// <summary>
        /// Error details if the request was not successful
        /// </summary>
        public ErrorDetail? errorDetail { get; set; }
        /// <summary>
        /// Indicates if the request was successful
        /// </summary>
        public int ResultCode { get; set; } = Microsoft.AspNetCore.Http.StatusCodes.Status200OK;
    }    
}
