@echo off
REM Build the application
cd /d "%~dp0TrayReminderApp"
dotnet build TrayReminderApp.csproj -c Release

REM Create install directory
set INSTALL_DIR="C:\Program Files\TrayReminder"
if not exist %INSTALL_DIR% mkdir %INSTALL_DIR%

REM Copy files to install directory
xcopy /Y /E "bin\Release\net9.0-windows\*" %INSTALL_DIR%

REM Add to startup for current user
set STARTUP_PATH=%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup\TrayReminderApp.lnk
powershell -Command "$s = (New-Object -ComObject WScript.Shell).CreateShortcut('%STARTUP_PATH%'); $s.TargetPath = '%INSTALL_DIR%\TrayReminderApp.exe'; $s.Save()"

echo Installation complete. The app will run at startup.
pause
