using MammothWPF.Controllers;
using MammothWPF.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MammothWPF.Views
{
	public partial class GameWindow : Window
	{
		private const int MapWidth = 510;
		private const int MapHeight = 310;
		private const int FrameMs = 300;
		private const int MaxScore = 1000;
		private int score = MaxScore;

		private Hunter _hunter;
		private Mammoth _mammoth;
		private MammothMovement _mammothMovement;
		private Direction _direction;
		private bool _isThrown;
		private Action _onHit;
		private Stopwatch _gameTimer;

		public GameWindow()
		{
			InitializeComponent();
			Loaded += OnLoaded; // Подключаем обработчик событий для загрузки окна

			// Инициализация полей
			_hunter = null!;
			_mammoth = null!;
			_mammothMovement = null!;
			_isThrown = false;
			_onHit = EndGame;
			_gameTimer = new Stopwatch();
		}

		/// <summary>
		/// Окно загружено
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			InitializeGame();
			StartGame();
		}

		private void InitializeGame()
		{
			_hunter = new Hunter(GameCanvas, 50, 50, Brushes.Khaki, Brushes.LightYellow, Brushes.SandyBrown);
			_mammoth = new Mammoth(GameCanvas, 200, 200, Brushes.Gray, Brushes.White);
			_mammothMovement = new MammothMovement((int)GameCanvas.ActualWidth, (int)GameCanvas.ActualHeight);

			this.KeyDown += Window_KeyDown; // Подключаем обработчик событий для нажатия клавиш
		}

		/// <summary>
		/// Метод для запуска игры
		/// </summary>
		private async void StartGame()
		{
			_gameTimer.Start();
			// Запускаем движение мамонта
			await _mammothMovement.MoveMammoth(_mammoth);
		}

		private void EndGame()
		{
			UpdateScore();
			GameCanvas.Children.Clear();
			MessageBox.Show($"Game Over! Your score: {score}");
		}

		/// <summary>
		/// Обработчик нажатия клавиш
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			MammothWPF.Models.Point target = new MammothWPF.Models.Point(Canvas.GetLeft(_hunter.Head), Canvas.GetTop(_hunter.Head));

			switch (e.Key)
			{
				case Key.Up:
					target.Y -= 10;
					_direction = Direction.Up;
					break;
				case Key.Down:
					target.Y += 10;
					_direction = Direction.Down;
					break;
				case Key.Left:
					target.X -= 10;
					_direction = Direction.Left;
					break;
				case Key.Right:
					target.X += 10;
					_direction = Direction.Right;
					break;
				case Key.Space:
					Throw(_direction);
					break;
				case Key.Escape:  // Добавлен выход по Escape
					new MainWindow().Show();
					this.Close();
					return;
			}

			_hunter.Move(new MammothWPF.Models.Point(target.X, target.Y));
		}

		private List<Spear> _spears = new List<Spear>(); // Список летящих копий

		public void Throw(Direction direction)
		{
			// Создаем копье в позиции охотника
			var spear = new Spear(_hunter._canvas, Canvas.GetLeft(_hunter.Head) + 40, Canvas.GetTop(_hunter.Head) + 20, Brushes.SandyBrown);
			_spears.Add(spear);

			// Запускаем перемещение копья
			Task.Run(() => MoveSpear(spear, direction));
		}


		private void UpdateScore()
		{
			int elapsed = (int)_gameTimer.Elapsed.TotalSeconds;
			score = 1000 - Math.Max(0, elapsed - 30) * 10 - Math.Min(30, elapsed) * 10;
		}

		private async Task MoveSpear(Spear spear, Direction direction)
		{
			int deltaX = 0;
			int deltaY = 0;

			switch (direction)
			{
				case Direction.Up: deltaY = -20; break;
				case Direction.Down: deltaY = 20; break;
				case Direction.Left: deltaX = -20; break;
				case Direction.Right: deltaX = 20; break;
			}

			while (true)
			{
				await Task.Delay(40); // Копье двигается раз в 100 мс

				Application.Current.Dispatcher.Invoke(() =>
				{
					double newLeft = Canvas.GetLeft(spear.Shape) + deltaX;
					double newTop = Canvas.GetTop(spear.Shape) + deltaY;

					// Проверка выхода за границы
					if (newLeft < 0 || newLeft >= _hunter._canvas.ActualWidth ||
							  newTop < 0 || newTop >= _hunter._canvas.ActualHeight)
					{
						_hunter._canvas.Children.Remove(spear.Shape);
						_spears.Remove(spear);
						return;
					}

					// Перемещаем копье
					Canvas.SetLeft(spear.Shape, newLeft);
					Canvas.SetTop(spear.Shape, newTop);

					// Проверяем попадание в мамонта
					if (IsHit(spear))
					{
						_hunter._canvas.Children.Remove(spear.Shape);
						_spears.Remove(spear);
						_onHit?.Invoke();
						return;
					}
				});
			}
		}

		// булево есть попадание или нет
		private bool IsHit(Spear spear)
		{
			// Получаем координаты и размеры копья
			Rect spearRect = new Rect(
				Canvas.GetLeft(spear.Shape),
				Canvas.GetTop(spear.Shape),
				spear.Shape.Width,
				spear.Shape.Height);

			// Проверка попадания в голову
			Rect headRect = new Rect(
				Canvas.GetLeft(_mammoth.Head),
				Canvas.GetTop(_mammoth.Head),
				_mammoth.Head.Width,
				_mammoth.Head.Height);

			if (spearRect.IntersectsWith(headRect)) return true;


			// Проверка попадания в тело
			foreach (var part in _mammoth.Body)
			{
				Rect bodyPartRect = new Rect(
					Canvas.GetLeft(part),
					Canvas.GetTop(part),
					part.Width,
					part.Height);

				if (spearRect.IntersectsWith(bodyPartRect)) return true;
			}

			// Проверка попадания в бивни
			foreach (var tusk in _mammoth.Tusk)
			{
				Rect tuskRect = new Rect(
					Canvas.GetLeft(tusk),
					Canvas.GetTop(tusk),
					tusk.Width,
					tusk.Height);

				if (spearRect.IntersectsWith(tuskRect)) return true;
			}

			return false;
		}
	}
}