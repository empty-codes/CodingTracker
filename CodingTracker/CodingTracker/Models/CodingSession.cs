using Microsoft.Data.Sqlite;
using Dapper;
using System.Diagnostics;

namespace CodingTracker.Models;

    internal class CodingSession
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }

        public void CalculateDuration()
        {
            if (EndTime > StartTime)
            {
                Duration = EndTime - StartTime;
            }
            else
            {
                throw new Exception("EndTime must be after StartTime.");
            }
        }

        public void InsertSession(CodingSession session)
        {
            using var conn = new SqliteConnection(App.ConnectionString);
            string insertQuery = "INSERT INTO CodingSessions(StartTime, EndTime, Duration) VALUES(@StartTime, @EndTime, @Duration)";
            try
            {
                var parameters = new
                {
                    StartTime = session.StartTime.ToString(App.DateFormat),
                    EndTime = session.EndTime.ToString(App.DateFormat),
                    Duration = session.Duration.ToString(@"hh\:mm\:ss")
                };
                conn.Execute(insertQuery, parameters);

                string getIdQuery = "SELECT last_insert_rowid();";
                session.Id = conn.ExecuteScalar<int>(getIdQuery);

                Debug.WriteLine($"Session successfully added. (Session Id: {session.Id})");
            }
            catch (SqliteException e)
            {
                Debug.WriteLine($"Error occurred while trying to insert your session\n - Details: {e.Message}");
            }
        }

        public static List<CodingSession> ViewAllSessions()
        {
            var sessions = new List<CodingSession>();
            using var conn = new SqliteConnection(App.ConnectionString);
            string readQuery = "SELECT * FROM CodingSessions";
            try
            {
                var rawSessions = conn.Query(readQuery).ToList();

                foreach (var rawSession in rawSessions)
                {
                    CodingSession session = new CodingSession();
                    session.Id = (int)rawSession.Id;
                    session.StartTime = DateTime.ParseExact(rawSession.StartTime, App.DateFormat, null);
                    session.EndTime = DateTime.ParseExact(rawSession.EndTime, App.DateFormat, null);
                    session.Duration = TimeSpan.Parse(rawSession.Duration);
                    sessions.Add(session);
                }
            }
            catch (SqliteException e)
            {
                Debug.WriteLine($"Error occurred while trying to access your sessions\n - Details: {e.Message}");
            }
            return sessions;
        }

        public void UpdateSession(CodingSession session)
        {
            using var conn = new SqliteConnection(App.ConnectionString);
            string updateQuery = "UPDATE CodingSessions SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id";
            try
            {
                var parameters = new
                {
                    StartTime = session.StartTime.ToString(App.DateFormat),
                    EndTime = session.EndTime.ToString(App.DateFormat),
                    Duration = session.Duration.ToString(@"hh\:mm\:ss"),
                    Id = session.Id
                };
                int result = conn.Execute(updateQuery, parameters);

                if (result == 0)
                {
                    Debug.WriteLine($"No session found with the provided Id: {session.Id}");
                }
                else
                {
                    Debug.WriteLine($"Session with Id: {session.Id} successfully updated.");
                }
            }
            catch (SqliteException e)
            {
                Debug.WriteLine($"Error occurred while trying to update your session\n - Details: {e.Message}");
            }
        }

        public void DeleteSession(CodingSession session)
        {
            using var conn = new SqliteConnection(App.ConnectionString);
            string deleteQuery = "DELETE FROM CodingSessions WHERE Id = @Id";
            try
            {
                int result = conn.Execute(deleteQuery, session);

                if (result == 0)
                {
                    Debug.WriteLine($"No session found with the provided Id: {session.Id}");
                }
                else
                {
                    Debug.WriteLine($"Session with Id: {session.Id} successfully deleted.");
                }
            }
            catch (SqliteException e)
            {
                Debug.WriteLine($"Error occurred while trying to delete your session\n - Details: {e.Message}");
            }
        }
    }

