# CodingTracker


## Challenges faced + Lessons Learned

-My first time using XAML
-Writing web apps vs desktop/mobile apps is vastly different especially in rendering of content
-My first time trying to use MVVM instead of MVC , found it hard to properly choose where to place each file
-MAUI apps take a longer time to load and are more error prone
-Hard to find my db file when i created it so I wasn't sure if it was working or not
-I'm not so good with UI design


Issues
1. When the add session button was clicked, it was meant to navigate to the coding session page but nothing happened:
Fix: Registered the page for navigation using routes in AppShell.xaml.cs

2. When i tap a session from list nothing happens, ended up splitting start and end info date and time entries for easier user input

The TimePicker in .NET MAUI is designed to only display hours and minutes by default, which is why you’re not seeing seconds in the UI even though you've set the format to "HH:mm:ss

3. When a session is added through the realtime tracker, it does not show up in the view all sessions list, even though succesfully added to db unless program closed and reopened
Fix: Ensures list of sessions are reloaded everyime the AllSessions page is navigated to