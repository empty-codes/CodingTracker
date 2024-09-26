using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using CodingTracker.Models;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace CodingTracker.ViewModels
{
    internal class CodingGoalsViewModel : ObservableObject
    {
        private readonly List<CodingSession> _sessions;

        private int goalHours;
        public int GoalHours
        {
            get => goalHours;
            set
            {
                goalHours = value;
                OnPropertyChanged(nameof(GoalHours)); // Notify the UI about the change
            }
        }

        private DateTime goalDeadline;
        public DateTime GoalDeadline
        {
            get => goalDeadline;
            set
            {
                goalDeadline = value;
                OnPropertyChanged(nameof(GoalDeadline));
            }
        } 

        private double currentHours;
        public double CurrentHours
        {
            get => currentHours;
            set
            {
                currentHours = value;
                OnPropertyChanged(nameof(CurrentHours));
            }
        }

        private double dailyTarget;
        public double DailyTarget
        {
            get => dailyTarget;
            set
            {
                dailyTarget = value;
                OnPropertyChanged(nameof(DailyTarget));
            }
        }

        private string goalStatus;
        public string GoalStatus
        {
            get => goalStatus;
            set
            {
                goalStatus = value;
                OnPropertyChanged(nameof(GoalStatus));
            }
        }

        public CodingGoalsViewModel()
        {
            _sessions = Models.CodingSession.ViewAllSessions(); // Load sessions
            CurrentHours = CalculateCurrentHours(_sessions);

            GoalDeadline = DateTime.Now.Date;

            SetGoalCommand = new RelayCommand(SetGoal);
        }

        public ICommand SetGoalCommand { get; private set; }

        public  void SetGoal()
        {
            int daysLeft = (GoalDeadline - DateTime.Now).Days;

            if (daysLeft <= 0)
            {
                GoalStatus = "The deadline is today or has passed. You can't set this goal.";
                return;
            }

            DailyTarget = CalculateDailyTarget(daysLeft, goalHours);
            UpdateGoalStatus(goalHours);
        }

        private void UpdateGoalStatus(int goalHours)
        {
            if (CurrentHours >= goalHours)
            {
                GoalStatus = $"Congratulations! You have reached your goal of {goalHours} hours!";
            }
            else
            {
                GoalStatus = $"You need to code {Math.Round(DailyTarget, 2)} hours per day to reach your goal of {goalHours} hours.";
            }
        }

        private double CalculateCurrentHours(List<CodingSession> sessions)
        {
            return sessions.Sum(s => s.Duration.TotalHours);
        }

        private double CalculateDailyTarget(int daysLeft, int goalHours)
        {
            double hoursLeft = goalHours - CurrentHours;
            return hoursLeft / daysLeft;
        }
    }
}
