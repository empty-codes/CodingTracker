using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;

namespace CodingTracker.ViewModels
{
    internal class RealTimeTrackerViewModel
    {
        private Models.CodingSession codingSession;

        public Stopwatch StopWatch { get; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsRunning { get; set; }

        public RealTimeTrackerViewModel()
        {
            StopWatch = new Stopwatch();
            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
        }

        public ICommand StartCommand { get; private set; }
        public ICommand StopCommand { get; private set; }

        public void Start()
        {
            if (IsRunning == false)
            {
                StartTime = DateTime.Now;
                StopWatch.Start();
                IsRunning = true;
                Debug.WriteLine("Stopwatch started.");
                //AnsiConsole.MarkupLine("[green]The stopwatch has started counting![/]");
            }
            else
            {
                Debug.WriteLine("Stopwatch is already running.");
                //AnsiConsole.MarkupLine("[yellow]The stopwatch is already running[/]");
            }
        }

        public void Stop()
        {
            if (IsRunning == true)
            {
                EndTime = DateTime.Now;
                StopWatch.Stop();
                IsRunning = false;
                Debug.WriteLine("Stopwatch stopped.");
                //AnsiConsole.MarkupLine("\n[green]The stopwatch has stopped![/]");
                CalculateDuration();
                Save();
            }
            else
            {
                Debug.WriteLine("Stopwatch is already stopped.");
                //AnsiConsole.MarkupLine("\n[yellow]The stopwatch has already ended[/]");
            }
        }

        public void Save()
        {
            codingSession = new Models.CodingSession();
            codingSession.StartTime = StartTime;
            codingSession.EndTime = EndTime;
            codingSession.Duration = Duration;
            codingSession.InsertSession(codingSession);

            Debug.WriteLine("Insert Session method succesfully called.");
        }

        public void CalculateDuration()
        {
            if (IsRunning == true)
            {
                Debug.WriteLine("Stop the stopwatch first!.");
                //AnsiConsole.MarkupLine("[red]Error: Stop the stopwatch first![/]");
            }
            else
            {
                Duration = EndTime - StartTime;
            }
        }

    }
}
