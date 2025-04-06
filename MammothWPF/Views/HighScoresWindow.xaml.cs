using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using MammothWPF.Models;

namespace MammothWPF.Views
{
	public partial class HighScoresWindow : Window
	{
		private const string RecordsFilePath = "records.json";

		public HighScoresWindow()
		{
			InitializeComponent();
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			try
			{
				if (File.Exists(RecordsFilePath))
				{
					var json = File.ReadAllText(RecordsFilePath);
					var options = new JsonSerializerOptions
					{
						Converters = { new RecordCollectionJsonConverter() },
						PropertyNameCaseInsensitive = true
					};

					var records = JsonSerializer.Deserialize<RecordCollection>(json, options)
								  ?? new RecordCollection();

					records.Sort();
					HighScoresListView.ItemsSource = records;
				}
				else
				{
					HighScoresListView.ItemsSource = new RecordCollection();
					MessageBox.Show("No records found. Play the game to set some records!",
								  "No Data",
								  MessageBoxButton.OK,
								  MessageBoxImage.Information);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to load records: {ex.Message}",
							   "Error",
							   MessageBoxButton.OK,
							   MessageBoxImage.Error);
			}
		}
	}
}