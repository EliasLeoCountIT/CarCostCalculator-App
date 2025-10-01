using CarCostCalculator_App.CCL.Models;
using Microsoft.AspNetCore.Http;

namespace CarCostCalculator_App.CCL.CQRS.AspNetCore.Utilities
{
    internal static class FormFileExtensions
    {
        #region Internal Methods

        internal static FileCommandItem MapToFileCommandItem(this IFormFile formFile)
            => new(
                formFile.OpenReadStream,
                formFile.FileName,
                formFile.ContentType,
                formFile.Length,
                formFile.ContentDisposition
            );

        #endregion
    }
}
