using MammothHunting.Views;
using MammothWPF.Controllers;
using MammothWPF.Models;
using System.Collections.Generic;
using System.Windows;

namespace MammothWPF.Views
{
	public partial class MainWindow : Window
	{
		private readonly GameController _gameController;
		private readonly RecordCollection _recordCollection;

		public MainWindow()
		{
			InitializeComponent();
			_gameController = new GameController(this);
			_recordCollection = new RecordCollection();
			_recordCollection.Sort();
		}

		private void NewGameButton_Click(object sender, RoutedEventArgs e)
		{
			_gameController.StartNewGame();
		}

		private void HighScoresButton_Click(object sender, RoutedEventArgs e)
		{
			var highScoresWindow = new HighScoresWindow();
			highScoresWindow.ShowDialog();
		}

		private void PlayerButton_Click(object sender, RoutedEventArgs e)
		{
			PlayerNameWindow playerNameWindow = new PlayerNameWindow();
			if (playerNameWindow.ShowDialog() == true)
			{
				MessageBox.Show($"Имя игрока: {User.Instance.Name}");
			}
		}

		private void HelpButton_Click(object sender, RoutedEventArgs e)
		{
			var helpWindow = new HelpWindow();
			helpWindow.ShowDialog();
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}