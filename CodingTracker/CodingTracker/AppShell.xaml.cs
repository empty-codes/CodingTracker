namespace CodingTracker;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(Views.CodingSessionPage), typeof(Views.CodingSessionPage));
        Routing.RegisterRoute(nameof(Views.CodingGoalsPage), typeof(Views.CodingGoalsPage));
    }
}