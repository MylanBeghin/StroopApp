using StroopApp.Models;

namespace StroopApp.Services.Window
{
    /// <summary>
    /// Defines contract for managing application windows.
    /// </summary>
    public interface IWindowManager
    {
        /// <summary>
        /// Shows or activates the participant management window.
        /// </summary>
        void ShowParticipantWindow(ExperimentSettings settings);

        /// <summary>
        /// Closes the participant management window.
        /// </summary>
        void CloseParticipantWindow();
    }
}
