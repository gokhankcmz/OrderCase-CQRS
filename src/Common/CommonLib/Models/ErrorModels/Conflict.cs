namespace CommonLib.Models.ErrorModels
{
    public class Conflict : ErrorDetails
    {
        public Conflict(string key=null, string variable = null, string message = null)
        {
            StatusCode = 409;
            ErrorMessage = message ?? $"{key} Request causes a conflict. {variable}";

        }
        
    }
}