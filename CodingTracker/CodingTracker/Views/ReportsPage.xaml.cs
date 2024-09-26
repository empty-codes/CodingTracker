using CodingTracker.ViewModels;

namespace CodingTracker.Views;

public partial class ReportsPage : ContentPage
{
	public ReportsPage()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        var report = new ReportsViewModel();
        var sessions = Models.CodingSession.ViewAllSessions();
        report.GenerateReport(sessions);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var viewModel = BindingContext as ReportsViewModel;
        viewModel?.RefreshReport();
    }

}