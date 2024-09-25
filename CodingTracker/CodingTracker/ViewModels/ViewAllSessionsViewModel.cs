using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Collections.ObjectModel;
using CodingTracker.Models;
using System.Diagnostics;

namespace CodingTracker.ViewModels;

internal class ViewAllSessionsViewModel : IQueryAttributable
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
            Shell.Current.GoToAsync($"{nameof(Views.CodingSessionPage)}?load={session.Id}");
        }
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string sessionId = query["deleted"].ToString();
            CodingSessionViewModel matchedSession = AllSessions.FirstOrDefault(s => s.Id == sessionId);

            // If session exists, delete it
            if (matchedSession != null)
                AllSessions.Remove(matchedSession);
        }
        else if (query.ContainsKey("saved"))
        {
            string sessionId = query["saved"].ToString();

            // Convert sessionId to an integer
            if (int.TryParse(sessionId, out int id))
            {
                CodingSessionViewModel matchedSession = AllSessions.FirstOrDefault(s => s.Id == sessionId);

                // If session is found, update it
                if (matchedSession != null)
                {
                    matchedSession.Reload();
                    AllSessions.Move(AllSessions.IndexOf(matchedSession), 0);
                }
                // If session isn't found, it's new; add it.
                else
                {
                    AllSessions.Insert(0, new CodingSessionViewModel(Models.CodingSession.LoadSession(id)));
                }
            }
            else
            {
                Debug.WriteLine("Invalid session ID format.");
            }
        }
    }

}

