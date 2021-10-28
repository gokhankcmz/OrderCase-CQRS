using System.Collections.Generic;

namespace CommonLib.Models.ErrorModels
{
    public class ValidationError : ErrorDetails
    {
        
        public ValidationError(Dictionary<string, string[]> errors)
        {
            StatusCode = 400; 
            Details = errors; //Todo: aktifleştir. 
            ErrorMessage =  "Model was not in correct form.";
        }
    }
}