using CodingTracker.ViewModels;

namespace CodingTracker.Views;

public partial class ViewAllSessionsPage : ContentPage
{
    public ViewAllSessionsPage()
	{
		InitializeComponent();
        BindingContext = new ViewAllSessionsViewModel();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (BindingContext is ViewAllSessionsViewModel viewModel)
        {
            viewModel.LoadAllSessions();
        }
        sessionsCollection.SelectedItem = null;
    }
}