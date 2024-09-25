using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;

namespace CodingTracker.ViewModels;

internal class CodingSessionViewModel : ObservableObject, IQueryAttributable
{
    private Models.CodingSession codingSession;

    public string Id => codingSession.Id.ToString();
    public DateTime StartTime
    {
        get => codingSession.StartTime;
        set
        {
            if (codingSession.StartTime != value)
            {
                codingSession.StartTime = value;
                OnPropertyChanged();
            }
        }
    }
    public DateTime EndTime
    {
        get => codingSession.EndTime;
        set
        {
            if (codingSession.EndTime != value)
            {
                codingSession.EndTime = value;
                OnPropertyChanged();
            }
        }
    }
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
    public ICommand UpdateCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }

    public CodingSessionViewModel()
    {
        codingSession = new Models.CodingSession();
        UpdateCommand = new RelayCommand(Update);
        DeleteCommand = new RelayCommand(Delete);
    }

    public CodingSessionViewModel(Models.CodingSession session)
    {
        codingSession = session;
        UpdateCommand = new RelayCommand(Update);
        DeleteCommand = new RelayCommand(Delete);
    }

    private void Update()
    {
        codingSession.CalculateDuration();
        codingSession.UpdateSession(codingSession);
    }

    private void Delete()
    {
        codingSession.UpdateSession(codingSession);
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            // Parse the string to an integer
            if (int.TryParse(query["load"].ToString(), out int sessionId))
            {
                codingSession = Models.CodingSession.LoadSession(sessionId);
                RefreshProperties();
            }
            else
            {
                Debug.WriteLine("Invalid session Id provided.");
            }
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
                StartTime = updatedSession.StartTime;
                EndTime = updatedSession.EndTime;
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
        OnPropertyChanged(nameof(Duration));
    }


}


