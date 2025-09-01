namespace TrayReminderApp;

public partial class EditReminders : Form
{
	// Custom members
	private NotifyIcon? trayIcon;
	private ContextMenuStrip? trayMenu;
	private System.Windows.Forms.Timer? reminderTimer;
	private List<Reminder> reminders = new();
	private string remindersPath = "reminders.json";

	public EditReminders()
	{
		InitializeComponent();
		InitializeTray();
		LoadReminders();
		SetupTimer();
		this.WindowState = FormWindowState.Minimized;
		this.ShowInTaskbar = false;
		this.Visible = false;
	}

	private void InitializeTray()
	{
		trayMenu = new ContextMenuStrip();
		trayMenu.Items.Add("Edit Reminders", null, OnEditReminders);
		trayMenu.Items.Add("Exit", null, OnExit);

		trayIcon = new NotifyIcon();
		trayIcon.Text = "TrayReminderApp";
		trayIcon.Icon = SystemIcons.Application;
		trayIcon.ContextMenuStrip = trayMenu;
		trayIcon.Visible = true;
		trayIcon.DoubleClick += (s, e) => OnEditReminders(s!, e);
	}

	private void LoadReminders()
	{
		if (File.Exists(remindersPath))
		{
			try
			{
				var json = File.ReadAllText(remindersPath);
				reminders = System.Text.Json.JsonSerializer.Deserialize<List<Reminder>>(json) ?? new();
			}
			catch
			{
				reminders = new();
			}
		}
		else
		{
			reminders = new();
		}
	}

	private void SetupTimer()
	{
		reminderTimer = new System.Windows.Forms.Timer();
		reminderTimer.Interval = 60000; // Check every minute
		reminderTimer.Tick += ReminderTimer_Tick;
		reminderTimer.Start();
	}

	private void ReminderTimer_Tick(object? sender, EventArgs e)
	{
		var now = DateTime.Now;
		foreach (var reminder in reminders)
		{
			if (reminder.LastShownDate != now.Date && now.ToString("HH:mm") == reminder.Time)
			{
				ShowReminder(reminder);
				reminder.LastShownDate = now.Date;
			}
		}
	}

	private void ShowReminder(Reminder reminder)
	{
		MessageBox.Show(reminder.Text, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Information);
	}

	private void OnEditReminders(object? sender, EventArgs e)
	{
		var editor = new RemindersEditor(reminders, (updatedReminders) => SaveReminders(updatedReminders));
		editor.ShowDialog();
	}

	private void SaveReminders(List<EditReminders.Reminder> updatedReminders)
	{
		reminders = updatedReminders;
		var json = System.Text.Json.JsonSerializer.Serialize(reminders);
		File.WriteAllText(remindersPath, json);
	}

	private void OnExit(object? sender, EventArgs e)
	{
		if (trayIcon != null) trayIcon.Visible = false;
		Application.Exit();
	}

	public class Reminder
	{
		public string Time { get; set; } = "10:00";
		public string Text { get; set; } = "Begin working!";
		[System.Text.Json.Serialization.JsonIgnore]
		public DateTime LastShownDate { get; set; } = DateTime.MinValue;
	}
}
