using CarCostCalculator_App.CCL.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace CarCostCalculator_App.CCL.Components.WebAssembly
{
    /// <summary>
    /// Extension methods for <see cref="IBrowserFile"/>.
    /// </summary>
    public static class BrowserFileExtensions
    {
        #region Public Methods

        /// <summary>
        /// Maps the <see cref="IBrowserFile"/> to a <see cref="FileCommandItem"/>.
        /// </summary>
        /// <param name="formFile">The <see cref="IBrowserFile"/> to map.</param>
        /// <returns>A new <see cref="FileCommandItem"/></returns>
        public static FileCommandItem MapToFileCommandItem(this IBrowserFile formFile)
            => new(
                () => formFile.OpenReadStream(),
                formFile.Name,
                formFile.ContentType,
                formFile.Size
            );

        #endregion
    }
}
