# TrayReminderApp

TrayReminderApp is a Windows system tray application that displays scheduled reminders as dialog boxes. You can edit reminders, and the app will run automatically at startup.

## Features

- Runs in the system tray
- Displays reminders from `reminders.json` at scheduled times
- Dialogs with dismiss button
- Edit reminders via tray menu
- Installs to `C:\Program Files\TrayReminder` and auto-starts with Windows

## Installation

1. Run `install_trayreminder.bat` to build, install, and add the app to startup.
2. The app will appear in your system tray and show reminders as configured.

## Editing Reminders

- Right-click the tray icon and select "Edit Reminders" to modify your reminders.
- Reminders are stored in `reminders.json` in the install directory.

## Uninstall

- Remove the shortcut from your Startup folder and delete the install directory.

---

For questions or improvements, open an issue in this repository.
