namespace CodingTracker.Views;

public partial class ViewAllSessionsPage : ContentPage
{
	public ViewAllSessionsPage()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        sessionsCollection.SelectedItem = null;
    }
}