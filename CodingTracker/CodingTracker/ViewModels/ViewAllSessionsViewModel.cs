using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CodingTracker.Models;

namespace CodingTracker.ViewModels;

internal class ViewAllSessionsViewModel : ObservableObject, IQueryAttributable
{
    public ObservableCollection<ViewModels.CodingSessionViewModel> AllSessions { get; }
    private readonly List<CodingSession> _sessions;
    public int SelectedFilterIndex { get; set; } = 0;
    public int SelectedSortIndex { get; set; } = 0; 

    public ICommand NewCommand { get; }
    public ICommand SelectSessionCommand { get; }
    public ICommand FilterAndSortCommand { get; }

    public ViewAllSessionsViewModel()
    {
        AllSessions = new ObservableCollection<ViewModels.CodingSessionViewModel>();
        _sessions = new List<CodingSession>();
        NewCommand = new RelayCommand(NewSession);
        SelectSessionCommand = new RelayCommand<ViewModels.CodingSessionViewModel>(SelectSession);
        FilterAndSortCommand = new RelayCommand(ApplyFilterAndSort);
        LoadAllSessions();
    }

    public void LoadAllSessions()
    {
        AllSessions.Clear();
        _sessions.Clear();

        var sessions = Models.CodingSession.ViewAllSessions();
        foreach (var session in sessions)
        {
            _sessions.Add(session);
            AllSessions.Add(new CodingSessionViewModel(session));
        }
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

    public void ApplyFilterAndSort()
    {
        var filteredSessions = FilterSessions(_sessions, SelectedFilterIndex, SelectedSortIndex);

        AllSessions.Clear();
        foreach (var session in filteredSessions)
        {
            AllSessions.Add(new CodingSessionViewModel(session));
        }
    }

    public static List<CodingSession> FilterSessions(List<CodingSession> sessions, int filterChoice, int sortingChoice)
    {
        DateTime currentDate = DateTime.Now;

        switch (filterChoice)
        {
            case 0: 
                sessions = sessions.Where(s => s.StartTime.Date == currentDate.Date).ToList();
                break;
            case 1: 
                sessions = sessions.Where(s => s.StartTime.Date >= currentDate.AddDays(-7)).ToList();
                break;
            case 2: 
                sessions = sessions.Where(s => s.StartTime.Date >= currentDate.AddMonths(-1)).ToList();
                break;
            case 3: 
                sessions = sessions.Where(s => s.StartTime.Date >= currentDate.AddYears(-1)).ToList();
                break;
            default:
                Debug.WriteLine("Error: Unrecognized input.");
                break;
        }

        switch (sortingChoice)
        {
            case 0: 
                sessions = sessions.OrderBy(s => s.StartTime).ToList();
                break;
            case 1: 
                sessions = sessions.OrderByDescending(s => s.StartTime).ToList();
                break;
            case 2: 
                sessions = sessions.OrderBy(s => s.Duration).ToList();
                break;
            case 3: 
                sessions = sessions.OrderByDescending(s => s.Duration).ToList();
                break;
            default:
                Debug.WriteLine("Error: Unrecognized input.");
                break;
        }

        return sessions;
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string sessionId = query["deleted"].ToString();
            CodingSessionViewModel matchedSession = AllSessions.FirstOrDefault(s => s.Id == sessionId);

            if (matchedSession != null)
                AllSessions.Remove(matchedSession);
        }
        else if (query.ContainsKey("saved"))
        {
            string sessionId = query["saved"].ToString();

            if (int.TryParse(sessionId, out int id))
            {
                CodingSessionViewModel matchedSession = AllSessions.FirstOrDefault(s => s.Id == sessionId);

                if (matchedSession != null)
                {
                    matchedSession.Reload();
                    AllSessions.Move(AllSessions.IndexOf(matchedSession), 0);
                }
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