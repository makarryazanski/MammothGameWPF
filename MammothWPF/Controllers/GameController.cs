using System.Diagnostics;
using System.Windows;
using MammothWPF.Views;

namespace MammothWPF.Controllers
{
    public class GameController
    {
        public MainWindow _mainWindow;
        public GameWindow gameWindow;

        public GameController(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            gameWindow = null!;
        }

        public void StartNewGame()
        {
            Debug.WriteLine("Запуск новой игры");
            var gameWindow = new GameWindow();
            gameWindow.WindowState = WindowState.Maximized; // Устанавливаем полноэкранный режим
            gameWindow.Show();
            _mainWindow.Close(); // Закрываем старое окно
        }
    }
}