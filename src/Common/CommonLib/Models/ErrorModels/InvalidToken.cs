namespace CommonLib.Models.ErrorModels
{
    public class InvalidToken : ErrorDetails
    {
        public InvalidToken(string key = null, string variable = null, string message = null)
        {
            StatusCode = 401;
            ErrorMessage = message ?? $"The Token {key} is not valid {variable}";
        }
        
    }
}