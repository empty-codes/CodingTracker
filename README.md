# Coding Tracker

## Overview

**Coding Tracker** is a cross-platform application built with .NET MAUI designed to log and manage coding sessions. Users can add, update, view, and delete coding sessions, as well as generate reports based on their recorded data. The application utilizes a SQLite database for data storage and Dapper ORM for data access.

![Flyout](PreviewImages/flyout.PNG)
![Home Page](PreviewImages/view.PNG)
![Filtered Sessions](PreviewImages/filteredview.PNG)
![Save/Delete Session](PreviewImages/session.PNG)
![Real-time Tracker](PreviewImages/tracker.PNG)
![Coding Goal](PreviewImages/goals.PNG)
![Report](PreviewImages/report.PNG)

## Requirements

### Functional Requirements

- **Add Coding Session**: Users can add new coding sessions with start and end times.
- **Update Coding Session**: Users can update existing coding sessions.
- **View All Sessions**: Users can view all recorded coding sessions.
- **Delete Coding Session**: Users can delete specific coding sessions.
- **View Report**: Users can view tailored reports of their sessions.

### Technical Requirements

- **Database**: SQLite
- **ORM**: Dapper
- **Configuration**: app.config for database connection and date format settings

## Features

- **CRUD Operations**: Create, Read, Update, and Delete operations for coding sessions.
- **Date and Time Validation**: Ensures correct date and time format and logical consistency (end time must be after start time).
- **Menu-driven Interface**: Provides a user-friendly console menu for interacting with the application.
- **Error Handling**: Handles exceptions and provides feedback for incorrect inputs or operations.

## Challenges Faced & Lessons Learned

- **First-time using XAML**: Adjusting to the XAML syntax and structure.
- **Difference between Web and Mobile/Desktop Apps**: Encountered significant differences in rendering content between web apps and mobile/desktop apps.
- **Transition from MVC to MVVM**: Initially struggled with properly organizing files within the MVVM pattern.
- **Loading Time**: Noted that MAUI apps generally take longer to load and can be more error-prone.
- **Database File Location**: Had difficulty locating the database file, which created uncertainty about its functionality.
- **UI Design**: Recognized that UI design is a skill that requires improvement.
- **First-time using async/await**: Gained practical experience with asynchronous programming in C#.

My biggest issue was setting up .NET MAUI; I encountered multiple installation issues with workloads. I had to uninstall Visual Studio three times before it finally worked. You can read more about my experience [here](https://emptycodesalsowrites.hashnode.dev/automating-msi-uninstall).

## Issues & Fixes
1. **Navigation to Coding Session Page**: When the "Add Session" button was clicked, it did not navigate to the coding session page.
   - **Fix**: Registered the page for navigation using routes in `AppShell.xaml.cs`.

2. **Session List Interaction**: Tapping a session from the list had no effect. I ended up splitting the start and end information into separate date and time entries for easier user input.
   - **Note**: The TimePicker in .NET MAUI only displays hours and minutes by default, despite setting the format to "HH:mm:ss".

3. **Real-time Tracker Session Visibility**: Sessions added through the real-time tracker did not appear in the "View All Sessions" list unless the program was closed and reopened.
   - **Fix**: Ensured the list of sessions is reloaded every time the AllSessions page is navigated to.

4. **Goal Page Crash**: The "Set Goals" page worked on desktop and during local device testing but crashed when accessed from the APK on a disconnected device.
   - **Debugging Steps**: Used ADB to retrieve logs and identified potential fixes:
     - Ensured the `GoalStatus` label is bound to a functional converter.
     - Registered the route for the goal page in `AppShell.xaml.cs`.
     - Cleared app data, uninstalled the APK, cleaned memory and storage, then reinstalled.

5. **Understanding IQueryAttributable**: Initially, I used `IQueryAttributable` without fully understanding its purpose, but learned that it helps manage navigation and query parameters effectively.