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

4. The set goals page worked perfectly on desktop, also when i ran the program in vs with my android phone connected as a local device, but when i disconnected it, and opened the apk left on my phone, anytime i tried to open the set goals page, the app crashed.
Tried to access the db path but it was stored in a hidden/restricted folder that simply did not show
Used adb to get the logs of the error and debug that way, but the logs didnt saw much, did a couple changes then it started working so any of these could have been the fix:
1. For example, the GoalStatus label is bound to a converter (StringNotEmptyConverter). Ensure the converter exists in your resource dictionary, and it's functioning correctly. so instead i  you can handle the visibility logic directly in your ViewModel by using a boolean property that determines whether the GoalStatus is visible. 
2. register the route in your AppShell.xaml.cs file for the goal page (or any other pages) to ensure that your application can navigate to them correctly.
3. cleared all app data, then unistalled the apk, then cleaned my memory and storage, then reinstalled