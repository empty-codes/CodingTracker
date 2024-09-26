using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;

namespace CodingTracker.ViewModels;

internal class CodingSessionViewModel : ObservableObject, IQueryAttributable
{
    private Models.CodingSession codingSession;

    public string Id => codingSession.Id.ToString();
    private DateTime startDate;
    private TimeSpan startTime;
    private DateTime endDate;
    private TimeSpan endTime;

    public DateTime StartDate
    {
        get => startDate;
        set
        {
            startDate = value;
            OnPropertyChanged();
        }
    }

    public TimeSpan StartTime
    {
        get => startTime;
        set
        {
            startTime = value;
            OnPropertyChanged();
        }
    }

    public DateTime EndDate
    {
        get => endDate;
        set
        {
            endDate = value;
            OnPropertyChanged();
        }
    }

    public TimeSpan EndTime
    {
        get => endTime;
        set
        {
            endTime = value;
            OnPropertyChanged();
        }
    }

    public DateTime CombinedStartTime => StartDate.Date + StartTime;
    public DateTime CombinedEndTime => EndDate.Date + EndTime;

    public string FormattedStartTime => CombinedStartTime.ToString("yyyy-MM-dd HH:mm");
    public string FormattedEndTime => CombinedEndTime.ToString("yyyy-MM-dd HH:mm");

    public TimeSpan Duration
    {
        get => codingSession.Duration;
        set
        {
            if (codingSession.Duration != value)
            {
                codingSession.Duration = value;
                OnPropertyChanged();
            }
        }
    }
    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }

    public CodingSessionViewModel()
    {
        codingSession = new Models.CodingSession();

        StartDate = DateTime.Now.Date; 
        EndDate = DateTime.Now.Date; 

        SaveCommand = new RelayCommand(Save);
        DeleteCommand = new RelayCommand(Delete);
    }

    public CodingSessionViewModel(Models.CodingSession session)
    {
        codingSession = session;
        StartDate = codingSession.StartTime.Date;
        StartTime = codingSession.StartTime.TimeOfDay;
        EndDate = codingSession.EndTime.Date;
        EndTime = codingSession.EndTime.TimeOfDay;
        Duration = codingSession.Duration;

        SaveCommand = new RelayCommand(Save);
        DeleteCommand = new RelayCommand(Delete);
    }

    private void Save()
    {
        codingSession.StartTime = CombinedStartTime;
        codingSession.EndTime = CombinedEndTime;
        codingSession.CalculateDuration();
        codingSession.UpdateSession(codingSession);
        Shell.Current.GoToAsync($"..?saved={codingSession.Id.ToString()}");
    }

    private void Delete()
    {
        codingSession.DeleteSession(codingSession);
        Shell.Current.GoToAsync($"..?deleted={codingSession.Id.ToString()}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            if (int.TryParse(query["load"].ToString(), out int sessionId))
            {
                codingSession = Models.CodingSession.LoadSession(sessionId);
                if (codingSession != null)
                {
                    StartDate = codingSession.StartTime.Date;         
                    StartTime = codingSession.StartTime.TimeOfDay;     

                    EndDate = codingSession.EndTime.Date;               
                    EndTime = codingSession.EndTime.TimeOfDay;

                    Duration = codingSession.Duration;
                }
                RefreshProperties();
            }
        }

        else
        {
            Debug.WriteLine("Invalid session Id provided.");
        }
        
    }

    public void Reload()
    {
        // Convert the Id string to an integer
        if (int.TryParse(Id, out int sessionId))
        {
            var updatedSession = Models.CodingSession.LoadSession(sessionId);

            if (updatedSession != null)
            {
                StartDate = updatedSession.StartTime.Date;         
                StartTime = updatedSession.StartTime.TimeOfDay;     

                EndDate = updatedSession.EndTime.Date;               
                EndTime = updatedSession.EndTime.TimeOfDay;

                Duration = updatedSession.Duration;
            }
        }
        else
        {
            Debug.WriteLine("Invalid session Id for reload.");
        }
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(StartTime));
        OnPropertyChanged(nameof(EndTime));
        OnPropertyChanged(nameof(StartDate));
        OnPropertyChanged(nameof(EndDate));
        OnPropertyChanged(nameof(Duration));
    }


}


