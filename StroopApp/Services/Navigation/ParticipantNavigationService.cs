using StroopApp.Services.Navigation.PageFactory;

namespace StroopApp.Services.Navigation
{
    /// <summary>
    /// Concrete navigation service for the Participant Window.
    /// Registered as a singleton in DI; the Participant Window calls SetFrame() on startup.
    /// SequenceManager injects this to drive participant-side page transitions.
    /// </summary>
    public class ParticipantNavigationService : PageFactory.NavigationService, IParticipantNavigationService
    {
        public ParticipantNavigationService(IPageFactory pageFactory) : base(pageFactory)
        {
        }
    }
}
