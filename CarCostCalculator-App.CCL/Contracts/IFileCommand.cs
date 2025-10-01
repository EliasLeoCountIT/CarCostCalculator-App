using CarCostCalculator_App.CCL.Models;
using System.ComponentModel;

namespace CarCostCalculator_App.CCL.Contracts
{
    /// <summary>
    /// DO NOT USE DIRECTLY! This Interface just exists to link together file commands with and without addresses
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFileCommandBase : IWebRequest
    {
        #region Public Properties

        /// <summary>
        /// Underlying file item of the command.
        /// </summary>
        FileCommandItem FileItem { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Assign the file item of the command.
        /// </summary>
        /// <param name="fileItem"><see cref="FileCommandItem"/></param>
        void ApplyFileItem(FileCommandItem fileItem);

        #endregion
    }

    /// <summary>
    /// Marker interface to represent a file command with a void response.
    /// </summary>
    public interface IFileCommand : IFileCommandBase, ICommand;

    /// <summary>
    /// Marker interface to represent a file command with a response.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public interface IFileCommand<out TResponse> : IFileCommandBase, ICommand<TResponse>;
}
