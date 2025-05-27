using StroopApp.Models;

namespace StroopApp.Services.Window
{
    public interface IWindowManager
    {
        void ShowParticipantWindow(ExperimentSettings settings);
        void CloseParticipantWindow();
    }
}
