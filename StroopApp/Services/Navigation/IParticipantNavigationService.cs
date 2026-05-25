namespace StroopApp.Services.Navigation
{
    /// <summary>
    /// Marker interface for the navigation service scoped to the Participant Window.
    /// Registered as a singleton in DI so that SequenceManager can inject and drive
    /// participant-side navigation without coupling to the window instance.
    /// </summary>
    public interface IParticipantNavigationService : INavigationService
    {
    }
}
