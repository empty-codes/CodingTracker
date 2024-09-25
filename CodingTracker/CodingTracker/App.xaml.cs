using Microsoft.Data.Sqlite;
using Dapper;
using System.Diagnostics;

namespace CodingTracker
{
    public partial class App : Application
    {
        public static string? ConnectionString { get; private set; }
        public static string? DateFormat { get; private set; }
        public static string? DatabasePath { get; private set; } 
        public App()
        {
            InitializeComponent();

            CreateDatabase();
            MainPage = new AppShell();
        }

        private void CreateDatabase()
        {
            DateFormat = "yyyy-MM-dd HH:mm";
            DatabasePath = Path.Combine(FileSystem.AppDataDirectory, "coding_tracker.db");
            ConnectionString = "Data Source={DatabasePath}";

            if (File.Exists(DatabasePath))
            {
                Debug.WriteLine($"Database file {DatabasePath} already exists.");
            }
            else
            {
                using var conn = new SqliteConnection(ConnectionString);

                var createTableQuery = @"
                CREATE TABLE IF NOT EXISTS CodingSessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT NOT NULL,
                    EndTime TEXT NOT NULL,
                    Duration TEXT NOT NULL
                );";
                try
                {
                    conn.Execute(createTableQuery);
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DatabasePath);
                    Debug.WriteLine($"Database file {DatabasePath} successfully created at {dbPath}. The database is ready to use.");
                }
                catch (SqliteException e)
                {
                    Debug.WriteLine($"Error occurred while trying to create the database Table\n - Details: {e.Message}");
                }
            }
        }
    }
}
