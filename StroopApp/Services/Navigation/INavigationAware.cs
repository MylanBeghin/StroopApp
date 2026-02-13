namespace StroopApp.Services.Navigation
{
    /// <summary>
    /// Implemented by pages that need to receive their window-specific INavigationService
    /// after construction by the DI container.
    /// Called automatically by NavigationService.NavigateTo&lt;T&gt;().
    /// </summary>
    public interface INavigationAware
    {
        INavigationService NavigationService { set; }
    }
}
