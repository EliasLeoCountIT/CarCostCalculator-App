using CarCostCalculator_App.CCL.Models;
using Microsoft.JSInterop;

namespace CarCostCalculator_App.CCL.Components.WebAssembly.BrowserInterop
{
    /// <summary>
    /// Module for downloading files.
    /// </summary>
    /// <param name="javaScriptRuntime">Represents an instance of the JavaScript runtime.</param>
    public class FileDownloadModule(IJSRuntime javaScriptRuntime)
    {
        #region Private Members

        private readonly IJSRuntime _javaScriptRuntime = javaScriptRuntime;
        private IJSObjectReference? _fileDownloadModule;

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs the download of the file.
        /// </summary>
        public async Task DownloadFile(FileQueryItem? fileQueryItem)
        {
            if (fileQueryItem is not null)
            {
                using var streamRef = new DotNetStreamReference(stream: fileQueryItem.FileStream);

                await _fileDownloadModule!.InvokeVoidAsync("downloadFileFromStream", fileQueryItem.DownloadName, streamRef);
            }
        }

        /// <summary>
        /// Initializes the Java script runtime.
        /// </summary>
        public async Task Initialize(string javaScriptModulePath = "./_content/Swietelsky.Basement.Components.WebAssembly/js/file-download.js")
            => _fileDownloadModule = await _javaScriptRuntime.InvokeAsync<IJSObjectReference>("import", javaScriptModulePath);

        #endregion
    }
}
