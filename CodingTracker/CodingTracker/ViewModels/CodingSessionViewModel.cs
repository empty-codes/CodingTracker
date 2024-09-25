using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace CodingTracker.ViewModels;

internal class CodingSessionViewModel : ObservableObject
{
    private Models.CodingSession codingSession;

    public int Id => codingSession.Id;
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
}

