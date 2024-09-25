using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Collections.ObjectModel;
using CodingTracker.Models;

namespace CodingTracker.ViewModels;

internal class ViewAllSessionsViewModel
{
    public ObservableCollection<ViewModels.CodingSessionViewModel> AllSessions { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectSessionCommand { get; }

    public ViewAllSessionsViewModel()
    {
        AllSessions = new ObservableCollection<ViewModels.CodingSessionViewModel>(Models.CodingSession.ViewAllSessions().Select(n => new CodingSessionViewModel(n)));
        NewCommand = new RelayCommand(NewSession);
        SelectSessionCommand = new RelayCommand<ViewModels.CodingSessionViewModel>(SelectSession);
    }

    private void NewSession()
    {
        Shell.Current.GoToAsync(nameof(Views.CodingSessionPage));
    }

    private void SelectSession(ViewModels.CodingSessionViewModel session)
    {
        if(session != null)
        {
            Shell.Current.GoToAsync($"{nameof(Views.CodingSessionPage)}?id={session.Id}");
        }
    }
}

