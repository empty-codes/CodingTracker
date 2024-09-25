using Microsoft.Data.Sqlite;
using Dapper;

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
            ConnectionString = "Data Source=coding_tracker.db;Provider=Microsoft.Data.SQLite";
            DateFormat = "yyyy-MM-dd HH:mm";
            DatabasePath = "coding_tracker.db";

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
                Console.WriteLine($"Database file {DatabasePath} successfully created. The database is ready to use.");
            }
            catch (SqliteException e)
            {
                 Console.WriteLine($"Error occurred while trying to create the database Table\n - Details: {e.Message}");
            }
            
        }
    }
}
