using System.Windows;
using System.Windows.Input;

namespace MammothHunting.Views
{
	public partial class HelpWindow : Window
	{
		public HelpWindow()
		{
			InitializeComponent();
			this.WindowState = WindowState.Maximized; // Развернуть на весь экран
			this.WindowStyle = WindowStyle.None; // Убрать рамки окна
			this.KeyDown += HelpWindow_KeyDown; // Подписаться на событие нажатия клавиш
		}

		private void HelpWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				this.Close(); // Закрыть окно при нажатии ESC
			}
		}
	}
}