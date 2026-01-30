using StroopApp.Models;
using StroopApp.Views;

namespace StroopApp.Services.Window
{
    /// <summary>
    /// Service for managing application windows (creation, activation, cleanup).
    /// Ensures single instance of participant management window.
    /// </summary>
    public class WindowManager : IWindowManager
    {
        private ParticipantWindow? _participantWindow;

        /// <summary>
        /// Shows or activates the participant management window.
        /// Creates a new instance if none exists, otherwise resets and activates the existing one.
        /// </summary>
        public void ShowParticipantWindow(ExperimentSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

            if (_participantWindow == null)
            {
                _participantWindow = new ParticipantWindow(settings);
                _participantWindow.Closed += (_, _) => _participantWindow = null;
                _participantWindow.Show();
            }
            else
            {
                _participantWindow.Reset();
                _participantWindow.Activate();

            }
        }

        /// <summary>
        /// Closes the participant management window if open.
        /// </summary>
        public void CloseParticipantWindow()
        {
            _participantWindow?.Close();
            _participantWindow = null;
        }
    }
}
