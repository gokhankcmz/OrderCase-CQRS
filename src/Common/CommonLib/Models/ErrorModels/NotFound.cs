using System;

namespace CommonLib.Models.ErrorModels
{
    public class NotFound : ErrorDetails
    {
        public NotFound(string key=null, string variable = null, string message = null)
        {
            StatusCode = 404; 
            ErrorMessage = message ?? $"Request {key} not found. {variable}";
        }
        
    }
}