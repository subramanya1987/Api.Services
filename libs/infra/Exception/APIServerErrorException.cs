namespace Api.Services.Infra.Exception
{
    public class APIServerErrorException: APIHttpException
    {
        public APIServerErrorException() : this(string.Empty) { }
        public APIServerErrorException(string message) : base(message)
        {
            StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }       
    }    
}
