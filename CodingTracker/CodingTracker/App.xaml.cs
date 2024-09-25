using Microsoft.VisualBasic;

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

            ConnectionString = "Data Source=coding_tracker.db;Provider=Microsoft.Data.SQLite";
            DateFormat = "yyyy-MM-dd HH:mm";
            DatabasePath = "coding_tracker.db";

            MainPage = new AppShell();
        }
    }
}
