using CarCostCalculator_App.CCL.Models;
using Microsoft.Net.Http.Headers;

namespace CarCostCalculator_App.CCL.CQRS.AspNetCore.Utilities
{
    internal static class FileQueryItemExtensions
    {
        #region Internal Methods

        internal static EntityTagHeaderValue? GetEntityTag(this FileQueryItem fileResponse)
        {
            EntityTagHeaderValue? etag = null;

            if (fileResponse.LastModified.HasValue)
            {
                var etagHash = fileResponse.LastModified.Value.ToFileTime() ^ fileResponse.FileStream.Length;

                etag = new EntityTagHeaderValue($"\"{Convert.ToString(etagHash, 16)}\"");
            }

            return etag;
        }

        #endregion
    }
}
