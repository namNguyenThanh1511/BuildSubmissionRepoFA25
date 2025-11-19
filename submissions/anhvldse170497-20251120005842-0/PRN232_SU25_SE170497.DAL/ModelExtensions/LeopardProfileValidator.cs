using PRN232_SU25_SE170497.DAL.DTOs;
using System.Text.RegularExpressions;

namespace PRN232_SU25_SE170497.DAL.ModelExtensions
{
    public static class LeopardProfileValidator
    {
        private static readonly Regex LeopardNameRegex = new("^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$");

        public static Result<string> Validate(LeopardProfileDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.LeopardName))
                return Errors.ValidationError<string>("LeopardProfile name is required.");

                   if (!LeopardNameRegex.IsMatch(dto.LeopardName))
                        return Errors.ValidationError<string>("LeopardProfile name format is invalid.");


                    if (dto.Weight < 15)
                       return Errors.ValidationError<string>("Weight must be  greater.");

           

                   return Result<string>.Ok();
          

        }
    }
}
