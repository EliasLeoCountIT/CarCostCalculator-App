using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CarCostCalculator_App.CCL.Components.WebAssembly.Helper
{
    /// <summary>
    /// Helper methods for <see cref="IWebAssemblyHostEnvironment"/>.
    /// </summary>
    public class WebAssemblyHostEnvironmentHelper
    {
        #region Public Constructors

        /// <summary>
        /// Constructor to initialize the <see cref="WebAssemblyHostEnvironmentHelper"/> class.
        /// </summary>
        public WebAssemblyHostEnvironmentHelper(IWebAssemblyHostEnvironment environment)
        {
            SetEnvironmentTextAndStyle(environment);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Provides information about the hosting environment style class.
        /// </summary>
        public string? EnvironmentStyle { get; private set; }

        /// <summary>
        /// Provides information about the hosting environment text.
        /// </summary>
        public string? EnvironmentText { get; private set; }

        #endregion

        #region Private Methods

        private void SetEnvironmentTextAndStyle(IWebAssemblyHostEnvironment environment)
        {
            switch (environment.BaseAddress)
            {
                case string debug when debug.Contains("localhost"):
                    EnvironmentText = "Debug";
                    EnvironmentStyle = "debug-environment";
                    break;

                case string ci when ci.Contains("dev.swietelsky.com"):
                    var splitUrl = ci.Split('-', '.');
                    EnvironmentStyle = "ci-environment";

                    if (splitUrl.Length > 0)
                    {
                        EnvironmentText = string.Concat("CI ", splitUrl[1][..1].ToUpper(), splitUrl[1].AsSpan(1));
                        return;
                    }

                    EnvironmentText = "CI";
                    break;

                case string staging when staging.Contains("staging.swietelsky.com"):
                    EnvironmentText = "Staging";
                    EnvironmentStyle = "staging-environment";
                    break;

                default:
                    //do nothing
                    return;
            }
        }

        #endregion
    }
}
