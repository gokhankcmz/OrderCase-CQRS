namespace CommonLib.Models.ErrorModels
{
    public class BadRequest: ErrorDetails
    {
        public BadRequest(string key = null, string variable = null, string message = null)
        {
            StatusCode = 400;
            ErrorMessage = message ?? $"The {key} is not valid : {variable}";
        }
    }
}