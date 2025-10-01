namespace CarCostCalculator_App.CCL.Models
{
    /// <summary>
    /// Represents a file response to a query.
    /// </summary>
    /// <param name="FileStream">The <see cref="Stream"/> with the contents of the file.</param>
    /// <param name="MimeType">The MIME-Type of the file.</param>
    /// <param name="DownloadName">The file name to be used to download the file.</param>
    /// <param name="LastModified">The <see cref="DateTimeOffset"/> of when the file was last modified.</param>
    public record FileQueryItem(Stream FileStream, string MimeType, string? DownloadName = null, DateTimeOffset? LastModified = null)
    {
        /// <summary>
        /// Reads all bytes from the <see cref="FileStream"/>.
        /// </summary>
        /// <returns>All bytes from the <see cref="FileStream"/>.</returns>
        public async Task<byte[]> ReadAllBytes()
        {
            if (FileStream is MemoryStream fileMemoryStream)
            {
                return fileMemoryStream.ToArray();
            }
            else
            {
                using (var tempMemoryStream = new MemoryStream())
                {
                    await FileStream.CopyToAsync(tempMemoryStream);

                    return tempMemoryStream.ToArray();
                }
            }
        }
    }
}
