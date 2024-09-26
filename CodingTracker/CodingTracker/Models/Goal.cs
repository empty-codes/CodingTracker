using Microsoft.Data.Sqlite;
using System.Diagnostics;
using Dapper;

namespace CodingTracker.Models
{
    internal class Goal
    {
        public int Id { get; set; }
        public int GoalHours { get; set; }
        public DateTime GoalDeadline { get; set; }
        public double CurrentHours { get; set; }
        public double DailyTarget { get; set; }
        public string GoalStatus { get; set; }

        public void SaveGoal()
        {
            using var conn = new SqliteConnection(App.ConnectionString);
            string insertQuery = "INSERT INTO Goals(GoalHours, GoalDeadline, CurrentHours, DailyTarget, GoalStatus) VALUES(@GoalHours, @GoalDeadline, @CurrentHours, @DailyTarget, @GoalStatus)";
            try
            {
                var parameters = new
                {
                    GoalHours,
                    GoalDeadline = GoalDeadline.ToString(App.DateFormat),
                    CurrentHours,
                    DailyTarget,
                    GoalStatus
                };
                conn.Execute(insertQuery, parameters);
                Debug.WriteLine($"Goal successfully saved.");
            }
            catch (SqliteException e)
            {
                Debug.WriteLine($"Error occurred while trying to save the goal\n - Details: {e.Message}");
            }
        }

        public static List<Goal> LoadGoals()
        {
            var goals = new List<Goal>();
            using var conn = new SqliteConnection(App.ConnectionString);
            string readQuery = "SELECT * FROM Goals";
            try
            {
                conn.Open();
                var rawGoals = conn.Query(readQuery).ToList();
                foreach (var rawGoal in rawGoals)
                {
                    goals.Add(new Goal
                    {
                        Id = (int)rawGoal.Id,
                        GoalHours = (int)rawGoal.GoalHours,
                        GoalDeadline = DateTime.ParseExact(rawGoal.GoalDeadline, App.DateFormat, null),
                        CurrentHours = (double)rawGoal.CurrentHours,
                        DailyTarget = (double)rawGoal.DailyTarget,
                        GoalStatus = (string)rawGoal.GoalStatus
                    });
                }
            }
            catch (SqliteException e)
            {
                Debug.WriteLine($"Error occurred while trying to access your goals\n - Details: {e.Message}");
                throw;
            }
            return goals;
        }
    }
}
