namespace CommonLib.Models.ErrorModels
{
    public class UnAuthorized : ErrorDetails
    {
        
        public UnAuthorized(string key = null, string variable = null, string message = null)
        {
            StatusCode = 401;
            ErrorMessage = message ?? $"Unauthorized Request. {key} {variable}";
        }
    }
}