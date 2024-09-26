using CommunityToolkit.Mvvm.ComponentModel;
using CodingTracker.Models;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace CodingTracker.ViewModels;

internal class ReportsViewModel : ObservableObject
{
    public ObservableCollection<ViewModels.CodingSessionViewModel>? AllSessions { get; }
    public TimeSpan TotalDuration { get; set; }
    public string? AverageDuration { get; set; }
    public int NumberOfSessions { get; set; }
    public string? LongestSession { get; set; }
    public string? ShortestSession { get; set; }

    public ReportsViewModel()
    {
        var sessions = Models.CodingSession.ViewAllSessions();
        GenerateReport(sessions);
    }

    public void RefreshReport()
    {
        var sessions = Models.CodingSession.ViewAllSessions();
        GenerateReport(sessions);
        RefreshProperties();
    }

    public void GenerateReport(List<CodingSession> sessions)
    {
        if (sessions.Count == 0)
        {
            Debug.WriteLine("No sessions found.");
            return;
        }

        TotalDuration = sessions.Aggregate(TimeSpan.Zero, (sum, session) => sum.Add(session.Duration));
        AverageDuration = (TimeSpan.FromTicks(TotalDuration.Ticks / sessions.Count)).ToString(@"hh\:mm");

        var longestSessionObj = sessions.OrderByDescending(s => s.Duration).First();
        var shortestSessionObj = sessions.OrderBy(s => s.Duration).First();

        LongestSession = $"{longestSessionObj.Duration} on {longestSessionObj.StartTime.ToString("yyyy-MM-dd")}";
        ShortestSession = $"{shortestSessionObj.Duration} on {shortestSessionObj.StartTime.ToString("yyyy-MM-dd")}";

        NumberOfSessions = sessions.Count;
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(TotalDuration));
        OnPropertyChanged(nameof(AverageDuration));
        OnPropertyChanged(nameof(NumberOfSessions));
        OnPropertyChanged(nameof(LongestSession));
        OnPropertyChanged(nameof(ShortestSession));
    }
}