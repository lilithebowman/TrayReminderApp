using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TrayReminderApp
{
	public partial class RemindersEditor : Form
	{
		private List<EditReminders.Reminder> reminders;
		private Action<List<EditReminders.Reminder>> onSave;

		private ListBox? listBox;
		private Button? addButton;
		private Button? editButton;
		private Button? deleteButton;
		private Button? saveButton;

		public RemindersEditor(List<EditReminders.Reminder> reminders, Action<List<EditReminders.Reminder>> onSave)
		{
			this.reminders = new List<EditReminders.Reminder>(reminders);
			this.onSave = onSave;
			InitializeEditorUI();
		}

		private void InitializeEditorUI()
		{
			this.Text = "Edit Reminders";
			this.Size = new System.Drawing.Size(400, 300);

			listBox = new ListBox { Dock = DockStyle.Top, Height = 180 };
			RefreshList();
			this.Controls.Add(listBox);

			addButton = new Button { Text = "Add", Left = 10, Top = 200, Width = 80 };
			addButton.Click += (s, e) => AddReminder();
			this.Controls.Add(addButton);

			editButton = new Button { Text = "Edit", Left = 100, Top = 200, Width = 80 };
			editButton.Click += (s, e) => EditReminder();
			this.Controls.Add(editButton);

			deleteButton = new Button { Text = "Delete", Left = 190, Top = 200, Width = 80 };
			deleteButton.Click += (s, e) => DeleteReminder();
			this.Controls.Add(deleteButton);

			saveButton = new Button { Text = "Save", Left = 280, Top = 200, Width = 80 };
			saveButton.Click += (s, e) => SaveReminders();
			this.Controls.Add(saveButton);
		}

		private void RefreshList()
		{
			if (listBox != null)
			{
				listBox.Items.Clear();
				foreach (var r in reminders)
					listBox.Items.Add($"{r.Time} - {r.Text}");
			}
		}

		private void AddReminder()
		{
			var dialog = new ReminderDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				reminders.Add(dialog.Reminder);
				RefreshList();
			}
		}

		private void EditReminder()
		{
			if (listBox == null || listBox.SelectedIndex < 0) return;
			var dialog = new ReminderDialog(reminders[listBox.SelectedIndex]);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				reminders[listBox.SelectedIndex] = dialog.Reminder;
				RefreshList();
			}
		}

		private void DeleteReminder()
		{
			if (listBox == null || listBox.SelectedIndex < 0) return;
			reminders.RemoveAt(listBox.SelectedIndex);
			RefreshList();
		}

		private void SaveReminders()
		{
			onSave(reminders);
			this.Close();
		}
	}

	public class ReminderDialog : Form
	{
		public EditReminders.Reminder Reminder { get; private set; }
		private TextBox? timeBox;
		private TextBox? textBox;
		private Button? okButton;

		public ReminderDialog(EditReminders.Reminder? reminder = null)
		{
			Reminder = reminder is not null ? new EditReminders.Reminder { Time = reminder.Time, Text = reminder.Text } : new EditReminders.Reminder();
			InitializeDialogUI();
		}

		private void InitializeDialogUI()
		{
			this.Text = "Reminder";
			this.Size = new System.Drawing.Size(300, 150);

			Label timeLabel = new Label { Text = "Time (HH:mm):", Left = 10, Top = 10, Width = 100 };
			this.Controls.Add(timeLabel);
			timeBox = new TextBox { Left = 120, Top = 10, Width = 150, Text = Reminder.Time };
			this.Controls.Add(timeBox);

			Label textLabel = new Label { Text = "Text:", Left = 10, Top = 40, Width = 100 };
			this.Controls.Add(textLabel);
			textBox = new TextBox { Left = 120, Top = 40, Width = 150, Text = Reminder.Text };
			this.Controls.Add(textBox);

			okButton = new Button { Text = "OK", Left = 100, Top = 80, Width = 80 };
			okButton.Click += (s, e) => { Reminder.Time = timeBox.Text; Reminder.Text = textBox.Text; this.DialogResult = DialogResult.OK; this.Close(); };
			this.Controls.Add(okButton);
		}
	}
}
