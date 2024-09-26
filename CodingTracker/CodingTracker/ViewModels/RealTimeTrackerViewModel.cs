using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;

namespace CodingTracker.ViewModels;

internal class RealTimeTrackerViewModel : ObservableObject
{
    private Models.CodingSession? codingSession;
    public Stopwatch StopWatch { get; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; set; }
    public bool IsRunning { get; set; }
    private string? status;
    public string? StopwatchStatus
    {
        get => status;
        set
        {
            if (status != value)
            {
                status = value;
                OnPropertyChanged();
            }
        }
    }

    public RealTimeTrackerViewModel()
    {
        StopWatch = new Stopwatch();
        StartCommand = new RelayCommand(Start);
        StopCommand = new RelayCommand(Stop);
        StopwatchStatus = "Stopwatch is not running.";
    }

    public ICommand StartCommand { get; private set; }
    public ICommand StopCommand { get; private set; }

    private void Start()
    {
        if (IsRunning == false)
        {
            StartTime = DateTime.Now;
            StopWatch.Start();
            IsRunning = true;
            Debug.WriteLine("Stopwatch started.");
            StopwatchStatus = "Stopwatch is running.";
        }
        else
        {
            StopwatchStatus = "Stopwatch is already running.";
            Debug.WriteLine("Stopwatch is already running.");
        }
    }

    private void Stop()
    {
        if (IsRunning == true)
        {
            EndTime = DateTime.Now;
            StopWatch.Stop();
            IsRunning = false;
            Debug.WriteLine("Stopwatch stopped.");
            StopwatchStatus = "Stopwatch is not running.";
            CalculateDuration();
            Save();
        }
        else
        {
            StopwatchStatus = "Stopwatch is already stopped.";
            Debug.WriteLine("Stopwatch is already stopped.");
        }
    }

    public void Save()
    {
        codingSession = new Models.CodingSession();
        codingSession.StartTime = StartTime;
        codingSession.EndTime = EndTime;
        codingSession.Duration = Duration;
        codingSession.InsertSession(codingSession);

        Shell.Current.GoToAsync($"..?saved={codingSession.Id.ToString()}");
    }

    public void CalculateDuration()
    {
        if (IsRunning == true)
        {
            Debug.WriteLine("Stop the stopwatch first!.");
        }
        else
        {
            Duration = EndTime - StartTime;
        }
    }
}