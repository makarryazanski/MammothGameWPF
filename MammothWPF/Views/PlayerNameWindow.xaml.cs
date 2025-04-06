using System.Windows;
using MammothWPF.Models;

namespace MammothWPF.Views
{
	public partial class PlayerNameWindow : Window
	{
		public PlayerNameWindow()
		{
			InitializeComponent();
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			User.Instance.Name = PlayerNameTextBox.Text;
			this.DialogResult = true;
			this.Close();
		}
	}
}